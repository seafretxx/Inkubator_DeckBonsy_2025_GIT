using System.Collections;
using UnityEngine;
using TMPro;
public class CardContainer : MonoBehaviour
{
    private Card cardInfo;
    public CardType cardType; 
    public int points;

    [SerializeField] private int handIndex;
    [SerializeField] private TMP_Text pointsText;
    [SerializeField] private TMP_Text nameText;
    public Card GetCardInfo()
    {
        return cardInfo;
    }

    public void SetCardInfo( Card _cardInfo)
    {
        cardInfo = _cardInfo;
        cardType = _cardInfo.cardType;
        points = _cardInfo.points;

        if (pointsText != null)
        {
            pointsText.text = points.ToString();
        }
        else
        {
            Debug.LogWarning("pointsText is not assigned!");
        }
        if (nameText != null)
        { 
            nameText.text = _cardInfo.cardName;
        }
        else
        {
            Debug.LogWarning("nameText is not assigned!");
        }
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

    private void Awake()
    {
        if (pointsText == null)
            pointsText = GetComponentInChildren<TMP_Text>();
        if (nameText == null)
        {
            TMP_Text[] allTexts = GetComponentsInChildren<TMP_Text>();
            foreach (var text in allTexts)
            {
                if (text.gameObject.name.Contains("NameTekst")) 
                    nameText = text;
            }
        }
    }
}
