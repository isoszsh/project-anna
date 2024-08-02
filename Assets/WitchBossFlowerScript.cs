using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchBossFlowerScript : MonoBehaviour
{
    public GameObject VaseController;
    public GameObject handleFlower;
    public GameObject vaseFlower;
    
     private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PickUpItem>() != null)
        {
            if (other.GetComponent<PickUpItem>().type == "Plant")
            {
                handleFlower = other.gameObject;
                PlantTheFlower();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<PickUpItem>() != null)
        {
            if (other.GetComponent<PickUpItem>().type == "Plant")
            {
                handleFlower = other.gameObject;
                PlantTheFlower();
            }
        }
    }

    public void PlantTheFlower()
    {
        this.GetComponent<MeshRenderer>().enabled = false;
        handleFlower.SetActive(false);
        vaseFlower.SetActive(true);
        VaseController.GetComponent<VaseControllerScript>().FlowerCountMinus(); 
    }
}
