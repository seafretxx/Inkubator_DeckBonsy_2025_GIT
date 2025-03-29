using System.Collections;
using UnityEngine;

public class CardContainer : MonoBehaviour
{
    private Card cardInfo;
    public CardType cardType; 
    public int points;

    [SerializeField] private int handIndex;

    private void Awake()
    {

    }
    public Card GetCardInfo()
    {
        return cardInfo;
    }
    public void SetHandIndex(int _handIndex)
    {
        handIndex = _handIndex;
    }

    public void WhenClicked()
    {
        Debug.Log("YAY!");
        GameManager.gameManager.SetChosenCardIndex(handIndex);
    }
}
