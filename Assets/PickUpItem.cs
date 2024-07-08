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
                if( !pc.pickedItem && Input.GetKeyDown(KeyCode.F))
                {
                    StartCoroutine(PickMe(pc));
                }
               
            }
        }
    }


    IEnumerator PickMe(PlayerController pc)
    {
        pc.playerAnimator.SetTrigger("Pick");
        pc.lockControls = true;
        yield return new WaitForSeconds(.7f);
        transform.position = pc.pickPoint.position;
        transform.rotation = pc.pickPoint.rotation;
        transform.parent = pc.pickPoint.parent;
        this.GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(2f);
        pc.pickedItem = this.gameObject;
        pc.lockControls = false;

    }
}
