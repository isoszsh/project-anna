using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateController : MonoBehaviour
{

    public AudioClip onSFX;
    public AudioClip offSFX;


    private AudioSource aus;
    private MeshRenderer mR;

    private bool isActive = false;


    private void Start()
    {
        aus = GetComponent<AudioSource>();
        mR = GetComponent<MeshRenderer>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Rigidbody>() != null && !isActive)
        {
            Activate();
        }
    }


    private void OnTriggerExit(Collider other)
    {
        Deactivate();
    }



    private void Activate()
    {
        mR.material.color = Color.green;
        aus.PlayOneShot(onSFX);
        isActive = true;
    }

    private void Deactivate()
    {
        mR.material.color = Color.red;
        aus.PlayOneShot(offSFX);
        isActive = false;
    }
}
