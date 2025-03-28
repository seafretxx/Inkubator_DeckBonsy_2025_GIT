using System.Collections;
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
        //ListDeck();
    }

    public void MockDeck()
    {
        for(int i =0;i<startingDeckSize;i++)
        {
            Card createdCard = new Card();
            createdCard.SetValues(0, "mock"+i, 0, 0, 0);
            startingDeck[i] = createdCard;
        }
    }

    public void ResetDeck()
    {
        foreach(Card card in startingDeck)
        {
            cardsInDeck.Add(card);
        }
    }

    public void ListDeck()
    {
        foreach(Card card in cardsInDeck)
        {
            Debug.Log(card.cardName);
        }
    }

    public void DrawCard()
    {
        if(HandManager.handManager.GetHandSize() >= HandManager.handManager.GetMaxHandSize())
        {
            // Dodać komunikat, że nie można dobrać więcej kart

            return;
        }
        Card temp = cardsInDeck[cardsInDeck.Count-1];
        cardsInDeck.Remove(temp);
        HandManager.handManager.AddCardToHand(temp);
    }
}
