using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggerShip : MonoBehaviour
{
    public GameObject fallingRock;

    // Update is called once per frame
    void Update()
    {
        
    }

    // ontirgger enter ile player gelirse
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //rigibody sinin use gravity sini true yap
            fallingRock.GetComponent<Rigidbody>().useGravity = true;
        }
    }
}
