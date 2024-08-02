using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewLevelMashine : MonoBehaviour
{
    public GameObject player;
    void Start()
    {
        player.GetComponent<ChestBossTrigger>().DoitPlease();
    }
}
