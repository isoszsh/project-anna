using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReTriggerScript : MonoBehaviour
{
    public GameObject boss;
    public GameObject backgroundMusic;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Again();
        }
    }

    public void Again()
    {
        boss.GetComponent<WitchController>().StartEveryThink();
        backgroundMusic.SetActive(true);
        this.gameObject.SetActive(false);
    }
    
}
