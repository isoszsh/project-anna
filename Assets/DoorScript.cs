using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public AudioClip onSFX;
    public AudioClip offSFX;


    private AudioSource aus;

    public bool oneWay;
    public bool timeActivated;
    public float time;

    public GameObject activatedObject;

    private bool isActive = false;
    private float timer;

    public string itemToBeAcceptedType = "Key";

    private void Start()
    {
        aus = GetComponent<AudioSource>();
    }

    public PickUpItem pickUpItem = null;


    private void OnTriggerStay(Collider other)
    {
        

        if(other.tag == "Key"){
            pickUpItem = other.GetComponent<PickUpItem>();
        }

        if (pickUpItem != null)
        {
            if(pickUpItem.type == itemToBeAcceptedType)
            {
                isActive = false;
                timeActivated = false;
                itemToBeAcceptedType = " ";
                Activate();
            }
        }

        if (other.GetComponent<Rigidbody>() != null && !isActive && timeActivated)
        {
            if(timer < time) {
                timer += Time.deltaTime;
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if(!oneWay && isActive)
        {
            Deactivate();
        }

        timer = 0;
       
    }



    private void Activate()
    {
        aus.PlayOneShot(onSFX);
        isActive = true;

        Animator animator = activatedObject.GetComponent<Animator>();
        AudioSource audioSource = activatedObject.GetComponent<AudioSource>();
        if (animator != null)
        {
            animator.SetTrigger("OpenDoor"); // Animator trigger set etmek i�in
        }
        if(audioSource != null)
        {
            audioSource.Play();
        }
    }

    private void Deactivate()
    {
        aus.PlayOneShot(offSFX);
        isActive = false;
    }
}