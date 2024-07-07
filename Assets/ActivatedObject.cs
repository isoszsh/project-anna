using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatedObject : MonoBehaviour
{
    public enum ActivationFunction
    {
        None, // Eðer seçim yapýlmadýysa
        Stairs,
        ActivateWithRigidbodyActivation
        // Ýhtiyaca göre diðer aktivasyon fonksiyonlarý eklenebilir
    }


    // Inspector üzerinden seçilebilir hale getirilmiþ public bir deðiþken
    public ActivationFunction selectedActivationFunction;

    // Seçilen fonksiyona göre nesneyi aktif etmek için metot
    public void Activate()
    {
        switch (selectedActivationFunction)
        {
            case ActivationFunction.Stairs:
                Stairs();
                break;
            case ActivationFunction.ActivateWithRigidbodyActivation:
                ActivateWithRigidbodyActivation();
                break;
            // Ýhtiyaca göre diðer case'ler eklenebilir
            default:
                Debug.LogWarning("No valid activation function selected.");
                break;
        }
    }

    // Farklý aktivasyon fonksiyonlarý için örnek metotlar
    void Stairs()
    {
        Animator animator = GetComponent<Animator>();
        AudioSource audioSource = GetComponent<AudioSource>();
        if (animator != null)
        {
            animator.SetTrigger("Stairs"); // Animator trigger set etmek için
        }
        if(audioSource != null)
        {
            audioSource.Play();
        }

        StartCoroutine(ShakeCamera(5f));
    }


    IEnumerator ShakeCamera(float time)
    {
        CinemachineBasicMultiChannelPerlin noiseProfile = GameManager.Instance.virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noiseProfile.m_FrequencyGain = 2f;
        noiseProfile.m_AmplitudeGain = 2f;
        yield return new WaitForSeconds(11);
        noiseProfile.m_FrequencyGain = .25f;
        noiseProfile.m_AmplitudeGain = .25f;
    }
    void ActivateWithRigidbodyActivation()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false; // Rigidbody'nin aktif olup olmadýðýný kontrol etmek için
        }
    }
}
