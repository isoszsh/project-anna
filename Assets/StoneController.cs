using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneController : MonoBehaviour
{

    private int impactCount = 0;
    public AudioSource aus;
    public AudioClip clip;
    public GameObject ripple;
    private void OnCollisionEnter(Collision collision)
    {
        if(impactCount < 1)
        {
            aus.PlayOneShot(clip);
            impactCount ++;
            GameObject RO = Instantiate(ripple,transform.position, Quaternion.identity);
            Destroy(RO, 3);
        }
        else if (impactCount < 3)
        {
            GameObject RO = Instantiate(ripple, transform.position, Quaternion.identity);
            Destroy(RO, 3);
            impactCount++;
        }
       
    }
}
