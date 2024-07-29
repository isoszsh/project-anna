using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusicBoss : MonoBehaviour
{
    public AudioClip runAudioClip;  // Ses dosyasını buraya atayın
    public AudioClip hitAudioClip;  // Ses dosyasını buraya atayın
    private AudioSource audioSource;

    void Start()
    {
        // AudioSource bileşenini al
        audioSource = GetComponent<AudioSource>();
    }

    public void NewEvent(int i)
    {
        audioSource.clip = runAudioClip;
        audioSource.volume = 0.1f;
        if (i == 1 || i == 2)
        {
            if (audioSource != null && runAudioClip != null)
            {
                audioSource.Play();
            }
            else
            {
                Debug.LogError("AudioSource or runAudioClip is not properly assigned!");
            }
        }
    }

    public void HitEvent(){
        audioSource.clip = hitAudioClip;
        audioSource.volume = 0.5f;
        if (audioSource != null && runAudioClip != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogError("AudioSource or runAudioClip is not properly assigned!");
        }
    }
}
