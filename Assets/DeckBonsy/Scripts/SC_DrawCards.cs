using UnityEngine;
using System.Collections.Generic;

public class SC_DrawCards : MonoBehaviour
{
    public List<GameObject> deckCards = new List<GameObject>(); // Talia kart (widoczna w Inspectorze)
    public GameObject Hand; // Obszar reprezentuj¹cy rêkê (np. panel UI)

    private List<GameObject> handCards = new List<GameObject>(); // Lista kart na rêce
    private int maxCardsInHand = 3; // Maksymalna liczba kart na rêce

    public void OnClick()
    {
        if (handCards.Count >= maxCardsInHand) // Sprawdzenie, czy mo¿na dobraæ kartê
        {
            Debug.Log("Masz ju¿ maksymaln¹ liczbê kart na rêce!");
            return;
        }

        if (deckCards.Count == 0) // Jeœli talia jest pusta
        {
            Debug.Log("Brak kart w talii!");
            return;
        }

        // Losowanie karty z talii
        int randomIndex = Random.Range(0, deckCards.Count);
        GameObject randomCardPrefab = deckCards[randomIndex];

        // Tworzenie karty i dodanie jej na rêkê
        GameObject card = Instantiate(randomCardPrefab, new Vector2(0, 0), Quaternion.identity);
        card.transform.SetParent(Hand.transform, false);
        handCards.Add(card);

        // Usuniêcie karty z talii (opcjonalnie)
        deckCards.RemoveAt(randomIndex);

        Debug.Log("Dobra³eœ kartê: " + randomCardPrefab.name + ". Aktualna liczba kart na rêce: " + handCards.Count);
    }

    public void RemoveCard(GameObject card)
    {
        if (handCards.Contains(card))
        {
            handCards.Remove(card);
            Destroy(card);
            Debug.Log("Usuniêto kartê. Aktualna liczba kart: " + handCards.Count);
        }
    }
}
