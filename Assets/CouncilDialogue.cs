using UnityEngine;

[System.Serializable]
public class CouncilDialogueOption
{
    public CouncilDialogueSentence[] answers; // Use CouncilDialogueSentence instead of DialogueSentence
}

[System.Serializable]
public class CouncilDialogueSentence
{

    public string npcName;
    public string text;
    public AudioClip audioClip;
}

[System.Serializable]
public class CouncilDialogue
{
    public CouncilDialogueSentence[] sentences; // Use CouncilDialogueSentence instead of DialogueSentence
}
