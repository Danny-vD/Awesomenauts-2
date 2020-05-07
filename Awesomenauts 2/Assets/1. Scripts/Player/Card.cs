using Assets._1._Scripts.ScriptableObjects.DragLogic;
using Assets._1._Scripts.ScriptableObjects.Effects;
using Maps;
using Networking;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

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

		// Start is called before the first frame update
		void Start()
		{
			SetCoverState(isClient && !hasAuthority);
		}

		public void SetSocket(CardSocket socket)
		{
			AttachedCardSocket = socket;
		}

		[ClientRpc]
		public void RpcSendStats(byte[] data)
		{
			ApplyStatistics(data);
		}

		private void ApplyStatistics(byte[] data)
		{
			Debug.Log("Received Stats");
			Statistics = CardEntry.FromNetwork(data);
			StatisticsValid = true;
			CardName.text = Statistics.GetValue<string>(CardPlayerStatType.CardName) ?? "";
			CardImage.sprite = CardNetworkManager.Instance.GetCardImage(CardName.text);
			BorderInfo bi = CardNetworkManager.Instance.GetCardBorder(CardName.text);
			CardBorderFilter.mesh = bi.BorderMesh;
			CardBorderRenderer.materials[0] = bi.BorderMaterial;
			EffectManager = new EffectManager(CardNetworkManager.Instance.GetCardEffects(CardName.text));
			CardDescription.text = EffectManager.GetEffectText();
			Statistics.Register(CardPlayerStatType.HP, OnHPChanged);
			Statistics.Invalidate();
		}


		private void OnHPChanged(object newvalue)
		{
			if (newvalue == null || !(newvalue is int hp)) return;
			if (hp <= 0)
			{
				EffectManager.TriggerEffects(EffectTrigger.OnDeath, AttachedCardSocket, null);
				GoCommitDie(); //Might move this into an effect class
			}
		}

		public void GoCommitDie()
		{
			AttachedCardSocket?.DockCard(null);
			Destroy(gameObject);
		}

		/// <summary>
		/// Sets the Cover Up Renderer Active or Inactive.
		/// </summary>
		/// <param name="covered"></param>
		public void SetCoverState(bool covered)
		{
			//Debug.Log("Enable Cover Up Renderer: " + covered);
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
			Debug.Log("ATTACK");

			int hpOwn = Statistics.GetValue<int>(CardPlayerStatType.HP);
			int atkOwn = Statistics.GetValue<int>(CardPlayerStatType.Attack);
			int hpOther = other.Statistics.GetValue<int>(CardPlayerStatType.HP);
			int atkOther = other.Statistics.GetValue<int>(CardPlayerStatType.Attack);
			Statistics.SetValue(CardPlayerStatType.HP, hpOwn - atkOther);
			other.Statistics.SetValue(CardPlayerStatType.HP, hpOther - atkOwn);

		}

	}
}