using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHand
{
	int MaxCardCount { get; }
	float MaxCardRotation { get; }
	float OffsetFromAnchor { get; }
	float Drag { get; }
	void Initialize(GameSettingsObject settings, IPlayer player, IDeck deck);
	void AddCard(ICard card);
	void SetAnchor(Transform anchor);
	void UpdateCardPositions();
	void RemoveCard(ICard card);
	void SetSelectedCard(ICard card);
	void SetLayer(LayerMask layer);
	bool CanAddCard();
	bool IsCardFromHand(ICard card);
	int GetCardCount();
}
