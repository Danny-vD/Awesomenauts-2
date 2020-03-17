using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDeck : IDeck
{
	private Queue<ICard> Cards;
	private ICard FatigueCard;

	public CardDeck(GameSettingsObject settings, ICard[] cards)
	{
		FatigueCard = settings.FatigueCard;
		//Copying the Deck to make sure that the cards parameter stays the same when shuffling.
		ICard[] tempCards = new ICard[cards.Length];
		cards.CopyTo(tempCards, 0);
		FisherYates(tempCards);

		Cards = new Queue<ICard>(tempCards);
	}

	public int GetCardCount()
	{
		return Cards.Count;
	}

	public ICard DrawCard()
	{
		if (Cards.Count == 0) return FatigueCard;
		return Cards.Dequeue();
	}


	private static System.Random rnd = new System.Random();
	public static void FisherYates<T>(T[] array)
	{
		int j;
		for (int i = array.Length - 1; i > 0; i--)
		{
			j = rnd.Next(0, i + 1);
			Swap(array, i, j);
		}
	}


	public static void Swap<T>(T[] array, int indexA, int indexB)
	{
		T tmp = array[indexA];
		array[indexA] = array[indexB];
		array[indexB] = tmp;
	}
}
