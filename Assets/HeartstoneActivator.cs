using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartstoneActivator : Event
{

    public Image heartstoneDCImage;

    public Sprite desiredSprite;

    private Sprite storedSprite;

    public ActivatedObject activatedObject;

    public GameObject hsObj;

    public GameObject hsDC;

    public Camera hsCam;
    public Camera playerCam;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.Instance.playerController.pickedItem.GetComponent<PickUpItem>().type == "Heartstone")
        {
            storedSprite = heartstoneDCImage.sprite;
            heartstoneDCImage.sprite = desiredSprite;
            GameManager.Instance.playerController.currentEvent = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.Instance.playerController.pickedItem.GetComponent<PickUpItem>().type == "Heartstone")
        {
            heartstoneDCImage.sprite = storedSprite;

            GameManager.Instance.playerController.currentEvent = null;
        }
    }


    public override void TriggerEvent()
    {
        StartCoroutine(EarthQuake());
        
    }

    IEnumerator EarthQuake()
    {
        GameManager.Instance.playerController.lockControls = true;
        GetComponent<SphereCollider>().enabled = false;
        playerCam.gameObject.SetActive(false);
        hsCam.gameObject.SetActive(true);
        GameManager.Instance.playerController.playerAnimator.SetTrigger("Paint");
        hsDC.SetActive(false);
        yield return new WaitForSeconds(1);
        hsObj.SetActive(true);
        GameManager.Instance.playerController.RemovePickupItem();
        yield return new WaitForSeconds(1);
        hsCam.gameObject.SetActive(false);
        playerCam.gameObject.SetActive(true);
        activatedObject.Activate();
    }

}
