using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseShrinkController : MonoBehaviour
{
   

    public bool shrinking = false;
    public GameObject house;
    public GameObject[] objects;
    public Camera houseCamera;
    public Camera mainCamera;
  



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this.GetComponent<BoxCollider>().enabled = false;
            
            StartCoroutine(Vanish());
        }
    }

    IEnumerator Vanish()
    {

        mainCamera.gameObject.SetActive(false);
        houseCamera.gameObject.SetActive(true);

        foreach (GameObject obj in objects)
        {
            GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip);
            yield return new WaitForSeconds(.05f);
            Destroy(obj);
            
        }

        
        yield return new WaitForSeconds(1f);
        Destroy(houseCamera.gameObject);
        mainCamera.gameObject.SetActive(true);
        Destroy(house);
        
        Destroy(gameObject);
    }
}
