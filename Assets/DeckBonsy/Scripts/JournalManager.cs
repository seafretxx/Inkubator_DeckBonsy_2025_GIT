using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class JournalManager : MonoBehaviour
{
    public static JournalManager journalManager;

    [Header("Journal UI Elements")]
    public GameObject journalPanel; 
    public TextMeshProUGUI journalText;  
    public GameObject nextPageButton;  
    public GameObject previousPageButton;  
    public GameObject closeButton;  

    private int currentPage = 0;
    private string[] journalPages = {
        "Strona 1: Treœæ dziennika...",
        "Strona 2: Kolejna treœæ...",
        "Strona 3: Kolejna strona...",
       
    };

    void Start()
    {
       
        if (journalPanel == null || journalText == null || nextPageButton == null || previousPageButton == null || closeButton == null)
        {
            Debug.LogError("Nie przypisano wszystkich elementów");
            return;
        }

        journalPanel.SetActive(false);  
        nextPageButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(NextPage);
        previousPageButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(PreviousPage);
        closeButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(CloseJournal);

        UpdateJournalPage();
    }

    public void OpenJournal()
    {
        journalPanel.SetActive(true);  
        currentPage = 0;  
        UpdateJournalPage();  
    }

    public void CloseJournal()
    {
        journalPanel.SetActive(false); 
    }

    public void NextPage()
    {
        if (currentPage < journalPages.Length - 1)
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

    private void UpdateJournalPage()
    {
        if (journalPages.Length > 0)
        {
            journalText.text = journalPages[currentPage];  
        }
        else
        {
            Debug.LogError("Brak stron dziennika!");
        }
    }
}
