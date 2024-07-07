using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatedObject : MonoBehaviour
{
    public enum ActivationFunction
    {
        None, // E�er se�im yap�lmad�ysa
        Stairs,
        ActivateWithRigidbodyActivation
        // �htiyaca g�re di�er aktivasyon fonksiyonlar� eklenebilir
    }


    // Inspector �zerinden se�ilebilir hale getirilmi� public bir de�i�ken
    public ActivationFunction selectedActivationFunction;

    // Se�ilen fonksiyona g�re nesneyi aktif etmek i�in metot
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
            // �htiyaca g�re di�er case'ler eklenebilir
            default:
                Debug.LogWarning("No valid activation function selected.");
                break;
        }
    }

    // Farkl� aktivasyon fonksiyonlar� i�in �rnek metotlar
    void Stairs()
    {
        Animator animator = GetComponent<Animator>();
        AudioSource audioSource = GetComponent<AudioSource>();
        if (animator != null)
        {
            animator.SetTrigger("Stairs"); // Animator trigger set etmek i�in
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
            rb.isKinematic = false; // Rigidbody'nin aktif olup olmad���n� kontrol etmek i�in
        }
    }
}
