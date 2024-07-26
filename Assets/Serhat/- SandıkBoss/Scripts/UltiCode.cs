using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltiCode : MonoBehaviour
{
    public float moveSpeed = 5f; // Hareket hızı

    // Doğduktan sonra 2 saniye bekle ardından x yönüne doğru hareket et
    public void Start()
    {
        StartCoroutine(MoveAfterDelay());
    }

    private IEnumerator MoveAfterDelay()
    {
        //ilk iki saniyede localSize'ı 0 dan başlıyarak büyüt ve 1 e getir
        float elapsedTime = 0;
        Vector3 startingScale = transform.localScale * 0;
        Vector3 targetScale = Vector3.one;

        while (elapsedTime < 2)
        {
            transform.localScale = Vector3.Lerp(startingScale, targetScale, (elapsedTime / 2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(2); // 2 saniye bekle
        while (true) // Sürekli hareket et
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime); // X yönünde hareket
            yield return null; // Bir sonraki frame'e kadar bekle
        }
    }
}
