using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LizardDoorEvent : MonoBehaviour
{

    public GameObject door;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && GameManager.Instance.playerController.pickedItem != null)
        {

            if(GameManager.Instance.playerController.pickedItem.GetComponent<PickUpItem>().type == "LizardKey")
            {
                StartCoroutine(OpenLizardDoor());
            }
            
        }
    }


    IEnumerator OpenLizardDoor()
    {
        GameManager.Instance.playerController.lockControls = true;
        GameManager.Instance.playerController.ResetVelocity();
        GameManager.Instance.playerController.playerAnimator.SetTrigger("Paint");
        yield return new WaitForSeconds(1);
        door.GetComponent<Animator>().enabled = true;
        door.GetComponent<Animator>().SetTrigger("Open");
        GameManager.Instance.playerController.RemovePickupItem();
        yield return new WaitForSeconds(1.5f);
        door.GetComponent<BoxCollider>().enabled = false;
        GameManager.Instance.playerController.lockControls = false;

    }
}
