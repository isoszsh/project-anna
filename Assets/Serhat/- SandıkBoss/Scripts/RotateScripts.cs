using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScripts : MonoBehaviour
{
    public bool isRight = true;
    public float speed = 2;

    public void Update()
    {
        transform.Rotate(Vector3.up * speed * Time.deltaTime);transform.Rotate(Vector3.up * speed * Time.deltaTime);
    }
}
