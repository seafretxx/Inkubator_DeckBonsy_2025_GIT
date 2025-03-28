using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager { get; private set; }

    [Header("Main Variables")]
    [SerializeField] private bool isPlayerTurn;
    [SerializeField] private bool gameReady;
    [SerializeField] public int selectedCardIndex;

    [Header("[TEMP] Input System")]
    [SerializeField] private bool chosenCard;
    [SerializeField] private int chosenCardIndex;
    [SerializeField] private bool chosenColumn;
    [SerializeField] private int chosenColumnIndex;

    [Header("Board References")]
    [SerializeField] private Board playerBoard;
    [SerializeField] private Board enemyBoard;

    private void Awake()
    {
        /// Singleton mechanism
        {
            if (gameManager != null && gameManager != this)
            {
                Destroy(this);
            }
            else
            {
                gameManager = this;
            }
        }
    }
    
    private void Start()
    {
        gameReady = true;
        chosenCard = false;
    }

    private void Update()
    {
        if(chosenCard&&chosenColumn)
        {
            Debug.Log(chosenCardIndex + " " + chosenColumnIndex);
            chosenCard = chosenColumn = false;
            playerBoard.AddCardToColumn(HandManager.handManager.GetCardByIndex(chosenCardIndex), chosenColumnIndex);
            HandManager.handManager.RemoveCardFromHand(chosenCardIndex);
        }
    }

    public void SetChosenCardIndex(int _chosenCardIndex)
    {
        chosenCard = true;
        chosenCardIndex = _chosenCardIndex;
    }
 
    public void SetChosenColumnIndex(int _chosenColumnIndex)
    {
        chosenColumn = true;
        chosenColumnIndex = _chosenColumnIndex;
    }
 

}