using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // jeœli u¿ywasz TMP do wyœwietlania tekstu

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI npcText;
    [SerializeField] private List<UnityEngine.UI.Button> choiceButtons;

    private DialogueData currentDialogue;

    private void Start()
    {
        HideDialoguePanel();  
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

        for (int i = 0; i < choiceButtons.Count; i++)
        {
            if (i < dialogue.playerChoices.Length)
            {
                choiceButtons[i].gameObject.SetActive(true);
                choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = dialogue.playerChoices[i];
                int choiceIndex = i;
                choiceButtons[i].onClick.RemoveAllListeners();
                choiceButtons[i].onClick.AddListener(() => OnChoiceSelected(choiceIndex));
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnChoiceSelected(int choiceIndex)
    {
        Debug.Log("Wybra³eœ opcjê: " + currentDialogue.playerChoices[choiceIndex]);
        Debug.Log("To prowadzi do zakoñczenia nr: " + currentDialogue.endings[choiceIndex]);

        
        dialoguePanel.SetActive(false);

        // dziennik
    }
    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);
    }
    public DialogueData GetDialogueForRound(int round)
    {
        switch (round)
        {
            case 0:
                return new DialogueData
                {
                    npcLine = "",
                    playerChoices = new string[] { "", "?",},
                    endings = new int[] { 0, 1, 2 }
                };
            case 1:
                return new DialogueData
                {
                    npcLine = "",
                    playerChoices = new string[] { "Tak", "Nie",},
                    endings = new int[] { 0, 1, 2 }
                };
            default:
                return null;
        }
    }
}
