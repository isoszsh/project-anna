using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoController : MonoBehaviour
{

    public SpiderController spider;
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        {
            spider.Out();
            this.enabled = false;
        }

        
    }
}
