using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JournalManager : MonoBehaviour
{
    public static JournalManager journalManager;

    [Header("Journal UI Elements")]
    public GameObject journalPanel;
    public GameObject nextPageButton;
    public GameObject previousPageButton;
    public GameObject closeButton;

    [Header("Journal Pages")]
    private List<string> journalPages; // Lista przechowuj¹ca strony dziennika
    private List<int> playerChoices;

    [SerializeField] private Image pageImage;
    [SerializeField] private List<Sprite> pageSprites;
    [SerializeField] private TextMeshProUGUI pageText;



    private int currentPage = 0;

    private Dictionary<int, string> dynamicTexts = new Dictionary<int, string>();

    private void Awake()
    {
        if (journalManager != null && journalManager != this)
        {
            Destroy(this);
        }
        else
        {
            journalManager = this;
        }

        journalPages = new List<string>();
        playerChoices = new List<int>();
    }



    void Start()
    {
        if (journalPanel == null || pageText == null || nextPageButton == null || previousPageButton == null || closeButton == null || pageImage == null)
        {
            Debug.LogError("Brakuje!");
            return;
        }

        journalPanel.SetActive(false);
        nextPageButton.GetComponent<Button>().onClick.AddListener(NextPage);
        previousPageButton.GetComponent<Button>().onClick.AddListener(PreviousPage);
        closeButton.GetComponent<Button>().onClick.AddListener(CloseJournal);
        LoadJournalData();
        UpdateJournalPage();
    }

    public void OpenJournal(int pageIndex)
    {
        if (pageIndex < pageSprites.Count)
        {
            currentPage = pageIndex;
            journalPanel.SetActive(true);
            UpdateJournalPage();
        }
        else
        {
            Debug.LogWarning("Brak strony o numerze: " + pageIndex);
        }
    }


    public int GetTotalPages()
    {
        return journalPages.Count; //ile dostepnych stron
    }

    public void CloseJournal()
    {
        journalPanel.SetActive(false);
    }

    public void NextPage()
    {
        if (currentPage < pageSprites.Count - 1)
        {
            currentPage++;
            UpdateJournalPage();
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdateJournalPage();
        }
    }

    public void UpdateJournalPage()
    {
        if (currentPage >= 0 && currentPage < pageSprites.Count)
        {
            pageImage.sprite = pageSprites[currentPage];

            if (dynamicTexts.ContainsKey(currentPage))
            {
                if (journalPanel.activeInHierarchy) 
                {
                    if (typingCoroutine != null)
                        StopCoroutine(typingCoroutine);

                    typingCoroutine = StartCoroutine(AnimateNote(dynamicTexts[currentPage]));
                }
                else
                {
                    pageText.text = dynamicTexts[currentPage]; // fallback – bez animacji
                }
            }
            else
            {
                pageText.text = "";
            }
        }
    }


    public void AddNoteToPage(int pageIndex, string note)
    {
        if (dynamicTexts.ContainsKey(pageIndex))
            dynamicTexts[pageIndex] += "\n" + note;
        else
            dynamicTexts[pageIndex] = note;

        SaveJournalData();

    }

    private Coroutine typingCoroutine;

   private System.Collections.IEnumerator AnimateNote(string text)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        pageText.text = "";
        foreach (char c in text)
        {
            pageText.text += c;
            yield return new WaitForSeconds(0.02f); // prêdkoœæ pisania
        }
    }

    private void SaveJournalData()
    {
        foreach (var entry in dynamicTexts)
        {
            PlayerPrefs.SetString("JournalPage_" + entry.Key, entry.Value);
        }
        PlayerPrefs.Save();
    }

    private void LoadJournalData()
    {
        dynamicTexts = new Dictionary<int, string>();

        for (int i = 0; i < pageSprites.Count; i++)
        {
            string key = "JournalPage_" + i;
            if (PlayerPrefs.HasKey(key))
            {
                dynamicTexts[i] = PlayerPrefs.GetString(key);
            }
        }
    }
    /*public void OnPlayerDialogChoice(int npcIndex, int playerChoice)
    {
        string choiceText = GetChoiceText(npcIndex, playerChoice);

        if (JournalManager.journalManager != null)
        {
            JournalManager.journalManager.AddNoteToPage(npcIndex, choiceText);
        }

       //otworz automatycznie po zakoñczeniu dialogu
        JournalManager.journalManager.OpenJournal();
    }*/

    private string GetChoiceText(int npcIndex, int playerChoice)
    {
        //tekst w zale¿noœci od wyboru
        if (npcIndex == 0)
        {
            if (playerChoice == 1)
                return "Gracz wybra³ opcjê 1: Rozmowa z NPC 1.";
            else
                return "Gracz wybra³ opcjê 2: Zignorowanie NPC 1.";
        }
        else if (npcIndex == 1)
        {
            if (playerChoice == 1)
                return "Gracz wybra³ opcjê 1: Rozmowa z NPC 2.";
            else
                return "Gracz wybra³ opcjê 2: Zignorowanie NPC 2.";
        }
    
        // reszta NPC
        return "Nieznany wybór.";
    }

    /* private void EndDialogueAndUpdateJournal(int npcIndex, int playerChoice)
     {
         // Po zakoñczeniu dialogu, zaktualizuj dziennik
         OnPlayerDialogChoice(npcIndex, playerChoice);

         JournalManager.journalManager.OpenJournal();
     }*/

    /*public void EndDialogueWithNPC(int npcIndex, int playerChoice)
    {
        // Zakoñczenie dialogu, wybór gracza
        EndDialogueAndUpdateJournal(npcIndex, playerChoice);
    }*/


    public void SaveChoiceAndOpenJournal(int npcIndex, int playerChoice)
    {
        string note = GetChoiceText(npcIndex, playerChoice);
        AddNoteToPage(npcIndex, note);
        OpenJournal(npcIndex);
    }

    public void OpenEmptyOrExistingJournalPage(int pageIndex)
    {
        if (pageIndex >= pageSprites.Count)
        {
            while (pageSprites.Count <= pageIndex)
            {
                pageSprites.Add(null); 
            }
        }

        // Nie twórz tekstu jeœli nie ma ¿adnych danych — tylko otwórz dziennik
        currentPage = pageIndex;
        journalPanel.SetActive(true);
        UpdateJournalPage();
    }


}
