using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] private int currentRound;

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

    [Header("Draw Texts")]
    [SerializeField] private GameObject playerDrawText;
    [SerializeField] private GameObject enemyDrawText;

    [Header("Dialogues")]
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private GameObject dialoguePanel;

    [Header("Journal")]
    [SerializeField] private GameObject journalButton;
    public JournalManager journalManager;



    private void Awake()
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

    private void Start()
    {
        isCardBeingPlayed = false;
        chosenCard = false;
        chosenColumn = false;
        isPlayerTurn = true;
        gameReady = true;
        scoreToWin = 5;
        UpdateScore();
        endGamePanel.SetActive(false);
        restartButton.onClick.AddListener(RestartGame);
        currentRound = 0;
        journalButton.SetActive(true);
        journalButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OpenJournal);
    }

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
                            chosenCard = chosenColumn = false;
                            playerBoard.AddCardToColumn(HandManager.handManager.GetCardObjectByIndex(chosenCardIndex), chosenColumnIndex);
                            HandManager.handManager.RemoveCardFromHand(chosenCardIndex);
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
                            chosenCard = chosenColumn = false;
                            enemyBoard.AddCardToColumn(HandManager.handManager.GetCardObjectByIndex(chosenCardIndex), chosenColumnIndex);
                            HandManager.handManager.RemoveCardFromHand(chosenCardIndex);
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
            isPlayerTurn = false;
            UpdateDrawTexts();
        }
        else
        {
            isPlayerTurn = true;
            UpdateDrawTexts();
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

    private void UpdateDrawTexts()
    {
        if (isPlayerTurn)
        {
            playerDrawText.SetActive(true);
            enemyDrawText.SetActive(false);
        }
        else
        {
            playerDrawText.SetActive(false);
            enemyDrawText.SetActive(true);
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
        CheckForRoundEnd();
    }

    private void CheckForRoundEnd()
    {
        if (!gameReady) return;

        bool isPlayerFull = playerBoard.IsBoardFull();
        bool isEnemyFull = enemyBoard.IsBoardFull();

        int playerScore = playerBoard.CountScore();
        int enemyScore = enemyBoard.CountScore();

        bool boardFull = isPlayerFull || isEnemyFull;
        bool playerWonByScore = playerScore >= scoreToWin;
        bool enemyWonByScore = enemyScore >= scoreToWin;

        string result = "";

        if (playerWonByScore && playerScore > enemyScore)
        {
            result = "PLAYER WINS!";
            gameReady = false;
            StartDialogueScene(dialogueManager.GetDialogueForRound(currentRound));
            //OpenJournalAfterWin();
        }
        else if (enemyWonByScore && enemyScore > playerScore)
        {
            result = "ENEMY WINS!";
            gameReady = false;
            endGamePanel.SetActive(true);
            scoreText.text = $"PLAYER: {playerScore}    ENEMY: {enemyScore}";
            resultText.text = result.ToUpper();
        }
        else if (boardFull)
        {
            if (playerScore > enemyScore)
            {
                result = "PLAYER WINS!";
                gameReady = false;
                StartDialogueScene(dialogueManager.GetDialogueForRound(currentRound));
            }
            else if (enemyScore > playerScore)
            {
                result = "ENEMY WINS!";
                gameReady = false;
                endGamePanel.SetActive(true);
                scoreText.text = $"PLAYER: {playerScore}    ENEMY: {enemyScore}";
                resultText.text = result.ToUpper();
            }
            else
            {
                result = "DRAW!";
                gameReady = false;
                StartDialogueScene(dialogueManager.GetDialogueForRound(currentRound));
            }
        }
    }

    public void StartDialogueScene(DialogueData dialogue)
    {
        gameReady = false;
        dialoguePanel.SetActive(true);
        dialogueManager.StartDialogue(dialogue);
        currentRound++;

        dialogueManager.OnDialogueEnd -= ContinueGameAfterDialogue;
        dialogueManager.OnDialogueEnd += ContinueGameAfterDialogue;
    }

    private void ContinueGameAfterDialogue()
    {
        dialoguePanel.SetActive(false);

        int npcIndex = currentRound - 1; 
        int playerChoice = dialogueManager.GetLastPlayerChoice();

        if (JournalManager.journalManager != null && npcIndex >= 0)
        {
            JournalManager.journalManager.SaveChoiceAndOpenJournal(npcIndex, playerChoice);
        }

        currentRound++;

        gameReady = true;
        isPlayerTurn = true;

        DeckManager.deckManager.ResetDeck();
        HandManager.handManager.ClearHand();
        playerBoard.ClearBoard();
        enemyBoard.ClearBoard();

        UpdateScore();
    }









    //private void OpenJournalAfterWin()
    //{

    //    SceneManager.LoadScene(journalSceneName);
    // }

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
    private void OpenJournal()
    {
        if (journalManager != null)
        {
            journalManager.OpenJournal(0); 
        }
    }



    //public void OpenJournalAfterWin(int enemyIndex)
    //{
    //    UnityEngine.SceneManagement.SceneManager.LoadScene(journalSceneName);
    //    JournalManager.journalManager.OpenJournalPage(enemyIndex);  
    //}
    // if (JournalManager.journalManager != null)
    //{
    //JournalManager.journalManager.AddNoteToPage(2, "Walczyłam z lisem.");
    //}
}
