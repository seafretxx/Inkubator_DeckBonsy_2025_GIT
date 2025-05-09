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
    public int CurrentRound => currentRound;

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
    [SerializeField] private JournalUpdateManager journalUpdateManager;

    private bool journalOpenedThisDialogue = false;

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

        if (journalUpdateManager == null)
        {
            journalUpdateManager = FindFirstObjectByType<JournalUpdateManager>();
            if (journalUpdateManager == null)
            {
                Debug.LogError("JournalUpdateManager nie znaleziony!");
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
        scoreToWin = 5;
        UpdateScore();
        endGamePanel.SetActive(false);
        restartButton.onClick.AddListener(RestartGame);
        currentRound = 0;
        journalOpenedThisDialogue = false;

        if (journalButton != null)
        {
            journalButton.SetActive(true);
            journalButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OpenJournal);
        }
        if (currentRound == 0 && PlayerPrefs.HasKey("JournalPage_0"))
        {
            PlayerPrefs.DeleteKey("JournalPage_0");
            PlayerPrefs.Save();
        }
    }

    private void Update()
    {
        if (!gameReady) return;

        if (!isCardBeingPlayed && chosenCard && chosenColumn)
        {
            isCardBeingPlayed = true;
            cardContainerBeingPlayed = HandManager.handManager.GetCardObjectByIndex(chosenCardIndex).GetComponent<CardContainer>();

            if (isPlayerTurn && playerBoard.CheckForEmptyInColumn(chosenColumnIndex))
            {
                playerBoard.AddCardToColumn(HandManager.handManager.GetCardObjectByIndex(chosenCardIndex), chosenColumnIndex);
                HandManager.handManager.RemoveCardFromHand(chosenCardIndex);
                ResetChoices();
                UpdateScore();
                CheckForRoundEnd();
            }
            else if (!isPlayerTurn && enemyBoard.CheckForEmptyInColumn(chosenColumnIndex))
            {
                enemyBoard.AddCardToColumn(HandManager.handManager.GetCardObjectByIndex(chosenCardIndex), chosenColumnIndex);
                HandManager.handManager.RemoveCardFromHand(chosenCardIndex);
                ResetChoices();
                UpdateScore();
                CheckForRoundEnd();
            }
            else
            {
                Debug.Log("Column full! Pick again.");
                ResetChoices();
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
                    isCardBeingPlayed = false;
                    EffectManager.effectManager.TriggerCardEffect(playedCard.effectId, cardContainerBeingPlayed, chosenCardContainerInPlayObject);
                    RemoveCardsWithEqualPoints(chosenColumnIndex, playedCard.points);
                    EndTurn();
                    chosenCardContainerInPlay = false;
                }
            }
            else
            {
                isCardBeingPlayed = false;
                EffectManager.effectManager.TriggerCardEffect(playedCard.effectId, cardContainerBeingPlayed, null);
                RemoveCardsWithEqualPoints(chosenColumnIndex, playedCard.points);
                EndTurn();
            }
        }
    }

    public bool GetPlayerTurn()
    {
        return isPlayerTurn;
    }

    private void ResetChoices()
    {
        chosenCard = false;
        chosenColumn = false;
    }

    private bool DoesEffectIdRequireInput(int effectId)
    {
        return effectId == 5 || effectId == 7 || effectId == 8;
    }

    public void RemoveCardsWithEqualPoints(int columnIndex, int cardPoints)
    {
        if (isPlayerTurn)
            enemyBoard.RemoveCardsFromColumn(columnIndex, cardPoints);
        else
            playerBoard.RemoveCardsFromColumn(columnIndex, cardPoints);
    }

    public int CountTypeOfCardOnBoard(CardType type, bool isPlayerBoard)
    {
        return isPlayerBoard ? playerBoard.CountTypeOnBoard(type) : enemyBoard.CountTypeOnBoard(type);
    }

    public int CountTypeOfCardInColumn(CardType type, bool isPlayerBoard, int columnIndex)
    {
        return isPlayerBoard ? playerBoard.CountTypeInColumn(type, columnIndex) : enemyBoard.CountTypeInColumn(type, columnIndex);
    }

    public void EndTurn()
    {
        isPlayerTurn = !isPlayerTurn;
        UpdateDrawTexts();
        UpdateBackground();
    }

    private void UpdateBackground()
    {
        backgroundImage.sprite = isPlayerTurn ? playerBackground : enemyBackground;
    }

    private void UpdateDrawTexts()
    {
        playerDrawText.SetActive(isPlayerTurn);
        enemyDrawText.SetActive(!isPlayerTurn);
    }

    public void SetChosenCardIndex(int _chosenCardIndex, bool _isPlayerCard)
    {
        if (isPlayerTurn == _isPlayerCard)
        {
            chosenCard = true;
            chosenCardIndex = _chosenCardIndex;
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
    }

    public void UpdateScore()
    {
        playerScoreCounter.text = $"Your score:\n{playerBoard.CountScore()}";
        enemyScoreCounter.text = $"Enemy score:\n{enemyBoard.CountScore()}";
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

        if (playerScore >= scoreToWin && playerScore > enemyScore)
        {
            FinishGame("PLAYER WINS!", playerScore, enemyScore, true);
        }
        else if (enemyScore >= scoreToWin && enemyScore > playerScore)
        {
            FinishGame("ENEMY WINS!", playerScore, enemyScore, false);
        }
        else if (boardFull)
        {
            if (playerScore > enemyScore)
                FinishGame("PLAYER WINS!", playerScore, enemyScore, true);
            else if (enemyScore > playerScore)
                FinishGame("ENEMY WINS!", playerScore, enemyScore, false);
            else
                FinishGame("DRAW!", playerScore, enemyScore, true); // Treat as win
        }
    }

    private void FinishGame(string result, int playerScore, int enemyScore, bool showDialogue)
    {
        gameReady = false;

        if (showDialogue)
        {
            StartDialogueScene(dialogueManager.GetDialogueForRound(currentRound));
        }
        else
        {
            endGamePanel.SetActive(true);
            resultText.text = result.ToUpper();
            scoreText.text = $"PLAYER: {playerScore}    ENEMY: {enemyScore}";
        }
    }

    public void StartDialogueScene(DialogueData dialogue)
    {
        gameReady = false;
        dialoguePanel.SetActive(true);
        dialogueManager.StartDialogue(dialogue);

        dialogueManager.OnDialogueEnd -= ContinueGameAfterDialogue;
        dialogueManager.OnDialogueEnd += ContinueGameAfterDialogue;
    }

    private void ContinueGameAfterDialogue()
    {
        dialoguePanel.SetActive(false);

        int npcIndex = currentRound;
        int playerChoice = dialogueManager.GetLastPlayerChoice(); 

        if (!journalOpenedThisDialogue && npcIndex >= 0 && playerChoice >= 0 && journalUpdateManager != null)
        {
            journalOpenedThisDialogue = true;

            journalUpdateManager.ShowNoteAfterDialogue(npcIndex, playerChoice);
        }

        currentRound++;
        journalOpenedThisDialogue = false; 
        gameReady = true;
        isPlayerTurn = true;

        DeckManager.deckManager.ResetDeck();
        HandManager.handManager.ClearHand();
        playerBoard.ClearBoard();
        enemyBoard.ClearBoard();

        UpdateScore();
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

    private void OpenJournal()
    {
        var display = FindFirstObjectByType<JournalDisplayManager>();
        if (display != null)
        {
            int pageToOpen = currentRound > 0 ? currentRound - 1 : 0;
            display.OpenJournal(pageToOpen);
        }
        else
        {
            Debug.LogError("Brak JournalDisplayManager w scenie!");
        }
    }
}
