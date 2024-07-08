using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogueStarter : MonoBehaviour
{
    public DialogueData dialogueData;

    public Sprite storedSprite;

    public Image characterHeadImage;

    public GameObject dialogueCanvas;

    public bool readyToTalk;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            storedSprite = characterHeadImage.sprite;
            characterHeadImage.sprite = DialogueManager.Instance.dialogueSprite;
            DialogueManager.Instance.audioSource = GetComponent<AudioSource>();
        }
        
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F) && !DialogueManager.Instance.isDialogueActive)
            {
                DialogueManager.Instance.StartDialogue(dialogueData.dialogue);
                dialogueCanvas.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            characterHeadImage.sprite = storedSprite;
            storedSprite = null;
            DialogueManager.Instance.audioSource = null;
            dialogueCanvas.SetActive(true);
        }
        
    }
}
