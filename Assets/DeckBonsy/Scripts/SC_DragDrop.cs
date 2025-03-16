using UnityEngine;

public class SC_Roman_Elite : MonoBehaviour
{
    private GameObject Canvas;
    
    private bool IsDragging = false;
    
    void Start()
    {
       
    }

    public void StartDrag()
    {
        IsDragging = true;
    }
    
    public void EndDrag()
    {
        IsDragging = false;
    }

    void Update()
    {
        if (IsDragging)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
          
        }
    }
}
