using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAnim : MonoBehaviour
{
    [Header("Speed")]
    public float speed;

    // Update is called once per frame
    void Update()
    {
        // hızına göre döndürme işlemi yapar
        this.transform.Rotate(0, this.speed * Time.deltaTime, 0);

    }

}
