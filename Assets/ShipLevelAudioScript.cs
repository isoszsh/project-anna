using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipLevelAudioScript : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioSource stepAudioSource;

    public AudioSource angryAudioSource;

    public AudioClip stepAudioClip;
    public AudioClip angryAudioClip;


    void Start()
    {
        // AudioSource bileşenlerini al
        audioSource = GetComponent<AudioSource>();
        
        // İkinci AudioSource bileşenini oluştur ve ekle
        stepAudioSource = audioSource;

        // Üçüncü AudioSource bileşenini oluştur ve ekle
        angryAudioSource = audioSource;
    }

    public void StepMusic()
    {
        if (stepAudioSource != null && stepAudioClip != null)
        {
            stepAudioSource.Stop();
            stepAudioSource.clip = stepAudioClip;
            stepAudioSource.Play();
        }
        else
        {
            Debug.LogError("AudioSource or grumbleAudioClip is not properly assigned!");
        }
    }

    public void AngryMusic()
    {
        if (angryAudioSource != null && angryAudioClip != null)
        {
            angryAudioSource.Stop();
            angryAudioSource.clip = angryAudioClip;
            angryAudioSource.Play();
        }
        else
        {
            Debug.LogError("AudioSource or grumbleAudioClip is not properly assigned!");
        }
    }
}
