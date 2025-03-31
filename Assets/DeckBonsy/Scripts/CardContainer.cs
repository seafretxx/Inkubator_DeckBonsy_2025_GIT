using System.Collections;
using UnityEngine;
using TMPro;

public class CardContainer : MonoBehaviour
{
    private Card cardInfo;
    [SerializeField] private bool isPlayerCard;
    [SerializeField] private int handIndex;
    [SerializeField] private TextMeshProUGUI handPower;
    [SerializeField] private TextMeshProUGUI handName;

    public Card GetCardInfo()
    {
        return cardInfo;
    }

    public void SetCardInfo(Card _cardInfo)
    {
        cardInfo = _cardInfo;
    }

    public void SetIsPlayerCard(bool _isPlayerCard)
    {
        isPlayerCard = _isPlayerCard;
    }

    public void SetHandIndex(int _handIndex)
    {
        handIndex = _handIndex;
    }

    public void WhenClicked()
    {
        GameManager.gameManager.SetChosenCardIndex(handIndex, isPlayerCard);
    }

    public void UpdateCard()
    {
        handPower.text = "" + cardInfo.points;
        handName.text = "" + cardInfo.cardName;
    }
}
