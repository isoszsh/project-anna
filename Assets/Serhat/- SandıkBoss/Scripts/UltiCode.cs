using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltiCode : MonoBehaviour
{
    public float moveSpeed = 5f; // Hareket hızı
     public GameObject deathManager;

    // Doğduktan sonra 2 saniye bekle ardından x yönüne doğru hareket et
    public void Start()
    {
        StartCoroutine(MoveAfterDelay());
    }

    //ontrigger enter
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            deathManager = GameObject.Find("ChestDeathManager");
            deathManager.GetComponent<ChestDeathManagerScript>().LoadDeathScene();
        }

    }

    private IEnumerator MoveAfterDelay()
    {
        //ilk iki saniyede localSize'ı 0 dan başlıyarak büyüt ve 1 e getir
        float elapsedTime = 0;
        Vector3 targetScale = transform.localScale;
        Vector3 startingScale = transform.localScale * 0;
        

        while (elapsedTime < 0.5)
        {
            transform.localScale = Vector3.Lerp(startingScale, targetScale, elapsedTime / 0.5f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f); // 2 saniye bekle
        while (true) // Sürekli hareket et
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime); // X yönünde hareket
            yield return null; // Bir sonraki frame'e kadar bekle
        }
    }
}
