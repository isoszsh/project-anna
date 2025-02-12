using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogueStarter : Event
{
    public DialogueData dialogueData;

    public Sprite storedSprite;


    public Sprite afterDialogueSprite;
    public Image characterHeadImage;

    public GameObject dialogueCanvas;

    public bool readyToTalk;

    public Event relatedEvent;

    public DialogueData firstFind;
    public DialogueData secondFind;
    public int findIndex = 0;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && readyToTalk)
        {
            storedSprite = characterHeadImage.sprite;
            characterHeadImage.sprite = DialogueManager.Instance.dialogueSprite;
            DialogueManager.Instance.audioSource = GetComponent<AudioSource>();
            GameManager.Instance.playerController.currentEvent = this;
        }
        
    }


    public override void TriggerEndEvent()
    {
        if(relatedEvent != null)
        {
            relatedEvent.TriggerStartEvent();
        }
    }

    public override void TriggerEvent()
    {
        if (!DialogueManager.Instance.isDialogueActive)
        {
            GameManager.Instance.GetComponent<DialogueManager>().SetNpc(this.gameObject); // Bu satır kamera açısı veya npc nin hareketlerini npc üstünden yönlendirmek için yazdığım bir satır.

            if(GameManager.Instance.playerController.pickedItem != null)
            {
                if (GameManager.Instance.playerController.pickedItem.GetComponent<ExtraData>())
                {
                    ExtraData ed = GameManager.Instance.playerController.pickedItem.GetComponent<ExtraData>();

                    if(ed.isCorrect)
                    {
                        if(findIndex == 0)
                        {
                            DialogueManager.Instance.StartDialogue(firstFind.dialogue);
                            findIndex++;
                        }
                        else if (findIndex == 1)
                        {
                            findIndex++;
                            relatedEvent.TriggerStartEvent();
                        }
                        GameManager.Instance.playerController.RemovePickupItem();
                    }
                    else
                    {
                        DialogueManager.Instance.StartDialogue(ed.DialogueData.dialogue);
                    }
                }
                else
                {
                    DialogueManager.Instance.StartDialogue(dialogueData.dialogue);
                }
            }
            else
            {
                DialogueManager.Instance.StartDialogue(dialogueData.dialogue);
            }
            
           
            dialogueCanvas.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && readyToTalk)
        {
            if (GameManager.Instance.playerController.currentEvent != null && GameManager.Instance.playerController.currentEvent == this)
            {
                if (afterDialogueSprite != null)
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

                GameManager.Instance.playerController.currentEvent = null;
            }
        }   
    }
}
