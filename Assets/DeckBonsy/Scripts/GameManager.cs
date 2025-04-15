using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager { get; private set; }

    [Header("Main Variables")]
    [SerializeField] private bool isPlayerTurn;
    [SerializeField] private bool gameReady;
    [SerializeField] public int selectedCardIndex;

    [Header("[TEMP] Input System")]
    [SerializeField] private bool chosenCard;
    [SerializeField] private int chosenCardIndex;
    [SerializeField] private bool chosenColumn;
    [SerializeField] private int chosenColumnIndex;

    [Header("Board References")]
    [SerializeField] private Board playerBoard;
    [SerializeField] private Board enemyBoard;
    [SerializeField] private TextMeshProUGUI playerScoreCounter;
    [SerializeField] private TextMeshProUGUI enemyScoreCounter;
   

    private void Awake()
    {
        /// Singleton mechanism
        {
            if (gameManager != null && gameManager != this)
            {
                Destroy(this);
            }
            else
            {
                gameManager = this;
            }
        }
    }

    private void Start()
    {
        chosenCard = false;
        chosenColumn = false;
        isPlayerTurn = true;
        gameReady = true;
        UpdateScore();
    }

    private void Update()
    {
        if (chosenCard && chosenColumn) // PLAYING A CARD
        {
            if (isPlayerTurn)
            {
                if (playerBoard.CheckForEmptyInColumn(chosenColumnIndex))
                {
                    //Debug.Log(chosenCardIndex + " " + chosenColumnIndex);
                    chosenCard = chosenColumn = false;
                    playerBoard.AddCardToColumn(HandManager.handManager.GetCardObjectByIndex(chosenCardIndex), chosenColumnIndex);
                    HandManager.handManager.RemoveCardFromHand(chosenCardIndex);
                    //playerBoard.ListBoard();
                    //HandManager.handManager.ListHand();
                    UpdateScore();
                    EndTurn();
                }
                else
                {
                    chosenCard = chosenColumn = false;
                    Debug.Log("Column full! Pick again.");
                }
            }
            else
            {
                if (enemyBoard.CheckForEmptyInColumn(chosenColumnIndex))
                {
                    //Debug.Log(chosenCardIndex + " " + chosenColumnIndex);
                    chosenCard = chosenColumn = false;
                    enemyBoard.AddCardToColumn(HandManager.handManager.GetCardObjectByIndex(chosenCardIndex), chosenColumnIndex);
                    HandManager.handManager.RemoveCardFromHand(chosenCardIndex);
                    //enemyBoard.ListBoard();
                    //.handManager.ListHand();
                    UpdateScore();
                    EndTurn();
                }
                else
                {
                    chosenCard = chosenColumn = false;
                    Debug.Log("Column full! Pick again.");
                }
            }
        }
    }

    public void RemoveCardsWithEqualPoints(int columnIndex, int cardPoints)
    {
        if (isPlayerTurn)
        {
            enemyBoard.RemoveCardsFromColumn(columnIndex, cardPoints);
        }
        else
        {
            playerBoard.RemoveCardsFromColumn(columnIndex, cardPoints);
        }
    }

    public void EndTurn()
    {
        if (isPlayerTurn)
        {
            Debug.Log("Player 1's turn has ended.");
            isPlayerTurn = false;
            Debug.Log("Now it's Player 2's turn.");
        }
        else
        {
            Debug.Log("Player 2's turn has ended.");
            isPlayerTurn = true;
            Debug.Log("Now it's Player 1's turn.");
        }
    }

    public bool GetPlayerTurn()
    {
        return isPlayerTurn;
    }

    public void SetChosenCardIndex(int _chosenCardIndex, bool _isPlayerCard)
    {
        if (isPlayerTurn == _isPlayerCard)
        {
            chosenCard = true;
            chosenCardIndex = _chosenCardIndex;
        }
        else
        {
            Debug.Log("Not your turn!");
        }
    }

    public void SetChosenColumnIndex(int _chosenColumnIndex, bool _isPlayerBoard)
    {
        if (isPlayerTurn == _isPlayerBoard)
        {
            chosenColumn = true;
            chosenColumnIndex = _chosenColumnIndex;
        }
        else
        {
            Debug.Log("Not your turn!");
        }
    }

    public void UpdateScore()
    {
        playerScoreCounter.text = ("Your score:\n" + playerBoard.CountScore());
        enemyScoreCounter.text = ("Enemy score:\n" + enemyBoard.CountScore());
    }
    
}