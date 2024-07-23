using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTrailScript : MonoBehaviour
{
    public void Start()
    {
        // doğduğun zmaan particle effecti başlasın
        GetComponent<ParticleSystem>().Play();
        // particle effecti bitince kendini imha etsin
        Destroy(gameObject, GetComponent<ParticleSystem>().main.duration * 2);
    }

}
