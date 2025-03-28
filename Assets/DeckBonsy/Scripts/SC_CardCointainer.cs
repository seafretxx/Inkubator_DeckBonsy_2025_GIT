using System;
using System.Collections.Generic;
using System.Linq;
using config;
using DefaultNamespace;
using events;
using UnityEngine;
using UnityEngine.UI;

public class CardContainer : MonoBehaviour
{

   

    [SerializeField]
    private SC_DrawCards drawCards;
    


    [Header("Constraints")]
    [SerializeField]
    private bool forceFitContainer;

    [SerializeField]
    private bool preventCardInteraction;

    [Header("Alignment")]
    [SerializeField]
    private CardAlignment alignment = CardAlignment.Center;

    [SerializeField]
    private bool allowCardRepositioning = true;

    [Header("Rotation")]
    [SerializeField]
    [Range(-90f, 90f)]
    private float maxCardRotation;

    [SerializeField]
    private float maxHeightDisplacement;

    [SerializeField]
    private ZoomConfig zoomConfig;

    [SerializeField]
    private AnimationSpeedConfig animationSpeedConfig;

    [SerializeField]
    private CardPlayConfig cardPlayConfig;

    [Header("Events")]
    [SerializeField]
    private EventsConfig eventsConfig;

    private List<CardWrapper> cards = new();

    private RectTransform rectTransform;
    private CardWrapper currentDraggedCard;


    public void Awake()
    {
        drawCards = GameObject.Find("Button").GetComponent<SC_DrawCards>();
        if (drawCards == null);
    }




    private SC_PlayerAreaSetup playerAreaManager;

    private void Start()
    {
        playerAreaManager = FindObjectOfType<SC_PlayerAreaSetup>();
        if (playerAreaManager == null)
        {
            Debug.LogError("Nie znaleziono SC_PlayerAreaSetup w scenie!");
        }
    }
private void InitCards()
    {
        SetUpCards();
        SetCardsAnchor();
    }

    private void SetCardsRotation()
    {
        for (var i = 0; i < cards.Count; i++)
        {
            cards[i].targetRotation = GetCardRotation(i);
            cards[i].targetVerticalDisplacement = GetCardVerticalDisplacement(i);
        }
    }

    private float GetCardVerticalDisplacement(int index)
    {
        if (cards.Count < 3) return 0;
        // Associate a vertical displacement based on the index in the cards list
        // so that the center card is at max displacement while the edges are at 0 displacement
        return maxHeightDisplacement *
               (1 - Mathf.Pow(index - (cards.Count - 1) / 2f, 2) / Mathf.Pow((cards.Count - 1) / 2f, 2));
    }

    private float GetCardRotation(int index)
    {
        if (cards.Count < 3) return 0;
        // Associate a rotation based on the index in the cards list
        // so that the first and last cards are at max rotation, mirrored around the center
        return -maxCardRotation * (index - (cards.Count - 1) / 2f) / ((cards.Count - 1) / 2f);
    }

    void Update()
    {
        UpdateCards();
    }

    void SetUpCards()
    {
        cards.Clear();
        foreach (Transform card in transform)
        {
            var wrapper = card.GetComponent<CardWrapper>();
            if (wrapper == null)
            {
                wrapper = card.gameObject.AddComponent<CardWrapper>();
            }

            cards.Add(wrapper);

            AddOtherComponentsIfNeeded(wrapper);

            // Pass child card any extra config it should be aware of
            wrapper.zoomConfig = zoomConfig;
            wrapper.animationSpeedConfig = animationSpeedConfig;
            wrapper.eventsConfig = eventsConfig;
            wrapper.preventCardInteraction = preventCardInteraction;
            wrapper.container = this;
        }
    }

    private void AddOtherComponentsIfNeeded(CardWrapper wrapper)
    {
        var canvas = wrapper.GetComponent<Canvas>();
        if (canvas == null)
        {
            canvas = wrapper.gameObject.AddComponent<Canvas>();
        }

        canvas.overrideSorting = true;

        if (wrapper.GetComponent<GraphicRaycaster>() == null)
        {
            wrapper.gameObject.AddComponent<GraphicRaycaster>();
        }
    }

    private void UpdateCards()
    {
        if (transform.childCount != cards.Count)
        {
            InitCards();
        }

        if (cards.Count == 0)
        {
            return;
        }

        SetCardsPosition();
        SetCardsRotation();
        SetCardsUILayers();
        UpdateCardOrder();
    }

    private void SetCardsUILayers()
    {
        for (var i = 0; i < cards.Count; i++)
        {
            cards[i].uiLayer = zoomConfig.defaultSortOrder + i;
        }
    }

    private void UpdateCardOrder()
    {
        if (!allowCardRepositioning || currentDraggedCard == null) return;

        // U¿yj "Try-Catch" aby uchwyciæ potencjalne b³êdy zwi¹zane z nieistniej¹cymi indeksami
        try
        {
            // Get the index of the dragged card depending on its position
            var newCardIdx = cards.Count(card => currentDraggedCard.transform.position.x > card.transform.position.x);
            var originalCardIdx = cards.IndexOf(currentDraggedCard);

            // SprawdŸ, czy nie próbujemy usun¹æ karty, która ju¿ nie istnieje
            if (originalCardIdx < 0 || originalCardIdx >= cards.Count)
            {
                Debug.LogWarning("Nie uda³o siê znaleŸæ karty o tym indeksie: " + originalCardIdx);
                return;  // Wychodzimy, jeœli indeks jest poza zakresem
            }

            // Jeœli karta siê przemieœci³a, zaktualizuj jej pozycjê w liœcie
            if (newCardIdx != originalCardIdx)
            {
                cards.RemoveAt(originalCardIdx);
                if (newCardIdx > originalCardIdx && newCardIdx < cards.Count - 1)
                {
                    newCardIdx--;
                }

                cards.Insert(newCardIdx, currentDraggedCard);
            }

            // Also reorder in the hierarchy
            currentDraggedCard.transform.SetSiblingIndex(newCardIdx);
        }
        catch (ArgumentOutOfRangeException e)
        {
            Debug.LogError("Wyst¹pi³ b³¹d przy aktualizacji kolejnoœci kart: " + e.Message);
        }
    }


    private void SetCardsPosition()
    {
        // Compute the total width of all the cards in global space
        var cardsTotalWidth = cards.Sum(card => card.width * card.transform.lossyScale.x);
        // Compute the width of the container in global space
        var containerWidth = rectTransform.rect.width * transform.lossyScale.x;
        if (forceFitContainer && cardsTotalWidth > containerWidth)
        {
            DistributeChildrenToFitContainer(cardsTotalWidth);
        }
        else
        {
            DistributeChildrenWithoutOverlap(cardsTotalWidth);
        }
    }

    private void DistributeChildrenToFitContainer(float childrenTotalWidth)
    {
        // Get the width of the container
        var width = rectTransform.rect.width * transform.lossyScale.x;
        // Get the distance between each child
        var distanceBetweenChildren = (width - childrenTotalWidth) / (cards.Count - 1);
        // Set all children's positions to be evenly spaced out
        var currentX = transform.position.x - width / 2;
        foreach (CardWrapper child in cards)
        {
            var adjustedChildWidth = child.width * child.transform.lossyScale.x;
            child.targetPosition = new Vector2(currentX + adjustedChildWidth / 2, transform.position.y);
            currentX += adjustedChildWidth + distanceBetweenChildren;
        }
    }

    private void DistributeChildrenWithoutOverlap(float childrenTotalWidth)
    {
        var currentPosition = GetAnchorPositionByAlignment(childrenTotalWidth);
        foreach (CardWrapper child in cards)
        {
            var adjustedChildWidth = child.width * child.transform.lossyScale.x;
            child.targetPosition = new Vector2(currentPosition + adjustedChildWidth / 2, transform.position.y);
            currentPosition += adjustedChildWidth;
        }
    }

    private float GetAnchorPositionByAlignment(float childrenWidth)
    {
        var containerWidthInGlobalSpace = rectTransform.rect.width * transform.lossyScale.x;
        switch (alignment)
        {
            case CardAlignment.Left:
                return transform.position.x - containerWidthInGlobalSpace / 2;
            case CardAlignment.Center:
                return transform.position.x - childrenWidth / 2;
            case CardAlignment.Right:
                return transform.position.x + containerWidthInGlobalSpace / 2 - childrenWidth;
            default:
                return 0;
        }
    }

    private void SetCardsAnchor()
    {
        foreach (CardWrapper child in cards)
        {
            child.SetAnchor(new Vector2(0, 0.5f), new Vector2(0, 0.5f));
        }
    }

    public void OnCardDragStart(CardWrapper card)
    {
        currentDraggedCard = card;
    }


    public void OnCardDragEnd()
    {
        if (currentDraggedCard == null) return;

        
        if (IsCursorInPlayArea())
        {

            RectTransform playArea = cardPlayConfig.playArea;
            int columnIndex = 0;
            SC_PlayerAreaSetup playerAreaManager = FindObjectOfType<SC_PlayerAreaSetup>();


            playerAreaManager.PlayCardToColumn(currentDraggedCard.gameObject, columnIndex);



            RectTransform cardRect = currentDraggedCard.GetComponent<RectTransform>();
            cardRect.localPosition = Vector3.zero;   
            cardRect.localRotation = Quaternion.identity; 
            cardRect.localScale = Vector3.one;  

            
            Debug.Log("Karta przeniesiona do: " + playArea.name);
            Debug.Log("Pozycja karty: " + cardRect.localPosition);

            
            Canvas cardCanvas = currentDraggedCard.GetComponent<Canvas>();
            if (cardCanvas != null)
            {
                cardCanvas.enabled = true; 
            }

            

            SC_DrawCards drawCardsScript = FindObjectOfType<SC_DrawCards>();
            if (drawCardsScript != null)
            {
                
                drawCardsScript.RemoveCard(currentDraggedCard.gameObject);
            }

            //cards.Remove(currentDraggedCard);


            Destroy(currentDraggedCard);

           
            eventsConfig?.OnCardPlayed?.Invoke(new CardPlayed(currentDraggedCard));

            
            Debug.Log("Zagrano kartê: " + currentDraggedCard.name);
            Debug.Log("PlayerArea: " + cardPlayConfig.playArea.name);

            drawCardsScript.UpdateHandCardCount();
        }

        
        currentDraggedCard = null;
    }

    public void DestroyCard(CardWrapper card)
    {
        drawCards.RemoveCard(card.gameObject);
        cards.Remove(card);
        eventsConfig.OnCardDestroy?.Invoke(new CardDestroy(card));
        Destroy(card.gameObject);
    }

    private bool IsCursorInPlayArea()
    {
        if (cardPlayConfig.playArea == null) return false;

        var cursorPosition = Input.mousePosition;
        var playArea = cardPlayConfig.playArea;
        var playAreaCorners = new Vector3[4];
        playArea.GetWorldCorners(playAreaCorners);
        return cursorPosition.x > playAreaCorners[0].x &&
               cursorPosition.x < playAreaCorners[2].x &&
               cursorPosition.y > playAreaCorners[0].y &&
               cursorPosition.y < playAreaCorners[2].y;

    }
}