using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderWebEvent : Event
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.playerController.currentEvent = this;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.playerController.currentEvent = null;
        }
    }

    public override void TriggerEvent()
    {
        StartCoroutine(WebCut());
    }


    IEnumerator WebCut()
    {
        yield return new WaitForSeconds(5);
    }
}
