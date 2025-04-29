using UnityEngine;


[System.Serializable]
public class DialogueData
{
    public string npcLine;
    public string[] playerChoices;
    public int[] endings; // Id zakoñczeñ 
    public Sprite npcImage;  
    public Sprite backgroundImage;
}
