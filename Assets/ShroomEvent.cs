using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShroomEvent : Event
{

    public Camera playerCam;
    public Camera flowersCam;

    public Camera plantZoneCam;
    public GameObject plantDC;
    public GameObject plantZone;

    public PickUpItem fl1;
    public PickUpItem fl2;
    public PickUpItem fl3;
    // Start is called before the first frame update
    public override void TriggerStartEvent()
    {
        StartCoroutine(Shroom());

    }


    IEnumerator Shroom()
    {
        playerCam.gameObject.SetActive(false);
        flowersCam.gameObject.SetActive(true);

        yield return new WaitForSeconds(1);
        plantDC.SetActive(true);
        yield return new WaitForSeconds(1);
        plantDC.SetActive(false);
        flowersCam.gameObject.SetActive(false);
        plantZoneCam.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        plantZone.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        plantZoneCam.gameObject.SetActive(false);
        playerCam.gameObject.SetActive(true);
        fl1.GetComponent<BoxCollider>().enabled = true;
        fl2.GetComponent<BoxCollider>().enabled = true;
        fl3.GetComponent<BoxCollider>().enabled = true;
        Destroy(gameObject);
    }
}
