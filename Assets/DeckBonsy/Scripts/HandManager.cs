using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

public class HandManager : MonoBehaviour
{
    public static HandManager handManager { get; private set; }

    [Header("Main Variables")]
    [SerializeField]
    public const int maxHandSize = 3;
    private int currentHandSize = 0;
    [SerializeField] private RectTransform[] positions;
    [SerializeField] private bool[] positionsOccupied;
    [SerializeField] private GameObject[] cardObjects;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Card[] hand = new Card[maxHandSize];

    public int GetMaxHandSize()
    {
        return maxHandSize;
    }
    public int GetHandSize()
    {
        return currentHandSize;
    }
      
    public Card GetCardByIndex(int index)
    {
        return hand[index];
    }

    public GameObject GetCardObjectByIndex(int index)
    {
        return cardObjects[index];
    }

    public void AddCardToHand(Card addedCard)
    {
        for (int i = 0; i < maxHandSize; i++)
        {
            if (positionsOccupied[i] == false)
            {
                positionsOccupied[i] = true;
                cardObjects[i] = Instantiate(cardPrefab, positions[i].position, Quaternion.identity);
                cardObjects[i].GetComponent<Transform>().SetParent(positions[i].GetComponent<Transform>());
                cardObjects[i].GetComponent<CardContainer>().SetHandIndex(i);
                currentHandSize++;

                hand[i] = addedCard;

                return;
            }
        }
    }


    public void RemoveCardFromHand(int index)
    {
        Destroy(cardObjects[index]);
        positionsOccupied[index] = false;
        hand[index] = null;
        currentHandSize--;
    }
    public void ListHand()
    {
        string s = "";
        for (int i = 0; i < maxHandSize; i++)
        {
            s += positionsOccupied[i];
        }
        Debug.Log(s);
    }
    private void Awake()
    {
        /// Singleton mechanism
        {
            if (handManager != null && handManager != this)
            {
                Destroy(this);
            }
            else
            {
                handManager = this;
            }
        }
    }

    private void Start()
    {
        currentHandSize = 0;
        cardObjects = new GameObject[maxHandSize];
        positionsOccupied = new bool[maxHandSize];
        for (int i = 0; i < maxHandSize; i++) 
        {
            positionsOccupied[i] = false;
        }
    }

}
