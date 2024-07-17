using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAnim : MonoBehaviour
{
    public float speed = 10.0f;

    void Start()
    {
        Rotate90Lerp();
    }

    void Update()
    {
        // hızına göre döndürme işlemi yapar
        //this.transform.Rotate(0, this.speed * Time.deltaTime, 0);
    }

    public void Rotate90Lerp()
    {
        StartCoroutine(Rotate90LerpCoroutine(false));
    }

    IEnumerator Rotate90LerpCoroutine(bool isRight)
    {
        // 90 derece dönmesi için 1 saniye beklet
        float time = 1.0f;
        float elapsedTime = 0.0f;
        Quaternion startRotation = this.transform.rotation;
        Quaternion endRotation;

        if (isRight)
        {
            endRotation = this.transform.rotation * Quaternion.Euler(0, 90, 0);
        }
        else
        {
            endRotation = this.transform.rotation * Quaternion.Euler(0, -90, 0);
        }

        while (elapsedTime < time)
        {
            this.transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        this.transform.rotation = endRotation;

        StartCoroutine(Rotate90LerpCoroutine(isRight));
    }

    
}
