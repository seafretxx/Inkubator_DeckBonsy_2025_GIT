using UnityEngine;
using System.Collections.Generic;

public class SC_DrawCards : MonoBehaviour
{
    public GameObject[] deckCards; // Talia kart (ró¿ne prefabry)
    public GameObject Hand;

    private List<GameObject> handCards = new List<GameObject>();
    private int maxCardsInHand = 3;

    public void OnClick()
    {
        if (handCards.Count >= maxCardsInHand)
        {
            Debug.Log("Masz ju¿ maksymaln¹ liczbê kart na rêce!");
            return;
        }

        if (deckCards.Length == 0)
        {
            Debug.Log("Talia jest pusta!");
            return;
        }

        int randomIndex = Random.Range(0, deckCards.Length);
        GameObject card = Instantiate(deckCards[randomIndex], Hand.transform, false);
        handCards.Add(card);

        Debug.Log("Dobra³eœ kartê. Aktualna liczba kart: " + handCards.Count);
    }

    public void RemoveCard(GameObject card)
    {
        if (handCards.Contains(card))
        {
            handCards.Remove(card);
            Debug.Log("Usuniêto kartê. Aktualna liczba kart: " + handCards.Count);
        }
    }
}
