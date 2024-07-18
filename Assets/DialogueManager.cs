using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    // Singleton instance
    private static DialogueManager instance;

    public Sprite dialogueSprite;

    // Public property for accessing the instance
    public static DialogueManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DialogueManager>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<DialogueManager>();
                    singletonObject.name = typeof(DialogueManager).ToString() + " (Singleton)";
                }
            }
            return instance;
        }
    }

    // DialogueManager variables
    public TextMeshProUGUI npcNameText;
    public TextMeshProUGUI dialogueText;
    public Button[] optionButtons;
    public AudioSource audioSource; // AudioSource bile�eni ekleyin
    private Queue<DialogueSentence> sentences;
    private Dialogue currentDialogue;

    // Typewriter effect variables
    public float typeSpeed = 0.05f; // Character display speed
    private bool isTyping = false;
    private string currentSentence = "";
    public bool isDialogueActive = false;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Makes the instance persist across scenes
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<DialogueSentence>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        GameManager.Instance.playerController.lockControls = true;

        currentDialogue = dialogue;
        npcNameText.text = dialogue.npcName + ":";
        sentences.Clear();

        foreach (DialogueSentence sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        isDialogueActive = true; // Set the flag to indicate active dialogue

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        
        if (sentences.Count == 0)
        {
            DisplayOptions();
            return;
        }

        DialogueSentence sentence = sentences.Dequeue();
        currentSentence = sentence.text;

        StopAllCoroutines();

        if(dialogueText.GetComponent<EnhancedText>() != null) //Eğer EnhancedText bileşeni varsa şu anki textin wobble indekslerini ayarla
        {
            dialogueText.GetComponent<EnhancedText>().isWobbly = false;
            dialogueText.GetComponent<EnhancedText>().SetAllIndices(currentSentence);
        }

        StartCoroutine(TypeSentence(currentSentence));

        if (sentence.audioClip != null)
        {
            audioSource.clip = sentence.audioClip;
            audioSource.Play();
            StartCoroutine(DisplaySentenceWithDelay(currentSentence.Length * typeSpeed + 2f));
        }
        else
        {
            StartCoroutine(DisplaySentenceWithDelay(currentSentence.Length * typeSpeed + 2f)); 
        }
    }


    private IEnumerator TypeSentence(string sentence)
    {
        HideOptions();
        isTyping = true;

        
        if(dialogueText.GetComponent<EnhancedText>() != null) //Eğer EnhancedText bileşeni varsa şu anki textin wobble indekslerini ayarla
        {
            dialogueText.GetComponent<EnhancedText>().isWobbly = false;
            dialogueText.GetComponent<EnhancedText>().SetAllIndices(currentSentence);
        }

        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            if(dialogueText.GetComponent<EnhancedText>() != null) // Eğer EnhancedText bileşeni varsa wobble efektini uygula
            {
                dialogueText.GetComponent<EnhancedText>().WooblyUpdate();
            }
            yield return new WaitForSeconds(typeSpeed);
        }

        isTyping = false;

        if(dialogueText.GetComponent<EnhancedText>() != null) // E�er EnhancedText bile�eni varsa wobble efektini aç
        {
            dialogueText.GetComponent<EnhancedText>().isWobbly = true;
        }

        // E�er �u anki c�mle `answers` dizisinin son c�mlesi ise butonlar� g�ster
        if (sentences.Count == 0 && currentDialogue.options.Length > 0)
        {
            DisplayOptions();
        }
    }

    private IEnumerator DisplaySentenceWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!isTyping)
        {
            DisplayNextSentence();
        }
    }

    private void DisplayOptions()
    {
        // �nce t�m d��meleri gizle
        HideOptions();

        // Diyalog se�eneklerini g�ster
        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i < currentDialogue.options.Length)
            {
                optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentDialogue.options[i].question;
                optionButtons[i].gameObject.SetActive(true);
                int optionIndex = i;
                optionButtons[i].onClick.RemoveAllListeners(); // �nceki dinleyicileri temizle
                optionButtons[i].onClick.AddListener(() => OnOptionSelected(optionIndex));
            }
            else
            {
                optionButtons[i].gameObject.SetActive(false);
            }
        }

        // "Goodbye" butonunu g�ster ve t�klama dinleyicisi ekle
        if (currentDialogue != null)
        {
            Button goodbyeButton = optionButtons[optionButtons.Length - 1]; // Son buton "Goodbye" olarak kabul edelim
            goodbyeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Goodbye";
            goodbyeButton.gameObject.SetActive(true);
            goodbyeButton.onClick.RemoveAllListeners(); // �nceki dinleyicileri temizle
            goodbyeButton.onClick.AddListener(() => EndDialogue());
        }
    }


    private void HideOptions()
    {
        for (int i = 0; i < optionButtons.Length; i++)
        {
            optionButtons[i].gameObject.SetActive(false);
        }
    }


    private void OnOptionSelected(int index)
    {
        DialogueOption selectedOption = currentDialogue.options[index];
        sentences.Clear(); // �nceki c�mleleri temizle

        // Her cevab� kuyru�a ekleyelim
        foreach (DialogueSentence answer in selectedOption.answers)
        {
            sentences.Enqueue(answer);
            
        }

        DisplayNextSentence(); // �lk cevab� g�ster
    }

    public void EndDialogue()
    {
        GameManager.Instance.playerController.lockControls = false;
        currentDialogue = null;
        npcNameText.text = "";
        dialogueText.text = "";
        sentences.Clear();
        HideOptions(); // Se�enekleri gizle

        // Opsiyon dinleyicilerini temizle
        foreach (Button button in optionButtons)
        {
            button.onClick.RemoveAllListeners();
        }

        isDialogueActive = false; // Unset the flag to indicate no active dialogue

    }

}
