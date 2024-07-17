using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AncientStoneController : Event
{


    private void Start()
    {
        GetComponent<MeshRenderer>().materials[2].color = Color.white;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.Instance.playerController.pickedItem)
        {
            string itemType = GameManager.Instance.playerController.pickedItem.GetComponent<PickUpItem>().type;
            if (itemType == "Brush")
            {
                GameManager.Instance.playerController.currentEvent = this;
            }

        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance.playerController.currentEvent != null && GameManager.Instance.playerController.currentEvent == this)
            {
                GameManager.Instance.playerController.currentEvent = null;
            }
        }
    }


    public override void TriggerEvent()
    {
        StartCoroutine(PaintStone());
    }

    IEnumerator PaintStone()
    {
        GameManager.Instance.playerController.lockControls = true;
        GameManager.Instance.playerController.playerAnimator.SetTrigger("Paint");
        yield return new WaitForSeconds(2);
        GetComponent<MeshRenderer>().materials[2].color = GameManager.Instance.playerController.pickedItem.GetComponent<BrushController>().currentColor;
        yield return new WaitForSeconds(1);
        GameManager.Instance.playerController.lockControls = false;
    }

}
