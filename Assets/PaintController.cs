using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintController : Event
{

    public enum ColorOption
    {
        Yellow,
        Green,
        Red,
        Blue,
        White
    }
    public Color bucketColor;
    public ColorOption selectedColorOption;
    // Start is called before the first frame update


    private void Start()
    {
        switch (selectedColorOption)
        {
            case ColorOption.Yellow:
                bucketColor = Color.yellow;
                break;
            case ColorOption.Green:
                bucketColor = Color.green;
                break;
            case ColorOption.Red:
                bucketColor = Color.red;
                break;
            case ColorOption.Blue:
                bucketColor = Color.blue;
                break;
            case ColorOption.White:
                bucketColor = Color.white;
                break;
            default:
                bucketColor = Color.white; // Varsayýlan olarak beyaz renk
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.Instance.playerController.pickedItem)
        {
            string itemType = GameManager.Instance.playerController.pickedItem.GetComponent<PickUpItem>().type;
            if(itemType == "Brush")
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

        StartCoroutine(PickPaint());
    }


    IEnumerator PickPaint()
    {
        GameManager.Instance.playerController.lockControls = true;
        GameManager.Instance.playerController.playerAnimator.SetTrigger("Paint");
        yield return new WaitForSeconds(2);
        GameManager.Instance.playerController.pickedItem.GetComponent<BrushController>().SetColor(bucketColor);
        yield return new WaitForSeconds(1);
        GameManager.Instance.playerController.lockControls = false;
    }
}
