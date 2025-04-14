using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System;
using TMPro;

public class HandManager : MonoBehaviour
{
    public static HandManager handManager { get; private set; }

    [Header("Main Variables")]

    [Header("References")]
    [SerializeField] Hand playerHand;
    [SerializeField] Hand enemyHand;
    [SerializeField] private GameObject descriptionBox;
    [SerializeField] private TextMeshProUGUI descriptionText;

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
    public void ShowCardDescription(string desc)
    {
        descriptionText.text = desc;
        descriptionBox.SetActive(true);
    }

    public void HideCardDescription()
    {
        descriptionBox.SetActive(false);
    }

    public int GetMaxHandSize()
    {
        if (GameManager.gameManager.GetPlayerTurn())
        {
            return playerHand.GetMaxHandSize();
        }
        else
        {
            return enemyHand.GetMaxHandSize();
        }
    }

    public int GetHandSize()
    {
        if (GameManager.gameManager.GetPlayerTurn())
        {
            return playerHand.GetHandSize();
        }
        else
        {
            return enemyHand.GetHandSize();
        }
    }

    public Card GetCardByIndex(int index)
    {
        if (GameManager.gameManager.GetPlayerTurn())
        {
            return playerHand.GetCardByIndex(index);
        }
        else
        {
            return enemyHand.GetCardByIndex(index);
        }
    }

    public GameObject GetCardObjectByIndex(int index)
    {
        if (GameManager.gameManager.GetPlayerTurn())
        {
            return playerHand.GetCardObjectByIndex(index);
        }
        else
        {
            return enemyHand.GetCardObjectByIndex(index);
        }
    }

    public void ListHand()
    {
        if (GameManager.gameManager.GetPlayerTurn())
        {
            playerHand.ListHand();
        }
        else
        {
            enemyHand.ListHand();
        }
    }

    public void AddCardToHand(Card addedCard)
    {
        if (GameManager.gameManager.GetPlayerTurn())
        {
            playerHand.AddCardToHand(addedCard);
        }
        else
        {
            enemyHand.AddCardToHand(addedCard);
        }
    }

    public void RemoveCardFromHand(int index)
    {
        if (GameManager.gameManager.GetPlayerTurn())
        {
            playerHand.RemoveCardFromHand(index);
        }
        else
        {
            enemyHand.RemoveCardFromHand(index);
        }
    }
}