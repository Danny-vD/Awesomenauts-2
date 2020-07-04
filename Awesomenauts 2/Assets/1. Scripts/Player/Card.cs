using System.Diagnostics;
using Assets._1._Scripts.ScriptableObjects.DragLogic;
using Assets._1._Scripts.ScriptableObjects.Effects;
using Enums.Cards;
using Events.Gameplay;
using Maps;
using Networking;
using Mirror;
using UI.Cards;
using UnityEngine;
using UnityEngine.UI;
using VDFramework.EventSystem;
using Debug = UnityEngine.Debug;

namespace Player
{
	public class Card : NetworkBehaviour
	{

		public Image CardImage;
		public Text CardName;
		public Text CardDescription;


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

		public Animator Animator;

		// Start is called before the first frame update
		void Start()
		{
			SetCoverState(isClient && !hasAuthority);

			//Instanciate Animator in child objects if any
			Animator = GetComponentInChildren<Animator>();

		}

		public void SetPreviewLayer(bool set)
		{
			if (this == null)
			{
				return;
			}

			for (int i = 0; i < transform.childCount; i++)
			{
				Transform child = transform.GetChild(i);
				SetChildLayer(child, set);
			}
		}

		private void SetChildLayer(Transform c, bool set)
		{
			c.gameObject.layer = set ? 13 : 0;
			for (int i = 0; i < c.childCount; i++)
			{
				Transform child = c.GetChild(i);
				SetChildLayer(child, set);
			}
		}


		public void SetSocket(CardSocket socket)
		{
			AttachedCardSocket = socket;
		}

		[ClientRpc]
		public void RpcSendStats(byte[] data, int[] effects)
		{
			ApplyStatistics(data);
			foreach (int effect in effects)
			{
				//Debug.Log("Adding effect: " + effect);
				EffectManager.Effects = new System.Collections.Generic.List<AEffect>();
				EffectManager.Effects.Add(CardNetworkManager.Instance.AllEffects[effect]);
			}
		}

		private void ApplyStatistics(byte[] data)
		{
			//Debug.Log("Received Stats");
			EntityStatistics stat = CardEntry.FromNetwork(data);
			stat.ReregisterEvents(Statistics);
			Statistics = stat;
			StatisticsValid = true;
			CardName.text = Statistics.GetValue<string>(CardPlayerStatType.CardName) ?? "";
			CardImage.sprite = CardNetworkManager.Instance.GetCardImage(CardName.text);
			BorderInfo bi = CardNetworkManager.Instance.GetCardBorder(CardName.text);
			CardBorderFilter.mesh = bi.BorderMesh;
			CardBorderRenderer.materials[0] = bi.BorderMaterial;
			EffectManager = new EffectManager(CardNetworkManager.Instance.GetCardEffects(CardName.text));
			CardDescription.text = EffectManager.GetEffectText();

			CardTextHelper cth = GetComponentInChildren<CardTextHelper>();
			cth.Register(this);

			Statistics.Register(CardPlayerStatType.HP, OnHPChanged);
			Statistics.Invalidate();
			if (!Statistics.HasValue(CardPlayerStatType.Range)) Statistics.SetValue(CardPlayerStatType.Range, 1);
			if (!Statistics.HasValue(CardPlayerStatType.CrossLaneRange)) Statistics.SetValue(CardPlayerStatType.CrossLaneRange, 0);
		}


		private void OnHPChanged(object newvalue)
		{
			if (newvalue == null || !(newvalue is int hp))
				return;
			if (hp <= 0)
			{
				EffectManager.InvokeEffects(EffectTrigger.OnDeath, AttachedCardSocket, null, this);
				GoCommitDie(); //Might move this into an effect class
			}
			else
			{
				Debug.Log("");
			}
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
			CardState = state;

			if (state == CardState.OnBoard)
			{
				SetCoverState(false);
				//Reverse the Turning over
				Quaternion turnOverRot = Quaternion.AngleAxis(-180, transform.up);
				transform.rotation *= turnOverRot;
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