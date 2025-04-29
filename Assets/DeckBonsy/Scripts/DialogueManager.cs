using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public event Action OnDialogueEnd;

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

    private int currentRound = 0;
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

       
        StartCoroutine(ShowChoicesAfterNpcSpeech(dialogue.npcLine));
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
                choiceButtons[i].onClick.RemoveAllListeners();  // Usuwamy poprzednie eventy
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
        Debug.Log("Wybra³eœ opcjê: " + currentDialogue.playerChoices[choiceIndex]);
        Debug.Log("To prowadzi do zakoñczenia nr: " + currentDialogue.endings[choiceIndex]);

        // (wiêcej logiki)
        EndDialogue();
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
                    npcLine = "siema jestem triceps",
                    playerChoices = new string[] { "halo", "nie teraz" },
                    endings = new int[] { 0, 1 },
                    npcImage = npcImageRound0,
                    backgroundImage = backgroundImageRound0
                };
            case 1:
                return new DialogueData
                {
                    npcLine = "siema nie jestem triceps",
                    playerChoices = new string[] { "siema", "nie" },
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
}
