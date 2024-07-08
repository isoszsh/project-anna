using UnityEngine;

[System.Serializable]
public class DialogueOption
{
    public string question;
    public DialogueSentence[] answers; // Her bir cevap için DialogueSentence kullanýyoruz
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
