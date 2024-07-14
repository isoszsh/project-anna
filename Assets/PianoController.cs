using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoController : Event
{

    public SpiderController spider;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.playerController.currentEvent = this;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if (GameManager.Instance.playerController.currentEvent != null && GameManager.Instance.playerController.currentEvent == this)
            {
                GameManager.Instance.playerController.currentEvent = null;
            }
        }      
    }


    public override void TriggerEvent()
    {
        spider.Out();
        this.enabled = false;
    }

}
