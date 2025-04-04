﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public static DeckManager deckManager { get; private set; }

    [Header("Main Variables")]
    private const int startingDeckSize = 10;
    [SerializeField] private Card[] startingDeck;
    [SerializeField] private List<Card> cardsInDeck;

    private void Awake()
    {
        /// Singleton mechanism
        {
            if (deckManager != null && deckManager != this)
            {
                Destroy(this);
            }
            else
            {
                deckManager = this;
            }
        }

        startingDeck = new Card[startingDeckSize];
        cardsInDeck = new List<Card>();
    }

    private void Start()
    {
        MockDeck();
        ResetDeck();
        ShuffleDeck();
        //ListDeck();
    }

    public void MockDeck()
    {
        // id, name, effectId, points, cardType
        (int, string, int, int, CardType)[] cardValues = new (int, string, int, int, CardType)[]
        {
        (0, "Slave", 0, 1, CardType.Slave),
        (1, "Emperor", 0, 6, CardType.Emperor),
        (2, "Kaeso", 0, 5, CardType.Politician),
        (3, "Octavian", 0, 4, CardType.Politician),
        (4, "Maximus Aulus", 0, 4, CardType.Politician),
        (5, "Magnus", 0, 4, CardType.Politician),
        (6, "Soldier", 0, 1, CardType.Soldier),
        (7, "Elite Soldier", 0, 2, CardType.Soldier),
        (8, "Citizen", 0, 1, CardType.Citizen),
        (9, "Boru", 0, 36, CardType.Citizen)
        // + 2 klasy specialne zlodziej i szpieg
        };

        for (int i = 0; i < startingDeckSize; i++)
        {
            Card createdCard = new Card();
            createdCard.SetValues(cardValues[i]);
            startingDeck[i] = createdCard;
        }
    }

    public void ResetDeck()
    {
        foreach (Card card in startingDeck)
        {
            cardsInDeck.Add(card);
        }
    }
    private void ShuffleDeck()
    {
        for (int i = 0; i < cardsInDeck.Count; i++)
        {
            Card temp = cardsInDeck[i];
            int randomIndex = Random.Range(i, cardsInDeck.Count);
            cardsInDeck[i] = cardsInDeck[randomIndex];
            cardsInDeck[randomIndex] = temp;
        }
    }
    public void AddCardToDeck(Card addedCard)
    {
        cardsInDeck.Add(addedCard);
    }

    public void ListDeck()
    {
        foreach (Card card in cardsInDeck)
        {
            Debug.Log(card.cardName);
        }
    }

    public void DrawCard()
    {
        if (HandManager.handManager.GetHandSize() >= HandManager.handManager.GetMaxHandSize())
        {
            // Dodać komunikat, że nie można dobrać więcej kart
            Debug.Log("Max hand size reached!");
            return;
        }
        if (cardsInDeck.Count > 0)
        {
            Card temp = cardsInDeck[cardsInDeck.Count - 1];
            HandManager.handManager.AddCardToHand(temp);
            cardsInDeck.Remove(temp);
            GameManager.gameManager.EndTurn();
        }
    }
}
