using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager { get; private set; }

    [Header("Main Variables")]
    //[SerializeField] private bool isPlayerTurn
    [SerializeField] private bool gameReady;
    [SerializeField] public int selectedCardIndex;

    [Header("[TEMP] Input System")]
    [SerializeField] private bool chosenCard;
    [SerializeField] private int chosenCardIndex;
    [SerializeField] private bool chosenColumn;
    [SerializeField] private int chosenColumnIndex;

    [Header("Board References")]
    [SerializeField] private Board player1Board;
    [SerializeField] private Board player2Board;

    [Header("UI")]
    //[SerializeField] private TextMeshProUGUI playerScore;
    [SerializeField] private TextMeshProUGUI player1ScoreText;
    [SerializeField] private TextMeshProUGUI player2ScoreText;

    private enum TurnState
    {
        Player1,
        Player2
    }
    private TurnState currentTurn;
    private void UpdateTurn()
    {
        // Zmiana tury
        if (currentTurn == TurnState.Player1)
        {
            currentTurn = TurnState.Player2;
        }
        else
        {
            currentTurn = TurnState.Player1;
        }

        Debug.Log("Current Turn: " + currentTurn.ToString());
    }
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
        gameReady = true;
        chosenCard = false;
        currentTurn = TurnState.Player1;
    }

    private void Update()
    {
        if (currentTurn == TurnState.Player1)

        {
            if (chosenCard && chosenColumn)
            {
                if (player1Board.CheckForEmptyInColumn(chosenColumnIndex))
                {
                    Debug.Log(chosenCardIndex + " " + chosenColumnIndex);
                    chosenCard = chosenColumn = false;
                    player1Board.AddCardToColumn(HandManager.handManager.GetCardObjectByIndex(chosenCardIndex), chosenColumnIndex);
                    HandManager.handManager.RemoveCardFromHand(chosenCardIndex);
                    player1Board.ListBoard();
                    HandManager.handManager.ListHand();
                    UpdateScore();
                    UpdateTurn();
                }
                else
                {
                    Debug.Log("Column full! Pick again.");
                }
            }
        }   

        else if (currentTurn == TurnState.Player2)
        {
            
            if (chosenCard && chosenColumn)
            {
                if (player2Board.CheckForEmptyInColumn(chosenColumnIndex))
                {
                    Debug.Log(chosenCardIndex + " " + chosenColumnIndex);
                    chosenCard = chosenColumn = false;
                    player2Board.AddCardToColumn(HandManager.handManager.GetCardObjectByIndex(chosenCardIndex), chosenColumnIndex);
                    HandManager.handManager.RemoveCardFromHand(chosenCardIndex);
                    player2Board.ListBoard();
                    HandManager.handManager.ListHand();
                    UpdateScore();
                    UpdateTurn(); 
                }
                else
                {
                    Debug.Log("Column full! Pick again.");
                }
            }
        }
    }   

    //dupa
    public void UpdateScore()
    {
        int player1Points = player1Board.GetPoints();
        int player2Points = player2Board.GetPoints();

        player1ScoreText.text = player1Points.ToString();
        player2ScoreText.text = player2Points.ToString();

    }
    public void SetChosenCardIndex(int _chosenCardIndex)
    {
        chosenCard = true;
        chosenCardIndex = _chosenCardIndex;
    }
 
    public void SetChosenColumnIndex(int _chosenColumnIndex)
    {
        chosenColumn = true;
        chosenColumnIndex = _chosenColumnIndex;
    }
 

}