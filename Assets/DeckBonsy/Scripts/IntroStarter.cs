using UnityEngine;
using System.Collections;

public class IntroStarter : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject startGameButton;
    [SerializeField] private GameObject journalButton;

    private bool introEnded = false;
    private bool waitingForPlayerInput = false;

    private void Start()
    {
        StartCoroutine(DelayedIntro());
    }

    private IEnumerator DelayedIntro()
    {
        // czeka aż gameManager i dialogueManager pojawią się w scenie
        yield return new WaitUntil(() => GameManager.gameManager != null && FindFirstObjectByType<DialogueManager>() != null);

        gameManager = GameManager.gameManager;
        dialogueManager = FindFirstObjectByType<DialogueManager>();

        if (gameManager == null || dialogueManager == null)
        {
            Debug.LogError("❌ GameManager lub DialogueManager nadal nie znaleziony!");
            yield break;
        }

        if (gameManager.CurrentRound != 0)
        {
            this.enabled = false;
            yield break;
        }

        var preGameIntro = dialogueManager.GetPreGameIntro();
        if (preGameIntro != null)
        {
            SetGameReady(false);
            dialogueManager.StartDialogue(preGameIntro);
            dialogueManager.OnDialogueEnd += OnPreIntroEnded;
        }
        else
        {
            StartMainIntro(); // fallback
        }
    }

    private void OnPreIntroEnded()
    {
        dialogueManager.OnDialogueEnd -= OnPreIntroEnded;
        StartMainIntro();
    }

    private void StartMainIntro()
    {
        var intro = dialogueManager.GetIntroDialogueForRound(0);
        if (intro != null)
        {
            SetGameReady(false);
            dialogueManager.StartDialogue(intro);
            dialogueManager.OnDialogueEnd += OnIntroEnded;
        }
    }

    private void OnIntroEnded()
    {
        dialogueManager.OnDialogueEnd -= OnIntroEnded;
        introEnded = true;
        waitingForPlayerInput = true;
    }

    private void Update()
    {
        if (!waitingForPlayerInput) return;

        if (introEnded)
        {
            StartCoroutine(AutoStartAfterDelay(1f));
            introEnded = false;
        }
    }

    private IEnumerator AutoStartAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (waitingForPlayerInput)
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        SetGameReady(false);
        SetStartCardGameAfterIntro(true);
        SetCurrentRound(0);

        if (startGameButton != null)
            startGameButton.SetActive(false);

        if (journalButton != null)
        {
            journalButton.SetActive(true);

            var display = FindFirstObjectByType<JournalDisplayManager>();
            if (display != null)
            {
                display.SetMaxPageIndex(0);
                journalButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
                {
                    display.OpenJournal(0);
                });
            }
        }
    }

    private void SetGameReady(bool value)
    {
        typeof(GameManager).GetField("gameReady", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                           ?.SetValue(gameManager, value);
    }

    private void SetStartCardGameAfterIntro(bool value)
    {
        typeof(GameManager).GetField("startCardGameAfterIntro", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                           ?.SetValue(gameManager, value);
    }

    private void SetCurrentRound(int round)
    {
        typeof(GameManager).GetField("currentRound", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                           ?.SetValue(gameManager, round);
    }
}
