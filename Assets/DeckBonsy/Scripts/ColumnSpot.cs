using UnityEngine;

public class ColumnSpot : MonoBehaviour
{
    [SerializeField] private int columnIndex;

    public void SetColumnIndex(int _columnIndex)
    {
        columnIndex = _columnIndex;
    }

    public void WhenClicked()
    {
        Debug.Log("YIPPIE!");
        GameManager.gameManager.SetChosenColumnIndex(columnIndex);
    }
}
