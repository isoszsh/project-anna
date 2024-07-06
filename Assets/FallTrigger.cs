using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTrigger : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController pc = other.GetComponent<PlayerController>();

            pc.transform.position = pc.lastCheckPoint.transform.position;
            pc.playerAnimator.SetTrigger("WakeUp");
            
        }
    }
}
