using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusicBoss : MonoBehaviour
{
    public AudioClip runAudioClip; 
    public AudioClip hitAudioClip;  
    public AudioClip openChestClip;
    private AudioSource audioSource;
    private AudioSource brustAudioSource;

    private AudioSource spawnAudioSource;
    public AudioClip brustAudioClip;
    public AudioClip toLaughAudioClip;
    public AudioClip grumbleAudioClip;
    public AudioClip purpleEffectAudioClip;
    public AudioClip openingChestAudioClip;
    public AudioClip closingChestAudioClip;

    public AudioClip stoneWallClip;

    public AudioClip spawnAudioClip;

    public AudioClip fireAudioClip;

    public AudioClip toyAudioClip;

    public AudioClip popAudioClip;

    public AudioClip blowAudioClip;


    void Start()
    {
        // AudioSource bileşenlerini al
        audioSource = GetComponent<AudioSource>();
        
        // İkinci AudioSource bileşenini oluştur ve ekle
        brustAudioSource = gameObject.AddComponent<AudioSource>();

        // Üçüncü AudioSource bileşenini oluştur ve ekle
        spawnAudioSource = gameObject.AddComponent<AudioSource>();
    }

    public void RunStepMusic(int i)
    {
        if (audioSource != null && runAudioClip != null)
        {
            audioSource.Stop();
            audioSource.clip = runAudioClip;
            audioSource.volume = 0.1f;
            if (i == 1 || i == 2)
            {
                audioSource.Play();
            }
            else
            {
                Debug.LogError("RunStepMusic: Invalid argument value!");
            }
        }
        else
        {
            Debug.LogError("AudioSource or runAudioClip is not properly assigned!");
        }
    }

    public void HitEvent()
    {
        if (audioSource != null && hitAudioClip != null)
        {
            audioSource.Stop();
            audioSource.clip = hitAudioClip;
            audioSource.volume = 0.1f;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("AudioSource or hitAudioClip is not properly assigned!");
        }
    }

    public void OpenChestMusic()
    {
        if (audioSource != null && openChestClip != null)
        {
            audioSource.Stop();
            audioSource.clip = openChestClip;
            audioSource.volume = 0.1f;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("AudioSource or openChestClip is not properly assigned!");
        }
    }

    public void BrustMusic()
    {
        if (brustAudioSource != null && brustAudioClip != null)
        {
            brustAudioSource.clip = brustAudioClip;
            brustAudioSource.volume = 0.1f;
            brustAudioSource.Play();
        }
        else
        {
            Debug.LogError("BrustAudioSource or brustAudioClip is not properly assigned!");
        }
    }

    public void ToLaughMusic()
    {
        if (audioSource != null && toLaughAudioClip != null)
        {
            audioSource.Stop();
            audioSource.clip = toLaughAudioClip;
            audioSource.volume = 0.1f;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("AudioSource or toLaughAudioClip is not properly assigned!");
        }
    }

    public void GrumbleMusic()
    {
        if (audioSource != null && grumbleAudioClip != null)
        {
            audioSource.Stop();
            audioSource.clip = grumbleAudioClip;
            audioSource.volume = 0.1f;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("AudioSource or grumbleAudioClip is not properly assigned!");
        }
    }

    public void PurpleEffectMusic()
    {
        if (audioSource != null && purpleEffectAudioClip != null)
        {
            audioSource.Stop();
            audioSource.clip = purpleEffectAudioClip;
            audioSource.volume = 0.1f;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("AudioSource or purpleEffectAudioClip is not properly assigned!");
        }
    }

    public void OpeningChestMusic()
    {
        if (audioSource != null && openingChestAudioClip != null)
        {
            audioSource.Stop();
            audioSource.clip = openingChestAudioClip;
            audioSource.volume = 0.1f;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("AudioSource or openingChestAudioClip is not properly assigned!");
        }
    }

    public void ClosingChestMusic()
    {
        if (audioSource != null && closingChestAudioClip != null)
        {
            audioSource.Stop();
            audioSource.clip = closingChestAudioClip;
            audioSource.volume = 0.1f;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("AudioSource or closingChestAudioClip is not properly assigned!");
        }
    }

    public void SpawnMusic()
    {
        if (spawnAudioSource != null && spawnAudioClip != null)
        {
            spawnAudioSource.Stop();
            spawnAudioSource.clip = spawnAudioClip;
            spawnAudioSource.volume = 0.03f;
            spawnAudioSource.Play();
        }
        else
        {
            Debug.LogError("SpawnAudioSource or spawnAudioClip is not properly assigned!");
        }
    }

    public void StoneWallMusic()
    {
        if (audioSource != null && stoneWallClip != null)
        {
            audioSource.Stop();
            audioSource.clip = stoneWallClip;
            audioSource.volume = 0.1f;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("AudioSource or stoneWallClip is not properly assigned!");
        }
    }

    public void FireMusic()
    {
        if (audioSource != null && fireAudioClip != null)
        {
            audioSource.Stop();
            audioSource.clip = fireAudioClip;
            audioSource.volume = 0.1f;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("AudioSource or fireAudioClip is not properly assigned!");
        }
    }

    public void ToyMusic()
    {
        if (audioSource != null && toyAudioClip != null)
        {
            audioSource.Stop();
            audioSource.clip = toyAudioClip;
            audioSource.volume = 0.1f;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("AudioSource or toyAudioClip is not properly assigned!");
        }
    }

    public void PopMusic()
    {
        if (audioSource != null && popAudioClip != null)
        {
            audioSource.Stop();
            audioSource.clip = popAudioClip;
            audioSource.volume = 0.1f;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("AudioSource or popAudioClip is not properly assigned!");
        }
    }

    public void BlowMusic()
    {
        if (audioSource != null && blowAudioClip != null)
        {
            audioSource.Stop();
            audioSource.clip = blowAudioClip;
            audioSource.volume = 0.1f;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("AudioSource or blowAudioClip is not properly assigned!");
        }
    }
}
