using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{

    public string type;
    // Start is called before the first frame update


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController pc = other.GetComponent<PlayerController>();
            if (!pc.pickedItem)
            {
                if(this.GetComponent<SoundReflector>())
                {
                    if(this.GetComponent <SoundReflector>().readyToPick )
                    {
                        pc.willPick = gameObject;
                    }
                }
                else
                {
                    pc.willPick = gameObject;
                }
               
            }

        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController pc = other.GetComponent<PlayerController>();
            pc.willPick = null;

        }
    }
}
