using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerSprite : MonoBehaviour
{
    [Header("Speed")]
    public float speed;

    [Header("destroy time")]
    public float delay_destroy_time;

    // Start is called before the first frame update
    void Start()
    {
        destroy_object();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vectorMesh = this.transform.localScale;
        float growing = this.speed * Time.deltaTime;
        this.transform.localScale = new Vector3(vectorMesh.x + growing, vectorMesh.y + growing, vectorMesh.z + growing);
    }

    private void destroy_object()
    {
        Destroy(this.gameObject, this.delay_destroy_time);
    }
}
