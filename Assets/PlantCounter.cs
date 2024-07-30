using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantCounter : MonoBehaviour
{
    public DialogueStarter shroomDS;
    public Image dcImg;
    public Sprite dcSprite;

    public DialogueData plantDD;

    bool planted = false;

    public int totalPlant = 0;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PickUpItem>() != null)
        {
            if (other.GetComponent<PickUpItem>().type == "Plant")
            {
                totalPlant++;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PickUpItem>() != null)
        {
            if (other.GetComponent<PickUpItem>().type == "Plant")
            {
                totalPlant--;
            }
        }
    }


    private void Update()
    {
        if (totalPlant == 3 && !planted) {
            planted = true;
            shroomDS.dialogueData = plantDD;
            dcImg.sprite = dcSprite;
            GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
