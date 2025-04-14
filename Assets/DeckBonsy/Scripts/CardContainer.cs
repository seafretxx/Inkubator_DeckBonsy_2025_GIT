using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class CardContainer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
    void OnMouseOver()
    {
        if (cardInfo != null && isPlayerCard) // tylko gracz widzi opisy
            HandManager.handManager.ShowCardDescription(cardInfo.cardDescription);
    }

    void OnMouseExit()
    {
        if (isPlayerCard)
            HandManager.handManager.HideCardDescription();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
<<<<<<< HEAD
        //Debug.Log("ONMOUSEOVER!" + cardInfo + isPlayerCard);
=======
>>>>>>> parent of 15abde0 (1.1)
        if (cardInfo != null && isPlayerCard)
            HandManager.handManager.ShowCardDescription(cardInfo.cardDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isPlayerCard)
            HandManager.handManager.HideCardDescription();
    }
}
