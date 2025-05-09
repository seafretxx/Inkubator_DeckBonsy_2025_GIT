using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class JournalUpdateManager : MonoBehaviour
{
    public GameObject journalPanel;
    public TextMeshProUGUI pageText;
    public Image pageImage;
    public List<Sprite> pageSprites;
    private Coroutine typingCoroutine;

    public void ShowNoteAfterDialogue(int npcIndex, int playerChoice)
    {
        string note = GetChoiceText(npcIndex, playerChoice);

        JournalDataManager.Instance.AddOrUpdateNote(npcIndex, note, true); 


        journalPanel.SetActive(true);
        pageImage.sprite = pageSprites[npcIndex];

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeNote(note));
    }

    private IEnumerator TypeNote(string note)
    {
        pageText.text = "";
        foreach (char c in note)
        {
            pageText.text += c;
            yield return new WaitForSeconds(0.03f);
        }
    }

    public void ClosePanel()
    {
        journalPanel.SetActive(false);
    }

    private string GetChoiceText(int npcIndex, int playerChoice)
    {
        if (npcIndex == 0)
            return playerChoice == 0 ? "Gracz wybra³ opcjê 1: Rozmowa z NPC 1." : "Gracz wybra³ opcjê 2: Zignorowanie NPC 1.";
        else if (npcIndex == 1)
            return playerChoice == 0 ? "Gracz wybra³ opcjê 1: Rozmowa z NPC 2." : "Gracz wybra³ opcjê 2: Zignorowanie NPC 2.";
        else
            return $"Gracz wybra³ opcjê {playerChoice + 1}: z NPC {npcIndex}.";
    }
}
