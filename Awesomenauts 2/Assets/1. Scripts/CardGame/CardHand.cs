using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

public class CardHand : IHand
{
	private float RotationOffset;
	public int MaxCardCount => 5;
	public float MaxCardRotation => 45;
	public float OffsetFromAnchor => 5;
	public float Drag => 0.1f;
	public Transform Anchor;

	private LayerMask HandLayer;
	private IPlayer Player;
	private List<ICard> Cards = new List<ICard>();
	private ICard SelectedCard;

	public CardHand(GameSettingsObject settings, float rotationOffset, IPlayer player, IDeck deck, LayerMask handLayer)
	{
		HandLayer = handLayer;
		
		RotationOffset = rotationOffset;
		Initialize(settings, player, deck);
	}

	public void SetAnchor(Transform anchor)
	{
		Anchor = anchor;
	}

	public void Initialize(GameSettingsObject settings, IPlayer player, IDeck deck)
	{
		Player = player;
	}


	public void AddCard(ICard card)
	{
		if (!Cards.Contains(card))
		{
			card.CardTransform.gameObject.layer = CardGameBoard.UnityTrashWorkaround(HandLayer);
			Cards.Add(card);
		}
	}
	

	public void RemoveCard(ICard card)
	{
		if (Cards.Contains(card))
		{
			Cards.Remove(card);
		}
	}

	public void SetSelectedCard(ICard card)
	{
		SelectedCard = card;
	}

	public bool CanAddCard()
	{
		return Cards.Count < MaxCardCount;
	}

	public int GetCardCount()
	{
		return Cards.Count;
	}

	private void SetCardTransform(float rotationOffset, Transform cardTransform, int i, Vector3 anchor, Transform cameraTransform, float offsetFromAnchor)
	{
		Vector3 centerPosition = anchor + Vector3.left * offsetFromAnchor;
		float rotation = (i + 0.5f) / Cards.Count;
		rotation -= 0.5f;
		rotation *= 2;
		rotation *= MaxCardRotation;
		rotation += rotationOffset;
		cardTransform.rotation = Quaternion.identity;


		Vector3 oldCardPos = cardTransform.position;


		cardTransform.position = centerPosition;
		//cardTransform.up = cameraTransform.position - cardTransform.position;
		cardTransform.RotateAround(anchor, Vector3.down, rotation);
		Vector3 newCardPos = cardTransform.position;
		cardTransform.position = Vector3.Lerp(oldCardPos, newCardPos, Drag);


	}

	public bool IsCardFromHand(ICard card)
	{
		return Cards.Count(x => x.CardID == card.CardID) != 0;
	}

	public void UpdateCardPositions()
	{
		for (int i = 0; i < Cards.Count; i++)
		{
			if (Cards[i] != SelectedCard)
			{
				SetCardTransform(RotationOffset, Cards[i].CardTransform, i, Anchor.position, Player.CameraTransform, OffsetFromAnchor);
			}
		}
	}
}
