using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreFallTrigger : MonoBehaviour

    
{

    public Animator TreeAnim;
    public AudioSource treeAus;


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            TreeAnim.SetTrigger("Fall");
            treeAus.Play();
        }
    }
}
