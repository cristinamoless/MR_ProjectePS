using UnityEngine;

public enum DialogueType
{
    Initial,
    Happy,
    Sad,
    Repartidor,
    Choice,
    Neutral
}

[System.Serializable]
public class DialogueChoice
{
    public string choiceText;    
    public Dialogue nextDialogue; 
}

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/Dialogue")]
public class Dialogue : ScriptableObject
{
    public string characterName;
    public Sprite characterPixel;

    [TextArea(3, 10)]
    public string[] sentences;

    public int day;
    public int orderIndex;
    public DialogueType type;

    public DialogueChoice[] choices;  
}
