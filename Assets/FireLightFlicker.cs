using UnityEngine;

public class FireLightFlicker : MonoBehaviour
{
    public Light fireLight; // Ate� �����
    public float minIntensity = 0.8f; // Minimum yo�unluk
    public float maxIntensity = 1.2f; // Maksimum yo�unluk
    public float flickerSpeed = 0.1f; // Titreme h�z�

    void Update()
    {
        fireLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PerlinNoise(Time.time * flickerSpeed, 0f));
    }
}
