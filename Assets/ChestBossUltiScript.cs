using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBossUltiScript : MonoBehaviour
{
    // doğruğu anda bir yöe doğru yavaşca hareket etsin 

    public float speed = 1f;
    public float lifeTime = 3f;
    public float distance = 10f;
    public float damage = 10f;
    public GameObject hitEffect;

    void Start()
    {
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
