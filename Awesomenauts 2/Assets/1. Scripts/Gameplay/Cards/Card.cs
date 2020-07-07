using System;
using System.Collections;
using AwsomenautsCardGame.AnimationSystem;
using AwsomenautsCardGame.Audio;
using AwsomenautsCardGame.DataObjects.Networking;
using AwsomenautsCardGame.Enums.Cards;
using AwsomenautsCardGame.Events.Gameplay;
using AwsomenautsCardGame.Networking;
using AwsomenautsCardGame.ScriptableObjects.DragLogic;
using AwsomenautsCardGame.ScriptableObjects.Effects;
using AwsomenautsCardGame.UI.Cards;
using VDFramework.SharedClasses.EventSystem;
using Mirror;
using UnityEngine;
using UnityEngine.UI;
using EventType = AwsomenautsCardGame.Enums.Audio.EventType;
using Object = System.Object;

namespace AwsomenautsCardGame.Gameplay.Cards
{
	public class Card : NetworkBehaviour
	{
		private int lastHp;

		public bool IsLocked { get; private set; }
		public object Locker { get; private set; }
		public Transform[] CardParts;
		public Image CardImage;
		public Text CardName;
		public Text CardDescription;

		public Transform Model;

		public EffectManager EffectManager;

		public MeshFilter CardBorderFilter;
		public MeshRenderer CardBorderRenderer;
		public MeshRenderer CoverUpRenderer;

		public CardSocket AttachedCardSocket;

		public CardDragLogic DragLogicFromBoard;

		public EntityStatistics Statistics;
		public bool StatisticsValid { get; private set; }
		public CardState CardState { get; private set; } = CardState.OnDeck;
		public CardType CardType => Statistics.GetValue<CardType>(CardPlayerStatType.CardType);

		public AnimationPlayer Animator;

		// Start is called before the first frame update
		private void Start()
		{

			//Instanciate Animator in child objects if any
			if (Animator == null)
			{
				Animator = GetComponentInChildren<AnimationPlayer>();
			}

		}

		public void Lock(bool lockState, object locker)
		{
			IsLocked = lockState;
			if (IsLocked)
				Locker = locker;
		}


		public void SetPreviewLayer(bool set)
		{
			for (int i = 0; i < CardParts.Length; i++)
			{
				SetChildLayer(CardParts[i], set);
			}
		}

		private void SetChildLayer(Transform c, bool set)
		{
			if (c == null) return;
			c.gameObject.layer = set ? 13 : 0;
			for (int i = 0; i < c.childCount; i++)
			{
				Transform child = c.GetChild(i);
				SetChildLayer(child, set);
			}
		}


		public void SetSocket(CardSocket socket)
		{
			if (Model != null && AttachedCardSocket != null)
			{
				Model.gameObject.SetActive(false);
			}
			AttachedCardSocket = socket;
			if (Model != null && AttachedCardSocket != null)
			{
				Model.gameObject.SetActive(true);
			}
		}

		[ClientRpc]
		public void RpcSendStats(byte[] data, int[] effects)
		{
			ApplyStatistics(data);
			EffectManager.Effects = new System.Collections.Generic.List<AEffect>();
			foreach (int effect in effects)
			{
				//Debug.Log("Adding effect: " + effect);
				EffectManager.Effects.Add(CardNetworkManager.Instance.AllEffects[effect]);
			}
		}

		private void ApplyStatistics(byte[] data)
		{
			//Debug.Log("Received Stats");
			EntityStatistics stat = CardEntry.FromNetwork(data);



			stat.ReregisterEvents(Statistics);

			if (Statistics.HasValue(CardPlayerStatType.Solar) && stat.HasValue(CardPlayerStatType.Solar) && stat.GetValue<int>(CardPlayerStatType.Solar) != Statistics.GetValue<int>(CardPlayerStatType.Solar))
			{
				stat.SetValue(CardPlayerStatType.Solar, Statistics.GetValue<int>(CardPlayerStatType.Solar));
			}

			Statistics = stat;
			StatisticsValid = true;
			SetCoverState(CardPlayer.LocalPlayer != null && Statistics.GetValue<int>(CardPlayerStatType.TeamID) != CardPlayer.LocalPlayer.ClientID);
			CardName.text = Statistics.GetValue<string>(CardPlayerStatType.CardName) ?? "";
			CardImage.sprite = CardNetworkManager.Instance.GetCardImage(CardName.text, Statistics.GetValue<int>(CardPlayerStatType.TeamID));
			BorderInfo bi = CardNetworkManager.Instance.GetCardBorder(CardName.text, stat.GetValue<int>(CardPlayerStatType.TeamID));
			CardBorderFilter.mesh = bi.BorderMesh;
			CardType type = (CardType)stat.GetValue<int>(CardPlayerStatType.CardType);
			CardBorderRenderer.sharedMaterial = bi.GetMaterial(type);
			EffectManager = new EffectManager(CardNetworkManager.Instance.GetCardEffects(CardName.text));
			CardDescription.text = EffectManager.GetEffectText();

			if (Model != null)
			{
				if (Model.childCount == 0)
				{
					GameObject modelPrefab = CardNetworkManager.Instance.GetCardModel(
						Statistics.GetValue<string>(CardPlayerStatType.CardName),
						Statistics.GetValue<int>(CardPlayerStatType.TeamID));
					if (modelPrefab != null)
					{
						GameObject model = Instantiate(modelPrefab, Model.position, modelPrefab.transform.rotation);
						model.transform.SetParent(Model, true);

						if (Animator == null)
						{
							Animator = Model.GetComponentInChildren<AnimationPlayer>();
						}
					}
				}
				else
				{
					GameObject model = Model.GetChild(0).gameObject;
					if (Animator == null)
					{
						Animator = Model.GetComponentInChildren<AnimationPlayer>();
					}
				}
			}


			CardTextHelper cth = GetComponentInChildren<CardTextHelper>();
			cth.Register(this);

			Statistics.Register(CardPlayerStatType.HP, OnHPChanged);
			Statistics.Invalidate();
			if (!Statistics.HasValue(CardPlayerStatType.Range)) Statistics.SetValue(CardPlayerStatType.Range, 1);
			if (!Statistics.HasValue(CardPlayerStatType.CrossLaneRange)) Statistics.SetValue(CardPlayerStatType.CrossLaneRange, 0);
		}


		private IEnumerator WaitForUnlock(Action action)
		{
			while (IsLocked) yield return 1;
			action();
		}
		private void OnDestroy()
		{
			Statistics.UnregisterAll();
		}

		private void OnHPChanged(object newvalue)
		{
			if (newvalue == null || !(newvalue is int hp))
				return;

			if (hp <= 0)
			{
				EffectManager.InvokeEffects(EffectTrigger.OnDeath, AttachedCardSocket, null, this);
				StartCoroutine(WaitForUnlock(GoCommitDie));
				//GoCommitDie(); //Might move this into an effect class
			}
			else if (lastHp > hp) //Damage has been done
			{
				EffectManager.InvokeEffects(EffectTrigger.OnAttacked, AttachedCardSocket, null, this);
			}
			lastHp = hp;
		}

		public void GoCommitDie()
		{
			if (AttachedCardSocket != null)
			{
				if (AttachedCardSocket.hasAuthority)
				{
					AttachedCardSocket.CmdUnDockCard();
				}
				else
				{
					AttachedCardSocket.DockCard(null);
				}
			}
			Destroy(gameObject);
		}

		/// <summary>
		/// Sets the Cover Up Renderer Active or Inactive.
		/// </summary>
		/// <param name="covered"></param>
		public void SetCoverState(bool covered)
		{
			////Debug.Log("Enable Cover Up Renderer: " + covered);
			CoverUpRenderer.enabled = covered;
		}

		public void SetState(CardState state)
		{
			if (CardPlayer.LocalPlayer == null) return;
			CardState = state;

			if (state == CardState.OnBoard)
			{
				bool fromHand = gameObject.layer == CardPlayer.LocalPlayer.Hand.PlayerHandLayer;
				gameObject.layer = CardPlayer.UnityTrashWorkaround(CardPlayer.LocalPlayer.BoardLayer);
				SetCoverState(false);
				//Reverse the Turning over
				if (!fromHand)
				{
					Quaternion turnOverRot = Quaternion.AngleAxis(-180, transform.up);
					transform.rotation *= turnOverRot;
				}

			}
		}


		public void Attack(Card other)
		{
			//Debug.Log("ATTACK");

			int hpOwn = Statistics.GetValue<int>(CardPlayerStatType.HP);
			int atkOwn = Statistics.GetValue<int>(CardPlayerStatType.Attack);
			int hpOther = other.Statistics.GetValue<int>(CardPlayerStatType.HP);
			int atkOther = other.Statistics.GetValue<int>(CardPlayerStatType.Attack);
			Statistics.SetValue(CardPlayerStatType.HP, hpOwn - atkOther);
			other.Statistics.SetValue(CardPlayerStatType.HP, hpOther - atkOwn);

			if (Statistics.GetValue<CardType>(CardPlayerStatType.CardType) == CardType.Action)
			{
				Destroy(gameObject);
			}
			else
			{
				EventManager.Instance.RaiseEvent(new CardAttackEvent(this, other));
			}

		}

	}
}