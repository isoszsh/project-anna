using UnityEngine;

public class FireLightFlicker : MonoBehaviour
{
    public Light fireLight; // Ateþ ýþýðý
    public float minIntensity = 0.8f; // Minimum yoðunluk
    public float maxIntensity = 1.2f; // Maksimum yoðunluk
    public float flickerSpeed = 0.1f; // Titreme hýzý

    void Update()
    {
        fireLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PerlinNoise(Time.time * flickerSpeed, 0f));
    }
}
