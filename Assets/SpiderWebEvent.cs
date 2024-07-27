using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpiderWebEvent : Event
{

    public GameObject webDC;
    public Image dcImg;
    public Sprite desiredSprite;
    public Camera webCam;
    public Camera playerCam;
    public AudioSource eventAus;
    public AudioClip scissorscut;


    private Sprite storedSprite;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(GameManager.Instance.playerController.pickedItem != null)
            {
                if(GameManager.Instance.playerController.pickedItem.GetComponent<PickUpItem>().type == "Scissors")
                {
                    GameManager.Instance.playerController.currentEvent = this;
                    storedSprite = dcImg.sprite;
                    dcImg.sprite = desiredSprite;
                }
            }


           
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (GameManager.Instance.playerController.pickedItem != null)
            {
                if (GameManager.Instance.playerController.pickedItem.GetComponent<PickUpItem>().type == "Scissors")
                {
                    GameManager.Instance.playerController.currentEvent = null;
                    dcImg.sprite = storedSprite;
                }
            }
        }
    }

    public override void TriggerEvent()
    {
        StartCoroutine(WebCut());
    }


    IEnumerator WebCut()
    {
        webDC.SetActive(false);
        playerCam.gameObject.SetActive(false);
        webCam.gameObject.SetActive(true);
        GameManager.Instance.playerController.playerAnimator.SetTrigger("Paint");
        yield return new WaitForSeconds(1);
        eventAus.PlayOneShot(scissorscut);
        yield return new WaitForSeconds(.5f);
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        gameObject.GetComponent<SphereCollider>().enabled = false;
        yield return new WaitForSeconds(1f);
        GameManager.Instance.playerController.RemovePickupItem();
        playerCam.gameObject.SetActive(true);
        webCam.gameObject.SetActive(false);
        
    }
}
