using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public static DeckManager deckManager { get; private set; }

    [Header("Main Variables")]
    private const int startingDeckSize = 19;
    [SerializeField] private Card[] startingDeck;
    [SerializeField] private List<Card> cardsInDeck;
    [SerializeField] private Sprite[] cardSprite;

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
        ListDeck();
    }

    public void MockDeck()
    {
        // id, name, description, effectId, points, cardType, sprite
        (int, string, string, int, int, CardType, Sprite)[] cardValues = new (int, string, string, int, int, CardType, Sprite)[]
        {
        (0, "Slave","Bazowa jednostka", 0, 1, CardType.Slave,cardSprite[0]),
        (1, "Slave","Bazowa jednostka", 0, 1, CardType.Slave,cardSprite[1]),
        (2, "Slave","Bazowa jednostka", 0, 1, CardType.Slave,cardSprite[2]),
        (3, "Emperor","Nie może zostać ukradziony, jednak w dalszym ciągu może być zbity", 1, 6, CardType.Emperor,cardSprite[3]),
        (4, "Kaeso","-1 punkt do karty od każdej karty Politycznej na planszy, +1 do każdej karty Slave na planszy", 2, 5, CardType.Politician,cardSprite[4]),
        (5, "Octavian Helion","Dodaje kartę Citizena na rękę. Boostują go karty Citizenów w tej samej kolumnie", 3, 4, CardType.Politician,cardSprite[5]),
        (6, "Domina Livia Versus","Boostują ją soldierzy w tej samej kolumnie ", 6, 3, CardType.Politician,cardSprite[6]),
        (7, "Pontifex Maximus Aulus","Daje immunity wybranej karcie w rzędzie (domyślnie karta poniżej, jednak jeśli karta Najwyższego Kapłana jest kartą na spodzie rzędu, wtedy immunity przechodzi na kartę powyżej", 4, 4, CardType.Politician,cardSprite[7]),
        (8, "Magnus","Wybiera kartę przeciwnika z planszy z zakresu punktacji i cofa ją na jego rękę, lub do talii, gdy nie ma miejsca ", 5, 4, CardType.Politician,cardSprite[8]),
        (9, "Soldier","Bazowa jednostka", 0, 1, CardType.Soldier,cardSprite[9]),
        (10, "Soldier","Bazowa jednostka", 0, 1, CardType.Soldier,cardSprite[10]),
        (11, "Soldier","Bazowa jednostka", 0, 1, CardType.Soldier,cardSprite[11]),
        (12, "Elite Soldier","Bazowa jednostka, ulepszony Soldier", 0, 2, CardType.Soldier,cardSprite[12]),
        (13, "Elite Soldier","Bazowa jednostka, ulepszony Soldier", 0, 2, CardType.Soldier,cardSprite[13]),
        (14, "Elite Soldier","Bazowa jednostka, ulepszony Soldier", 0, 2, CardType.Soldier,cardSprite[14]),
        (15, "Citizen","Bazowa jednostka", 0, 1, CardType.Citizen,cardSprite[15]),
        (16, "Citizen","Bazowa jednostka", 0, 1, CardType.Citizen,cardSprite[16]),
        (17, "Citizen","Bazowa jednostka", 0, 1, CardType.Citizen,cardSprite[17]),
        (18, "Infiltrator","Gracz może podejrzeć pierwszą kartę z wierzchu stosu kart przeciwnika",7, 2,CardType.Citizen,cardSprite[18]),
        (19, "Złodziej", " Gracz zamienia dowolną kartę przeciwnika ze swoją kartą Złodzieja i bierze ją na rękę",8,0,CardType.Citizen,cardSprite[19])
        
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
