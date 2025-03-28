
using UnityEngine;

public class Board : MonoBehaviour
{
    [Header("Main Variables")]
    static private int size = 3;
    [SerializeField] private bool playerBoard = true;
    [SerializeField] private RectTransform[] columns;
    [SerializeField] private GameObject columnPrefab;
    [SerializeField] private RectTransform[,] boardSpots;
    [SerializeField] private bool[,] occupiedBoardSpots;
    [SerializeField] private GameObject boardSpotPrefab;
    [SerializeField] private Card[,] placedCards;
    [SerializeField] private GameObject[,] placedCardsObjects;

    private int CountType(CardType type)
    {
        int count = 0;

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (placedCards[i, j].cardType == type)
                {
                    count++;
                }
            }
        }
        return count;
    }

    public void AddCardToColumn(Card addedCard, int columnIndex)
    {
        for (int i = 0; i < size; i++)
        {
            if (occupiedBoardSpots[columnIndex,i] == false)
            {
                occupiedBoardSpots[columnIndex,i] = true;
                placedCards[columnIndex, i] = addedCard;
                HandManager.handManager.GetCardObjectByIndex(i).transform.position = Vector3.zero;
                placedCardsObjects[columnIndex, i] = Instantiate(HandManager.handManager.GetCardObjectByIndex(i), 
                    boardSpots[columnIndex, i].transform.position, Quaternion.identity, boardSpots[columnIndex,i]);
                return;
            }
        }
        Debug.Log("Column full!");
    }

    private void Awake()
    {
        columns = new RectTransform[size];
        boardSpots = new RectTransform[size, size];
        occupiedBoardSpots = new bool[size, size];
        placedCards = new Card[size, size];
        placedCardsObjects = new GameObject[size, size];

        for (int i = 0; i < size; i++) 
        {
            GameObject newColumn = Instantiate(columnPrefab, Vector3.zero, Quaternion.identity, transform.GetChild(0));
            newColumn.GetComponent<ColumnSpot>().SetColumnIndex(i);
            columns[i] = newColumn.GetComponent<RectTransform>();
            for (int j = 0; j < size; j++) 
            {
                GameObject newBoardSpot = Instantiate(boardSpotPrefab, Vector3.zero, Quaternion.identity, transform.GetChild(1));
                boardSpots[i,j] = newBoardSpot.GetComponent<RectTransform>();
                occupiedBoardSpots[i, j] = false;
            }
        }
    }

    private void Start()
    {

    }


}
