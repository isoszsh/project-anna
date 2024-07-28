using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableScript : MonoBehaviour
{
    public bool isRight = true;
    public float speed = 2;
    public float floatSpeed = 0.5f; // Yükselme ve alçalma hızı
    public float floatAmplitude = 1f; // Yükselme ve alçalma mesafesi
    private float initialY;

    public List<GameObject> particalEffects;

    private void Start()
    {
        initialY = transform.position.y;
    }

    public void Update()
    {
        // Döndürme işlemi
        transform.Rotate(Vector3.up * speed * Time.deltaTime);

        // Yukarı ve aşağı hareket
        Vector3 position = transform.position;
        position.y = initialY + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player collected the object");
            // yok olmadan önce partical effect spawnla
            // transform position un biraz altından spawnla
            var newTransrom = transform.position;
            newTransrom.y -= 0.5f;

            foreach (var particalEffect in particalEffects)
            {
                Instantiate(particalEffect, newTransrom, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}
