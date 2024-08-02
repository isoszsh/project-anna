using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchWeaponHitScript : MonoBehaviour
{
    // player taglı bir obje ile çarpıştığında çalışacak olan fonksiyon

    private GameObject deathManager;
    void Start()
    { 
        deathManager = GameObject.Find("DeathManager");  
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Player hit by witch weapon");
            deathManager.GetComponent<DeathManagerScript>().DeathCircle();
        }
    }
}
