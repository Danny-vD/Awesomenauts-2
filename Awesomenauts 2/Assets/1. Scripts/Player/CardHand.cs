using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace Player {
	public class CardHand : NetworkBehaviour
	{

		public Vector3 Anchor;
		private List<Card> CardsOnHand = new List<Card>();
		private Card SelectedCard;


		[Range(1, 10)]
		public int MaxCardCount = 5;

		public int CardSlotsFree => MaxCardCount - CardsOnHand.Count;

		[Range(0f, 180f)]
		public float MaxCardRotation = 45;
		[Range(-10, 10)]
		public float OffsetFromAnchor = 5;
		[Range(0, 1)]
		public float Drag = 0.1f;
		[Range(-10, 10)]
		public float AnchorOffset;
		[Range(-1, 1)]
		public float DeltaYPerCard;
		[Range(-1, 1)]
		public float DeltaYLocalPerCard;
		public LayerMask PlayerHandLayer;

		public void InvertValues()
		{
			AnchorOffset *= -1;
		}

		public void SetSelectedCard(Card card)
		{
			SelectedCard = card;
		}


		private void SetCardTransform(Transform cardTransform, int i, Vector3 anchor, float yOffsetFromAnchor, float deltaYLocal)
		{
			Vector3 centerPosition = anchor + cardTransform.up * yOffsetFromAnchor + Vector3.left * AnchorOffset;
			float rotation = (i + 0.5f) / CardsOnHand.Count;
			rotation -= 0.5f;
			rotation *= 2;
			rotation *= MaxCardRotation;
			cardTransform.rotation = Quaternion.identity;


			Vector3 oldCardPos = cardTransform.position;


			cardTransform.position = centerPosition;
			cardTransform.RotateAround(anchor, Vector3.down, rotation);
			Vector3 newCardPos = cardTransform.position;
			cardTransform.position = Vector3.Lerp(oldCardPos, newCardPos, Drag);

			cardTransform.LookAt(Camera.main.transform);

			cardTransform.position += transform.up * deltaYLocal;
		}

		[TargetRpc]
		public void TargetAddToHand(NetworkConnection target, NetworkIdentity id)
		{
			Debug.Log("Adding cards to the hand of client: " + GetComponent<CardPlayer>().ClientID);

			AddToHand(id);
		}

		public void AddToHand(NetworkIdentity id)
		{
			Card c = id.GetComponent<Card>();
			if (c != null)
			{
				c.gameObject.layer = CardPlayer.UnityTrashWorkaround(PlayerHandLayer);
				AddCard(c);
				c.SetState(CardState.OnHand);
			}
		}

		public void AddCard(Card card)
		{
			if (CardsOnHand.Contains(card)) return;
			CardsOnHand.Add(card);
		}

		public void RemoveCard(Card card)
		{
			if (!CardsOnHand.Contains(card)) return;
			if (SelectedCard == card) SetSelectedCard(null);

			CardsOnHand.Remove(card);
		}

		[TargetRpc]
		public void TargetSetPosition(Vector3 HandAnchor)
		{
			Anchor = HandAnchor;
		}

		public bool IsCardFromHand(Card c)
		{
			return CardsOnHand.Contains(c);
		}

		private void Update()
		{
			if(!hasAuthority)return;
			for (int i = 0; i < CardsOnHand.Count; i++)
			{
				if (CardsOnHand[i] != SelectedCard)
				{
					float ii = i + 0.5f - CardsOnHand.Count / 2f;
					ii = Mathf.Abs(ii);
					float deltaY = OffsetFromAnchor + DeltaYPerCard * ii;
					float deltaYLocal = DeltaYLocalPerCard * ii;
					SetCardTransform(CardsOnHand[i].transform, i, Anchor, deltaY, deltaYLocal);
				}
			}
		}
	}
}
