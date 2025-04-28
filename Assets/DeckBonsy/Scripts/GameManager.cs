using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager { get; private set; }

    [Header("Main Variables")]
    [SerializeField] public bool isPlayerTurn { get; private set; }
    [SerializeField] private bool gameReady;
    [SerializeField] private bool isCardBeingPlayed;
    [SerializeField] private CardContainer cardContainerBeingPlayed;
    [SerializeField] private int scoreToWin;


    [Header("Input System")]
    [SerializeField] private bool chosenCard;
    [SerializeField] private int chosenCardIndex;
    [SerializeField] private bool chosenColumn;
    [SerializeField] private int chosenColumnIndex;
    [SerializeField] private bool chosenCardContainerInPlay;
    [SerializeField] private CardContainer chosenCardContainerInPlayObject;
    [SerializeField] public int selectedCardIndex;

    [Header("Board References")]
    [SerializeField] private Board playerBoard;
    [SerializeField] private Board enemyBoard;
    [SerializeField] private TextMeshProUGUI playerScoreCounter;
    [SerializeField] private TextMeshProUGUI enemyScoreCounter;

    [Header("Background Sprites")]
    [SerializeField] private UnityEngine.UI.Image backgroundImage;
    [SerializeField] private Sprite playerBackground;
    [SerializeField] private Sprite enemyBackground;

    [Header("End Game UI")]
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private UnityEngine.UI.Button restartButton;




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
        isCardBeingPlayed = false;
        chosenCard = false;
        chosenColumn = false;
        isPlayerTurn = true;
        gameReady = true;
        scoreToWin = 40;
        UpdateScore();
        endGamePanel.SetActive(false);
        restartButton.onClick.AddListener(RestartGame);

    }
    //if (!gameReady) CheckForRoundEnd();
    //   return;
    private void Update()
    { 
        if (gameReady)
        {
            if (!isCardBeingPlayed)
            {
                if (chosenCard && chosenColumn) // PLAYING A CARD
                {
                    isCardBeingPlayed = true;
                    cardContainerBeingPlayed = HandManager.handManager.GetCardObjectByIndex(chosenCardIndex).GetComponent<CardContainer>();
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
                            CheckForRoundEnd();

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
                            CheckForRoundEnd();

                        }
                        else
                        {
                            chosenCard = chosenColumn = false;
                            Debug.Log("Column full! Pick again.");
                        }
                    }
                }
            }
            if (isCardBeingPlayed)
            {
                Card playedCard = cardContainerBeingPlayed.GetCardInfo();
                Board inactiveBoard = isPlayerTurn ? enemyBoard : playerBoard;

                if (DoesEffectIdRequireInput(playedCard.effectId) && !inactiveBoard.IsBoardEmpty())
                {
                    Debug.Log("Requires input!");
                    if (chosenCardContainerInPlay)
                    {
                        chosenCardContainerInPlay = false;
                        isCardBeingPlayed = false;
                        RemoveCardsWithEqualPoints(chosenColumnIndex, playedCard.points);
                        EffectManager.effectManager.TriggerCardEffect(playedCard.effectId, cardContainerBeingPlayed, chosenCardContainerInPlayObject);
                        EndTurn();
                    }
                }
                else
                {
                    Debug.Log("Does not require input!");
                    chosenCardContainerInPlay = false;
                    isCardBeingPlayed = false;
                    RemoveCardsWithEqualPoints(chosenColumnIndex, playedCard.points);
                    EffectManager.effectManager.TriggerCardEffect(playedCard.effectId, cardContainerBeingPlayed, null);
                    EndTurn();
                }
            }
        }
        
    }

    private bool DoesEffectIdRequireInput(int effectId)
    {
        if (effectId == 5)
            return true;
        if (effectId == 7)
            return true;
        if (effectId == 8)
            return true;
        return false;
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
    public int CountTypeOfCardOnBoard(CardType type, bool isPlayerBoard)
    {
        if (isPlayerBoard)
        {
            return playerBoard.CountTypeOnBoard(type);
        }
        else
        {
            return enemyBoard.CountTypeOnBoard(type);
        }
    }

    public int CountTypeOfCardInColumn(CardType type, bool isPlayerBoard, int columnIndex)
    {
        if (isPlayerBoard)
        {
            return playerBoard.CountTypeInColumn(type, columnIndex);
        }
        else
        {
            return enemyBoard.CountTypeInColumn(type, columnIndex);
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
        UpdateBackground();
    }
    private void UpdateBackground()
    {
        if (isPlayerTurn)
        {
            backgroundImage.sprite = playerBackground;
        }
        else
        {
            backgroundImage.sprite = enemyBackground;
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
    public void SetChosenCardInPlayObject(CardContainer _chosenCardContainerInPlayObject)
    {
        chosenCardContainerInPlay = true;
        chosenCardContainerInPlayObject = _chosenCardContainerInPlayObject;
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
    private void CheckForRoundEnd()
    {
        bool isPlayerFull = playerBoard.IsBoardFull();
        bool isEnemyFull = enemyBoard.IsBoardFull();

        int playerScore = playerBoard.CountScore();
        int enemyScore = enemyBoard.CountScore();

        

        if (isPlayerFull || isEnemyFull || playerScore >= scoreToWin || enemyScore >= scoreToWin)
        {
            
            string result;

            if (playerScore > enemyScore)
                result = "PLAYER WINS!";
            else if (enemyScore > playerScore)
                result = "ENEMY WINS!";
            else
                result = "DRAW!";

            Debug.Log("ROUND OVER!");
            Debug.Log("Player: " + playerScore + "  Enemy: " + enemyScore);
            Debug.Log(result);

            gameReady = false;


            endGamePanel.SetActive(true);
            scoreText.text = "PLAYER: " + playerScore + "    ENEMY: " + enemyScore;
            resultText.text = result.ToUpper();

        }
    }

     private void RestartGame()
        {
            endGamePanel.SetActive(false);
            gameReady = true;
            isPlayerTurn = true;
            chosenCard = false;
            chosenColumn = false;

            DeckManager.deckManager.ResetDeck();
            HandManager.handManager.ClearHand();
            playerBoard.ClearBoard();
            enemyBoard.ClearBoard();

            UpdateScore();
        }


}