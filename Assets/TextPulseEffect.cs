using System.Collections;
using UnityEngine;
using TMPro;

public class TextPulseEffect : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public float pulseSpeed = 1.0f; // Pulse hýzýný ayarlar
    public float minAlpha = 0.3f; // Yazýnýn minimum görünürlüðü
    public float maxAlpha = 1.0f; // Yazýnýn maksimum görünürlüðü

    private Color originalColor;

    void Start()
    {
        // Orijinal metin rengini kaydet
        originalColor = textMeshPro.color;
        StartCoroutine(PulseText());
    }

    IEnumerator PulseText()
    {
        while (true)
        {
            // Zamanla alfa deðerini arttýr ve azalt
            float alpha = Mathf.PingPong(Time.time * pulseSpeed, maxAlpha - minAlpha) + minAlpha;
            Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            textMeshPro.color = newColor;

            yield return null; // Bir sonraki frame'e geç
        }
    }
}
