using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneController : MonoBehaviour
{

    private bool isImpact = false;
    public AudioSource aus;
    public AudioClip clip;
    private void OnCollisionEnter(Collision collision)
    {
        if(!isImpact)
        {
            aus.PlayOneShot(clip);
            isImpact = true;
        }
    }
}
