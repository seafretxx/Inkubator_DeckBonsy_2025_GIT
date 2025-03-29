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
       
        (string, CardType, int)[] cardData = new (string, CardType, int)[]
        {
        ("Slave 1", CardType.Slave, 3),
        ("Emperor 1", CardType.Emperor, 6),
        ("Politician 1", CardType.Politician, 5),   // Senator Kaeso
        ("Politician 2", CardType.Politician, 4),   // Primus Octavian
        ("Politician 3", CardType.Politician, 4),   // Pontifex Maximus Aulus 
        ("Politician 3", CardType.Politician, 4),   // Praetor Magnus
        ("Soldier 1", CardType.Soldier, 1),         // Roman Solider
        ("Soldier 2", CardType.Soldier, 2),         // Roman Elite
        ("Citizen 1", CardType.Citizen, 1)          
        // + 2 klasy specialne zlodziej i szpieg
        };

       
        for (int i = 0; i < cardData.Length; i++)
        {
            var (cardName, cardType, points) = cardData[i];
            startingDeck[i] = new Card(i, cardName, 0, 0, cardType, points);
            Debug.Log("Card: " + cardName + " Points: " + points);
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
        if (HandManager.handManager.GetHandSize() >= HandManager.handManager.GetMaxHandSize())
        {
            // Dodać komunikat, że nie można dobrać więcej kart

            return;
        }
        if (cardsInDeck.Count > 0)
        {
            Card temp = cardsInDeck[cardsInDeck.Count - 1];
            HandManager.handManager.AddCardToHand(temp);
            cardsInDeck.Remove(temp);
        }
    }
}
