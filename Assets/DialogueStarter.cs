using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogueStarter : MonoBehaviour
{
    public DialogueData dialogueData;

    public Sprite storedSprite;


    public Sprite afterDialogueSprite;
    public Image characterHeadImage;

    public GameObject dialogueCanvas;

    public bool readyToTalk;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && readyToTalk)
        {
            storedSprite = characterHeadImage.sprite;
            characterHeadImage.sprite = DialogueManager.Instance.dialogueSprite;
            DialogueManager.Instance.audioSource = GetComponent<AudioSource>();
        }
        
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && readyToTalk)
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
        if (other.CompareTag("Player") && readyToTalk)
        {
            
            if(afterDialogueSprite != null)
            {
                characterHeadImage.sprite = afterDialogueSprite;
            }
            else
            {
                characterHeadImage.sprite = storedSprite;
            }
            storedSprite = null;
            DialogueManager.Instance.audioSource = null;
            dialogueCanvas.SetActive(true);
        }
        
    }
}
