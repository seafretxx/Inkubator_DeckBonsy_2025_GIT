using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public event Action OnDialogueEnd;
    private int lastPlayerChoice;

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI npcText;
    [SerializeField] private List<UnityEngine.UI.Button> choiceButtons;
    [SerializeField] private UnityEngine.UI.Image npcImageHolder;
    [SerializeField] private UnityEngine.UI.Image backgroundImageHolder;

    [Header("Background Images")]
    [SerializeField] private Sprite npcImageRound0;
    [SerializeField] private Sprite backgroundImageRound0;
    [SerializeField] private Sprite npcImageRound1;
    [SerializeField] private Sprite backgroundImageRound1;

    [Header("Button Images")]
    [SerializeField] private Sprite buttonImageNormal;
    [SerializeField] private Sprite buttonImageHighlighted;

    private DialogueData currentDialogue;
    private int currentRound = 0;

    private void Start()
    {
        HideDialoguePanel();
    }

    public DialogueData GetIntroDialogueForRound(int round) //przed rozgrywka
    {
        switch (round)
        {
            case 0:
                return new DialogueData
                {
                    npcLine = "NPC1 Witaj",
                    playerChoices = new string[0],
                    endings = new int[0],
                    npcImage = npcImageRound0,
                    backgroundImage = backgroundImageRound0
                };
            case 1:
                return new DialogueData
                {
                    npcLine = "NPC2 WItaj",
                    playerChoices = new string[0],
                    endings = new int[0],
                    npcImage = npcImageRound1,
                    backgroundImage = backgroundImageRound1
                };
            // kolejne NPC
            default:
                return null;
        }
    }

    public void StartIntroDialogue(DialogueData dialogue)
    {
        ShowDialoguePanel();
        currentDialogue = dialogue;
        npcText.text = dialogue.npcLine;

        if (dialogue.backgroundImage != null)
            backgroundImageHolder.sprite = dialogue.backgroundImage;

        if (dialogue.npcImage != null)
            npcImageHolder.sprite = dialogue.npcImage;

        HideChoiceButtons();

        StartCoroutine(AutoEndDialogue());
    }

    public void SetLastPlayerChoice(int choice)
    {
        lastPlayerChoice = choice;
    }
    public int GetLastPlayerChoice()
    {
        return lastPlayerChoice;
    }

    public void HideDialoguePanel()
    {
        dialoguePanel.SetActive(false);
    }

    public void ShowDialoguePanel()
    {
        dialoguePanel.SetActive(true);
    }

    public void StartDialogue(DialogueData dialogue)
    {
        ShowDialoguePanel();
        currentDialogue = dialogue;
        npcText.text = dialogue.npcLine;

        if (dialogue.backgroundImage != null)
            backgroundImageHolder.sprite = dialogue.backgroundImage;

        if (dialogue.npcImage != null)
            npcImageHolder.sprite = dialogue.npcImage;

        HideChoiceButtons();

        if (dialogue.playerChoices == null || dialogue.playerChoices.Length == 0)
        {
            StartCoroutine(AutoEndDialogue());
        }
        else
        {
            StartCoroutine(ShowChoicesAfterNpcSpeech(dialogue.npcLine));
        }

    }
    private void AddHoverEffect(Button button)
    {
        EventTrigger trigger = button.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = button.gameObject.AddComponent<EventTrigger>();

        trigger.triggers.Clear();

        var entryEnter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        entryEnter.callback.AddListener((data) =>
        {
            button.transform.localScale = Vector3.one * 0.95f;
        });
        trigger.triggers.Add(entryEnter);

        var entryExit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        entryExit.callback.AddListener((data) =>
        {
            button.transform.localScale = Vector3.one;
        });
        trigger.triggers.Add(entryExit);
    }

    private void HideChoiceButtons()
    {
        foreach (var button in choiceButtons)
        {
            button.gameObject.SetActive(false);
        }
    }

    private IEnumerator ShowChoicesAfterNpcSpeech(string npcSpeech)
    {
        yield return new WaitForSeconds(npcSpeech.Length * 0.1f);
        ShowChoiceButtons();
    }

    private void ShowChoiceButtons()
    {
        for (int i = 0; i < choiceButtons.Count; i++)
        {
            if (i < currentDialogue.playerChoices.Length)
            {
                choiceButtons[i].gameObject.SetActive(true);
                choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentDialogue.playerChoices[i];

                choiceButtons[i].GetComponent<UnityEngine.UI.Image>().sprite = buttonImageNormal;

                int choiceIndex = i;
                choiceButtons[i].onClick.RemoveAllListeners();
                choiceButtons[i].onClick.AddListener(() => OnChoiceSelected(choiceIndex));

                UnityEngine.UI.Button button = choiceButtons[i];
                button.onClick.AddListener(() => OnButtonClick(button));
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnButtonClick(UnityEngine.UI.Button button)
    {
        UnityEngine.UI.Image buttonImage = button.GetComponent<UnityEngine.UI.Image>();
        if (buttonImage != null)
        {
            buttonImage.sprite = buttonImageHighlighted;
        }
    }

    private void OnChoiceSelected(int choiceIndex)
    {
        SetLastPlayerChoice(choiceIndex);

        string playerResponse = currentDialogue.playerExpandedResponses[choiceIndex];
        string npcResponse = currentDialogue.npcResponses[choiceIndex];

        StartCoroutine(PlayChoiceSequence(playerResponse, npcResponse));
    }

    private IEnumerator PlayChoiceSequence(string playerLine, string npcLine)
    {
        npcText.text = "";
        npcText.color = Color.cyan;

        foreach (char c in playerLine)
        {
            npcText.text += c;
            yield return new WaitForSeconds(0.03f);
        }

        yield return new WaitForSeconds(1f);

        npcText.text = "";
        npcText.color = Color.white;

        foreach (char c in npcLine)
        {
            npcText.text += c;
            yield return new WaitForSeconds(0.03f);
        }

        yield return new WaitForSeconds(1f);

        EndDialogue();
    }

    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);
    }

    public DialogueData GetDialogueForRound(int round)
    {
        currentRound = round;
        switch (round)
        {
            case 0:
                return new DialogueData
                {
                    npcLine = "siema jestem triceps",
                    playerChoices = new string[] { "halo", "nie teraz" },
                    playerExpandedResponses = new string[]
                    {
                        "Tak naprawdê chcia³am tylko siê przywitaæ.",
                        "Nie mam teraz czasu na rozmowê."
                    },
                    npcResponses = new string[]
                    {
                        "Mi³o Ciê poznaæ!",
                        "No trudno, mo¿e innym razem."
                    },
                    endings = new int[] { 0, 1 },
                    npcImage = npcImageRound0,
                    backgroundImage = backgroundImageRound0
                };


            case 1:
                return new DialogueData
                {
                    npcLine = "siema nie jestem triceps",
                    playerChoices = new string[] { "siema", "nie" },
                    playerExpandedResponses = new string[]
                    {
                        "noooooooooooooooooo",
                        "Nie mam teraz czasu na rozmowê."
                    },
                    npcResponses = new string[]
                    {
                        "Mi³o Ciê poznaæ!",
                        "No trudno, mo¿e innym razem."
                    },
                    endings = new int[] { 0, 1 },
                    npcImage = npcImageRound1,
                    backgroundImage = backgroundImageRound1
                };
            default:
                return null;
        }
    }

    public void EndDialogue()
    {
        OnDialogueEnd?.Invoke();
    }
    private IEnumerator AutoEndDialogue()
    {
        yield return new WaitForSeconds(npcText.text.Length * 0.05f + 2f);
        EndDialogue();
    }

}
