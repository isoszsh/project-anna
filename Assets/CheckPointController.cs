using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointController : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
           PlayerController pc = other.GetComponent<PlayerController>();

            pc.lastCheckPoint = transform;
        }
    }
}
