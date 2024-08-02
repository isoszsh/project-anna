using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchMusicManagerScript : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip bossLaughAudioClip;
    public AudioClip fissshAudioClip;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayBossLaugh()
    {
        if (audioSource != null && bossLaughAudioClip != null)
        {
            audioSource.Stop();
            audioSource.clip = bossLaughAudioClip;
            audioSource.volume = 0.1f;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("PlayBossLaugh: AudioSource or AudioClip is null!");
        }
    }

    public void PlayFisssh()
    {
        if (audioSource != null && fissshAudioClip != null)
        {
            audioSource.Stop();
            audioSource.clip = fissshAudioClip;
            audioSource.volume = 0.1f;
            audioSource.Play();
        }
        else 
        {
            Debug.LogError("PlayFisssh: AudioSource or AudioClip is null!");
        }
    }
}
