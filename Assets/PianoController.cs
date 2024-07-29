using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PianoController : Event
{

    public GameObject pianoDC;

    public Image dcImg;
    public Sprite desiredSprite;

    private Sprite storedSprite;

    public SpiderController spider;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            storedSprite = dcImg.sprite;
            dcImg.sprite = desiredSprite;
            GameManager.Instance.playerController.currentEvent = this;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if (GameManager.Instance.playerController.currentEvent != null && GameManager.Instance.playerController.currentEvent == this)
            {
                dcImg.sprite = storedSprite;
                GameManager.Instance.playerController.currentEvent = null;
            }
        }      
    }


    public override void TriggerEvent()
    {
        pianoDC.SetActive(false);
        spider.GetSound();
        this.enabled = false;
    }

}
