using UnityEngine;

[System.Serializable]
public class DialogueOption
{
    public string question;
    public DialogueSentence[] answers; // Her bir cevap i�in DialogueSentence kullan�yoruz
}

[System.Serializable]
public class DialogueSentence
{
    public string text;
    public AudioClip audioClip;
}

[System.Serializable]
public class Dialogue
{
    public string npcName;
    public DialogueSentence[] sentences;
    public DialogueOption[] options;
}
