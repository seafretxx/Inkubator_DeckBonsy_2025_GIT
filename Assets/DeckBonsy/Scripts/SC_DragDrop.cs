using UnityEngine;
using UnityEngine.EventSystems;

public class SC_Roman_Elite : MonoBehaviour
{
    private bool IsDragging = false;
    private Vector3 startPosition;
    private Transform startParent;

    void Start()
    {
        startParent = transform.parent; // Zapisujemy pocz¹tkowego rodzica (rêka)
    }

    public void StartDrag()
    {
        IsDragging = true;
        startPosition = transform.position;
    }

    public void EndDrag()
    {
        IsDragging = false;

        GameObject dropZone = GameObject.Find("DropZone"); // ZnajdŸ DropZone
        if (dropZone != null && IsOverDropZone(dropZone))
        {
            transform.SetParent(dropZone.transform, false); // Ustawienie karty w DropZone
            transform.position = dropZone.transform.position; // Blokowanie pozycji karty
        }
        else
        {
            transform.position = startPosition; // Jeœli nie trafi³a w DropZone, wraca na rêkê
        }
    }

    void Update()
    {
        if (IsDragging)
        {
            transform.position = Input.mousePosition; // Karta porusza siê za myszk¹
        }
    }

    private bool IsOverDropZone(GameObject dropZone)
    {
        RectTransform dropRect = dropZone.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(dropRect, Input.mousePosition);
    }
}
