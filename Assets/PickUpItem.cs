using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{

    public string type;
    // Start is called before the first frame update
    private void OnTriggerStay(Collider other)
    {
        if (other != null)
        {
           if( other.CompareTag("Player") )
           {
                PlayerController pc = other.GetComponent<PlayerController>();
                if(!pc.pickedItem)
                {
                    pc.willPick = gameObject;
                }
               
            }
        }
    }


}
