using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
public class SC_PlayerAreaSetup : MonoBehaviour

{
    public GameObject playerArea;
    public int numberOfColumns = 3;
    public GameObject cardPrefab;

    public List<Transform> columns = new List<Transform>();
    private int maxCardsPerColumn = 3;

    void Start()
    {
        CreateColumns();
    }

    void CreateColumns()
    {
        for (int i = 0; i < numberOfColumns; i++)
        {
            GameObject column = new GameObject("Column" + i);
            column.transform.SetParent(playerArea.transform, false);

            VerticalLayoutGroup layout = column.AddComponent<VerticalLayoutGroup>();
            layout.childAlignment = TextAnchor.LowerCenter;
            layout.spacing = -10f;
            layout.childControlHeight = true;
            layout.childControlWidth = true;
            layout.childForceExpandHeight = false;
            layout.childForceExpandWidth = false;

            ContentSizeFitter fitter = column.AddComponent<ContentSizeFitter>();
            fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            Image bg = column.AddComponent<Image>();
            bg.color = new Color(1, 1, 1, 1); // przezroczysty

            columns.Add(column.transform);
        }
    }

    public void PlayCardToColumn(GameObject cardToPlay, int columnIndex)
    {
        
        if (columnIndex < 0 || columnIndex >= columns.Count)
        {
            Debug.LogError("Niepoprawny indeks kolumny!");
            return;
        }

        Transform selectedColumn = columns[columnIndex];

        
        if (selectedColumn.childCount < maxCardsPerColumn)
        {
            
            cardToPlay.transform.SetParent(selectedColumn, false);
            RectTransform cardRect = cardToPlay.GetComponent<RectTransform>();

           
            cardRect.localPosition = new Vector3(cardRect.localPosition.x, selectedColumn.childCount * -100, 0); 
            Debug.Log($"Karta dodana do kolumny {columnIndex}");
        }
        else
        {
            Debug.Log("Kolumna jest pe³na!");
        }
    }
}


