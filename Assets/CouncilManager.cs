﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CouncilManager : MonoBehaviour
{
    public Camera playerCamera;
    public Camera councilCamera;
    public Camera cutsceneCamera;
    public Camera cutsceneCamera2;
    public Camera cutsceneCamera3;

    public CouncilDialogueData dialogueData;
    public CouncilDialogueData acceptDialogue;
    public CouncilDialogueData rejectDialogue;
    private Queue<CouncilDialogueSentence> sentences;
    private CouncilDialogue currentDialogue;
    
    private string currentSentence;
    private AudioSource audioSource;
    public GameObject dialogueText;
    public Button[] optionButtons;
    public TextMeshProUGUI npcNameText;
    private float typeSpeed = 0.04f;
    private bool isTyping = false;
    public GameObject DecisionPanel;
    public GameObject dialoguePanel;
    public GameObject timePanel;


    public Image fadeImage; // UI Image bileşenini burada atayın
    public GameObject achievementText;
    public AudioClip achievementClip;
    public float fadeDuration = 1f;

    private int sentenceIndex;

    public int decision = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CouncilStartCutscene());
        GameManager.Instance.playerController.lockControls = true;
        playerCamera.gameObject.SetActive(false);
        councilCamera.gameObject.SetActive(false);
        cutsceneCamera2.gameObject.SetActive(false);
        cutsceneCamera3.gameObject.SetActive(false);
        sentences = new Queue<CouncilDialogueSentence>();
        audioSource = GetComponent<AudioSource>();
    }

    IEnumerator CouncilStartCutscene()
    {
        yield return new WaitForSeconds(1);
        timePanel.SetActive(false);
        cutsceneCamera.GetComponent<Animator>().SetTrigger("CS");
        yield return new WaitForSeconds(2.3f);
        cutsceneCamera.gameObject.SetActive(false);
        cutsceneCamera2.gameObject.SetActive(true);
        cutsceneCamera2.GetComponent<Animator>().SetTrigger("CS");
        yield return new WaitForSeconds(2.3f);
        cutsceneCamera2.gameObject.SetActive(false);
        cutsceneCamera3.gameObject.SetActive(true);
        cutsceneCamera3.GetComponent<Animator>().SetTrigger("CS");
        yield return new WaitForSeconds(3.3f);
        cutsceneCamera3.gameObject.SetActive(false);
        councilCamera.gameObject.SetActive(true);
        StartDialogue(dialogueData.dialogue);
    }

    public void StartDialogue(CouncilDialogue dialogue)
    {
        sentenceIndex = 0;
        GameManager.Instance.playerController.lockControls = true;
        dialoguePanel.SetActive(true);
        currentDialogue = dialogue;
        sentences.Clear();

        foreach (CouncilDialogueSentence sentence in currentDialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            DisplayOptions();
            return;
        }

        CouncilDialogueSentence sentence = sentences.Dequeue();
        currentSentence = sentence.text;
        StopAllCoroutines();
        SetCouncilCamera(sentence.npcName);
        StartCoroutine(TypeSentence(currentSentence,sentence.npcName));


        if (sentenceIndex == 1)
        {
            GameManager.Instance.playerController.playerAnimator.SetTrigger("Wave");
        }

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


        sentenceIndex++;
    }


    public void SetCouncilCamera(string npc)
    {
        if(npc == "Bear")
        {
            councilCamera.transform.position = new Vector3(-48.59f, 1.1f, -66.9f);
            councilCamera.transform.rotation = Quaternion.Euler(0, 324.767f, 0);
        }
        else if (npc == "Anna")
        {
            councilCamera.transform.position = new Vector3(-47.753f, 0.87f, -67.681f);
            councilCamera.transform.rotation = Quaternion.Euler(0, 500.265f, 0);
        }
        else if (npc == "Fox")
        {
            councilCamera.transform.position = new Vector3(-49.54f, 1.13f, -66.76f);
            councilCamera.transform.rotation = Quaternion.Euler(8.4f, 617.787f, 0);
        }
        else if (npc == "Orca")
        {
            councilCamera.transform.position = new Vector3(-48.05f, 0.71f, -66.72f);
            councilCamera.transform.rotation = Quaternion.Euler(-4.389f, 447.192f, 0);
        }
        else if (npc == "Deer")
        {
            councilCamera.transform.position = new Vector3(-48.888f, 1.392f, -67.298f);
            councilCamera.transform.rotation = Quaternion.Euler(8.504f, 560.825f, 0);
        }
        else if (npc == "Pinguin")
        {
            councilCamera.transform.position = new Vector3(-47.81f, 0.48f, -65.86f);
            councilCamera.transform.rotation = Quaternion.Euler(-1.833f, 385.768f, 0);
        }
    }

    IEnumerator TypeSentence(string sentence,string npcname)
    {
        isTyping = true;
        npcNameText.text = npcname + ":";
        dialogueText.GetComponent<TextMeshProUGUI>().text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.GetComponent<TextMeshProUGUI>().text += letter;
            yield return new WaitForSeconds(typeSpeed);
        }
        isTyping = false;
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
        dialoguePanel.SetActive(false);
        if (decision == 0)
        {
            HideOptions();
            SetCouncilCamera("Anna");
            dialogueText.GetComponent<TextMeshProUGUI>().text = "";
            DecisionPanel.SetActive(true);
        }
        else if (decision == 1)
        {
            StartCoroutine(GoToLevel3_2());
        }
        else if (decision == 2)
        {
            StartCoroutine(GoToLevel4());
            SceneManager.LoadScene("lv3_to_lv4");
        }
        
       
    }

    IEnumerator GoToLevel3_2()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Level3_CouncilAccepted");
    }

    IEnumerator GoToLevel4()
    {
        yield return StartCoroutine(FadeToBlack(fadeDuration));
        yield return new WaitForSeconds(10);
        SceneManager.LoadScene("lv3_to_lv4");

    }

    IEnumerator FadeToBlack(float duration)
    {
        fadeImage.gameObject.SetActive(true) ;
        float elapsedTime = 0f;
        Color startColor = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0f); // Alpha değeri 0
        Color endColor = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1f);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startColor.a, endColor.a, elapsedTime / duration);
            fadeImage.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        fadeImage.color = endColor; // Son rengi garantile

        yield return new WaitForSeconds(2);
        audioSource.PlayOneShot(achievementClip);
        achievementText.SetActive(true);
        

    }

    public void AcceptShip()
    {
        GameManager.Instance.pr.SaveDecision("Level_3", 1);
        decision = 1;
        DecisionPanel.SetActive(false);
        StartDialogue(acceptDialogue.dialogue);
    }

    public void RejectShip()
    {
        GameManager.Instance.pr.SaveDecision("Level_3", 2);
        decision = 2;
        DecisionPanel.SetActive(false);
        StartDialogue(rejectDialogue.dialogue);
    }

    private void HideOptions()
    {
        for (int i = 0; i < optionButtons.Length; i++)
        {
            optionButtons[i].gameObject.SetActive(false);
        }
    }

 

    public void EndDialogue()
    {
        GameManager.Instance.playerController.lockControls = false;
        currentDialogue = null;
        npcNameText.text = "";
        dialogueText.GetComponent<TextMeshProUGUI>().text = "";
        sentences.Clear();
        HideOptions();

        foreach (Button button in optionButtons)
        {
            button.onClick.RemoveAllListeners();
        }

        GameManager.Instance.playerController.currentEvent.TriggerEndEvent();
    }
}
