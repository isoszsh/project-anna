using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionController : MonoBehaviour
{
    public GameObject gasPrefab;

    public AudioSource potionAus;
    public AudioClip potionClip;

    private bool exploded = false;

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < 3.25f && !exploded)
        {
            exploded = true;
            potionAus.PlayOneShot(potionClip);
        }
    }

}
