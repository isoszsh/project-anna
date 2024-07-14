using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerController : MonoBehaviour
{

    public float waveTime;
    public float waveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,waveTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale += Vector3.one * waveSpeed * Time.deltaTime;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<SoundReflector>())
        {
            StartCoroutine(other.gameObject.GetComponent<SoundReflector>().Reflect());
        }
    }
}
