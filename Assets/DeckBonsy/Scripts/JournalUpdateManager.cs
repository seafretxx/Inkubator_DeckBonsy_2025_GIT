using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class JournalUpdateManager : MonoBehaviour
{
    public static JournalUpdateManager Instance;

    public GameObject journalPanel;
    public TextMeshProUGUI pageText;
    public Image pageImage;
    public List<Sprite> pageSprites;
    private Coroutine typingCoroutine;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ShowNoteAfterDialogue(int npcIndex, int playerChoice)
    {
        string note = GetChoiceText(npcIndex, playerChoice);

        if (string.IsNullOrEmpty(note) || npcIndex < 0 || npcIndex >= pageSprites.Count)
        {
            Debug.LogWarning("B³êdny indeks NPC lub pusty tekst!");
            return;
        }

        JournalDataManager.Instance.AddOrUpdateNote(npcIndex, note);

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

    public string GetChoiceText(int npcIndex, int playerChoice)
    {
        if (npcIndex == 0)
        {
            if (playerChoice == 0)
                return "Gracz powiedzia³: 'Halo!'.\nNPC odpowiedzia³: 'Czeœæ! Mi³o Ciê widzieæ.'";
            else
                return "Gracz powiedzia³: 'Nie teraz'.\nNPC odpowiedzia³: 'Rozumiem. Mo¿e póŸniej?'.";
        }
        else if (npcIndex == 1)
        {
            if (playerChoice == 0)
                return "Gracz powiedzia³: 'Siema!'.\nNPC odpowiedzia³: 'Yo, stary znajomy!'.";
            else
                return "Gracz powiedzia³: 'Nie dzisiaj'.\nNPC odpowiedzia³: 'Szkoda...'.";
        }

        return $"Gracz wybra³ opcjê {playerChoice + 1} z NPC {npcIndex}.";
    }

}
