using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class JournalDisplayManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject journalPanel;
    public Image pageImage;
    public TextMeshProUGUI pageText;
    public Button nextPageButton;
    public Button previousPageButton;
    public Button closeButton;
    public List<Sprite> pageSprites;

    private int currentPage = 0;

    private void Start()
    {
        journalPanel.SetActive(false);

        if (nextPageButton) nextPageButton.onClick.AddListener(NextPage);
        if (previousPageButton) previousPageButton.onClick.AddListener(PreviousPage);
        if (closeButton) closeButton.onClick.AddListener(CloseJournal);
    }

    public void OpenJournal(int pageIndex)
    {
        if (pageIndex < 0 || pageIndex >= pageSprites.Count)
        {
            Debug.LogWarning("Nieprawid³owy indeks strony: " + pageIndex);
            return;
        }

        currentPage = pageIndex;
        journalPanel.SetActive(true);
        UpdateJournalPage();
    }

    private void CloseJournal()
    {
        journalPanel.SetActive(false);
    }

    private void NextPage()
    {
        if (currentPage < pageSprites.Count - 1)
        {
            currentPage++;
            UpdateJournalPage();
        }
    }

    private void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdateJournalPage();
        }
    }

    private void UpdateJournalPage()
    {
        if (currentPage >= 0 && currentPage < pageSprites.Count)
        {
            pageImage.sprite = pageSprites[currentPage];

            string text = JournalDataManager.Instance.GetTextForPage(currentPage);

            pageText.text = string.IsNullOrWhiteSpace(text) ? "<i>Brak zapisu dla tej strony</i>" : text;
        }
    }
}
