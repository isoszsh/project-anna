using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BoatController : Event
{

    public GameObject boatDc;

    public Image triggerHeadImage;

    public Sprite desiredSprite;

    private Sprite storedSprite;

    public Transform annaPos;
    public Transform endPos;


    public Camera playerCamera;
    public Camera heartstoneCamera;

    public GameObject glassDc;

    public AudioClip glassClip;
    public AudioSource glassAus;
    public GameObject glassContainer;

    public DialogueStarter oopsieDS;
    public GameObject oopsieDC;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.Instance.playerController.pickedItem.GetComponent<PickUpItem>().type == "Heartstone")
        {
            storedSprite = triggerHeadImage.sprite;
            triggerHeadImage.sprite = desiredSprite;
            GameManager.Instance.playerController.currentEvent = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.Instance.playerController.pickedItem.GetComponent<PickUpItem>().type == "Heartstone")
        {
            triggerHeadImage.sprite = storedSprite;

            GameManager.Instance.playerController.currentEvent = null;
        }
    }


    public override void TriggerEvent()
    {
        StartCoroutine(RideBoat());
    }

    public override void TriggerStartEvent()
    {
       this.GetComponent<SphereCollider>().enabled = true;
        boatDc.SetActive(true);

        StartCoroutine(ShowHeartStone());
       
    }

    IEnumerator ShowHeartStone()
    {
        playerCamera.gameObject.SetActive(false);
        heartstoneCamera.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        glassAus.PlayOneShot(glassClip);
        glassContainer.gameObject.SetActive(false);
        glassDc.SetActive(true);
        yield return new WaitForSeconds(1f);
        glassDc.SetActive(false);
        heartstoneCamera.gameObject.SetActive(false);
        playerCamera.gameObject.SetActive(true);
        oopsieDS.readyToTalk = false;
        oopsieDC.SetActive(false);
        GameManager.Instance.playerController.currentEvent = null;

    }


    IEnumerator RideBoat()
    {
        boatDc.SetActive(false);
        GetComponent<Animator>().SetTrigger("Ride");

        GameManager.Instance.playerController.lockControls = true;
        yield return new WaitForSeconds(2f);
        GameManager.Instance.playerController.transform.parent = transform;
        GameManager.Instance.playerController.GetComponent<Rigidbody>().isKinematic = true;
        GameManager.Instance.playerController.transform.position = annaPos.position;
        GameManager.Instance.playerController.transform.rotation = annaPos.rotation;

        yield return new WaitForSeconds(2.5f);
        GameManager.Instance.playerController.transform.position = endPos.position;
        GameManager.Instance.playerController.transform.rotation = endPos.rotation;
        GameManager.Instance.playerController.transform.parent = null;
        GameManager.Instance.playerController.GetComponent<Rigidbody>().isKinematic = false;
        GameManager.Instance.playerController.lockControls = false;


    }
}
