using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateController : MonoBehaviour
{

    public AudioClip onSFX;
    public AudioClip offSFX;


    private AudioSource aus;
    private MeshRenderer mR;

    public bool oneWay;
    public bool timeActivated;
    public float time;

    public ActivatedObject activatedObject;

    private bool isActive = false;
    private float timer;

    public string itemToBeAcceptedType = "Mantar";

    private void Start()
    {
        aus = GetComponent<AudioSource>();
        mR = GetComponent<MeshRenderer>();
    }

    public PickUpItem pickUpItem = null;


    private void OnTriggerStay(Collider other)
    {
        

        if(other.tag == "Mantar"){
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
        mR.material.color = Color.green;
        aus.PlayOneShot(onSFX);
        isActive = true;
        if (activatedObject)
        {
            activatedObject.Activate();
        }
    }

    private void Deactivate()
    {
        mR.material.color = Color.red;
        aus.PlayOneShot(offSFX);
        isActive = false;
    }
}
