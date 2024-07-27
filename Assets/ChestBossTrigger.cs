using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBossTrigger : MonoBehaviour
{
    public GameObject outDoorWall;

    // Update is called once per frame
    void Update()
    {
        // eğer j harfine basılır sa
        if (Input.GetKeyDown(KeyCode.J))
        {
            outDoorWall.SetActive(true);
            this.GetComponent<ChestBossStateMachine>().Initialise();
        }
    }
}
