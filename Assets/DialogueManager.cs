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

    public GameObject npc = null ; // Serhat Ekledi NPC'yi tutmak için

    public void SetNpc(GameObject npc)
    {
        this.npc = npc;
    }

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
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

        isDialogueActive = true; 

        if(npc != null)
        {
            CameraActiveForDialog cameraDialog = npc.GetComponent<CameraActiveForDialog>();
            if(cameraDialog != null){
                cameraDialog.isSpeakerCamActive(true);
            }
            else { Debug.Log("CameraActiveForDialog component is missing on npc."); }
        }
        else { Debug.Log("npc is null, cannot change camera angle."); }

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

        var dialogueEnhancedText = dialogueText.GetComponent<EnhancedText>();
        if(dialogueEnhancedText != null) //Eğer EnhancedText bileşeni varsa şu anki textin wobble indekslerini ayarla
        {
            dialogueText.GetComponent<EnhancedText>().isWobbly = false;
            dialogueText.GetComponent<EnhancedText>().SetAllIndices(currentSentence);
        }

        StartCoroutine(TypeSentence(currentSentence));

        if (sentence.audioClip != null)
        {
            audioSource.clip = sentence.audioClip;
            audioSource.Play();
            StartCoroutine(DisplaySentenceWithDelay(sentence.audioClip.length));
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

        var dialogueEnhancedText = dialogueText.GetComponent<EnhancedText>();
        if(dialogueEnhancedText != null) //Eğer EnhancedText bileşeni varsa şu anki textin wobble indekslerini ayarla
        {
            dialogueText.GetComponent<EnhancedText>().isWobbly = false;
            dialogueText.GetComponent<EnhancedText>().SetAllIndices(currentSentence);
        }

        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;

            dialogueEnhancedText = dialogueText.GetComponent<EnhancedText>();
            if(dialogueEnhancedText != null) // Eğer EnhancedText bileşeni varsa wobble efektini uygula
            {
                dialogueText.GetComponent<EnhancedText>().WooblyUpdate();
            }

            yield return new WaitForSeconds(typeSpeed);
        }

        isTyping = false;

        dialogueEnhancedText = dialogueText.GetComponent<EnhancedText>();
        if(dialogueText.GetComponent<EnhancedText>() != null) // E�er EnhancedText bile�eni varsa wobble efektini aç
        {
            dialogueText.GetComponent<EnhancedText>().isWobbly = true;
        }

       
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
        
        HideOptions();

        
        for (int i = 0; i < optionButtons.Length; i++)
        {
            if(currentDialogue != null)
            {
                if (i < currentDialogue.options.Length)
                {
                    optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentDialogue.options[i].question;
                    optionButtons[i].gameObject.SetActive(true);
                    int optionIndex = i;
                    optionButtons[i].onClick.RemoveAllListeners();
                    optionButtons[i].onClick.AddListener(() => OnOptionSelected(optionIndex));
                }
                else
                {
                    optionButtons[i].gameObject.SetActive(false);
                }
            }
            
        }

        
        if (currentDialogue != null)
        {
            Button goodbyeButton = optionButtons[optionButtons.Length - 1]; 
            goodbyeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Goodbye";
            goodbyeButton.gameObject.SetActive(true);
            goodbyeButton.onClick.RemoveAllListeners();
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
        sentences.Clear(); 

        
        foreach (DialogueSentence answer in selectedOption.answers)
        {
            sentences.Enqueue(answer);
            
        }

        DisplayNextSentence(); 
    }

    public void EndDialogue()
    {

        // //Konuştuğumuz esnada kamera açısı değişen npc için 
        if(npc != null)
        {
            CameraActiveForDialog cameraDialog = npc.GetComponent<CameraActiveForDialog>();
            if(cameraDialog != null){
                cameraDialog.isSpeakerCamActive(false);
                //npc = null;
            }
            else { Debug.Log("CameraActiveForDialog component is missing on npc."); }
        }
        else { Debug.Log("npc is null, cannot change camera angle."); }

        var dialogueEnhancedText = dialogueText.GetComponent<EnhancedText>();
        if(dialogueEnhancedText != null) //Eğer EnhancedText bileşeni varsa şu anki textin wobble indekslerini ayarla
        {
            Debug.Log("EnhancedText component is found.");
            dialogueText.GetComponent<EnhancedText>().ClearAll();
            Debug.Log("EnhancedText component is cleared.");
        }





        GameManager.Instance.playerController.lockControls = false;
        currentDialogue = null;
        npcNameText.text = "";
        dialogueText.text = "";
        sentences.Clear();

        HideOptions(); // Se�enekleri gizle

        audioSource.Stop();

        // Opsiyon dinleyicilerini temizle
        foreach (Button button in optionButtons)
        {
            button.onClick.RemoveAllListeners();
        }

        GameManager.Instance.playerController.currentEvent.TriggerEndEvent();
        isDialogueActive = false;
    }

}
