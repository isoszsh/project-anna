using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossStarterEvent : Event
{

    public Image dcImg;
    public Sprite desiredSprite;

    private Sprite storedSprite;

    public ChestBossTrigger cbt;

    public GameObject bossDC;



    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            storedSprite = dcImg.sprite;
            dcImg.sprite = desiredSprite;

            GameManager.Instance.playerController.currentEvent = this;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            dcImg.sprite = storedSprite;
            GameManager.Instance.playerController.currentEvent = null;
        }
    }


    public override void TriggerEvent()
    {
        StartCoroutine(StartBoss());
    }

    IEnumerator StartBoss()
    {
        GameManager.Instance.playerController.lockControls = true;
        GameManager.Instance.playerController.ResetVelocity();
        StartCoroutine(cbt.MakeEveryThing());
        bossDC.SetActive(false);
        yield return new WaitForEndOfFrame();
    }
}
