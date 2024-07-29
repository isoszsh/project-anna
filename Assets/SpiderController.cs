using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpiderController : MonoBehaviour
{

    public Animator animator;
    public AudioSource aus;
    public Camera gameCam;
    public Camera spiderCam;
    public Camera spiderCam2;
    public Camera AnnaCam;
    public Camera NotesCam;
    public Camera fightCam;

    public AudioClip SpiderClip;
    public AudioClip annaMotherClip;
    public Texture annaFaceTexture;
    public Animator capsuleAnimator;
    public AudioSource DialoguesAus;
    public Animator notesAnimator;
    public NoteLoader notesLoader;

    public AudioClip annaHears;

    public GameObject[] rigidbodies;

    public Vector3 explosionPosition; // Patlama noktasý
    public float explosionForce = 10f; // Patlama kuvveti
    public float explosionRadius = 5f; // Patlama yarýçapý
    public float upwardsModifier = 1.0f;

    public AudioClip explosionSFX;


    public Camera spiderLoseCam;
    public AudioClip spiderLoseSFX;
    public GameObject annaLoseCam;
    public AudioClip afterLoseGo;
    public AudioClip afterLoseDont;
    public AudioClip annaVictory;
    public GameObject portal;

    public GameObject annaHearsCam;
    public GameObject annaHearsCam2;

    public GameObject annaDecision;

    public void GetSound()
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();

        // Her bir AudioSource bileþeninin outputAudioMixerGroup özelliðini null yapýn
        foreach (AudioSource audioSource in allAudioSources)
        {
            audioSource.outputAudioMixerGroup = null;
        }
        StartCoroutine(GetSoundIE());
    }


    IEnumerator GetSoundIE()
    {
        GameManager.Instance.playerController.lockControls = true;
        GameManager.Instance.playerController.GetComponent<Rigidbody>().isKinematic = true;
        GameManager.Instance.playerController.transform.position = new Vector3(-26.69323f, 0.008f, -118.303f);
        DialoguesAus.PlayOneShot(annaHears);
        gameCam.gameObject.SetActive(false);
        annaHearsCam.gameObject.SetActive(true);
        annaHearsCam.GetComponent<Animator>().SetTrigger("Hear");
        GameManager.Instance.playerController.playerAnimator.SetBool("PlayingPiano", true);
        yield return new WaitForSeconds(10);
        annaHearsCam.gameObject.SetActive(false );
        annaHearsCam2.gameObject.SetActive(true);
        annaHearsCam2.GetComponent<Animator>().SetTrigger("Hear");
        yield return new WaitForSeconds(10);
        GameManager.Instance.playerController.playerAnimator.SetBool("PlayingPiano", false);
        annaHearsCam.gameObject.SetActive(false);
        
        Out();
    }

    public void Out()
    {
        animator.SetTrigger("Out");
        aus.Play();
        StartCoroutine(SpiderSequence());
    }



    IEnumerator SpiderSequence()
    {
        annaHearsCam2.SetActive(false);
        GameManager.Instance.playerController.lockControls = true;
        gameCam.gameObject.SetActive(false);
        spiderCam.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);
        DialoguesAus.PlayOneShot(SpiderClip);
        yield return new WaitForSeconds(4f);
        spiderCam.gameObject.SetActive(false );
        AnnaCam.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        capsuleAnimator.SetTrigger("CapsuleMove");
        yield return new WaitForSeconds(.5f);
        animator.SetBool("Walk", true);
        spiderCam.gameObject.SetActive(true);
        AnnaCam.gameObject.SetActive(false);
        yield return new WaitForSeconds(5f);
        animator.SetBool("Walk", false);
        notesAnimator.SetTrigger("Notes");
        spiderCam.gameObject.SetActive(false);
        NotesCam.gameObject.SetActive(true);
        yield return new WaitForSeconds(6f);
        NotesCam.gameObject.SetActive(false);
        spiderCam2.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        spiderCam2.gameObject.SetActive(false);
        AnnaCam.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        DialoguesAus.PlayOneShot(annaMotherClip);
        yield return new WaitForSeconds(14.5f);
        GameManager.Instance.playerController.Anger();
        yield return new WaitForSeconds(2f);
        gameCam.gameObject.SetActive(false);
        fightCam.gameObject.SetActive(true );
        spiderCam.gameObject.SetActive(false);
        AnnaCam.gameObject.SetActive(false);
        aus.Stop();
        GameManager.Instance.playerController.GetComponent<Rigidbody>().isKinematic = false;
        GameManager.Instance.playerController.playerAnimator.SetBool("PlayingPiano", true);
        StartCoroutine(ExplosionCounter());
        notesLoader.StartFight();
        yield return new WaitForSeconds(37);
        animator.SetBool("isPlaying", true);
        yield return new WaitForSeconds(23);
        animator.SetBool("isPlaying", false);
        yield return new WaitForSeconds(4.5f);
        animator.SetBool("isPlaying", true);
        yield return new WaitForSeconds(30);
        animator.SetBool("isPlaying", false);


    }


    IEnumerator ExplosionCounter()
    {
        yield return new WaitForSeconds(98);
        foreach (GameObject obj in rigidbodies)
        {
            if (obj.GetComponent<Rigidbody>() == null)
            {
                obj.AddComponent<Rigidbody>();
            }
        }

        Explode();

    }

    void Explode()
    {
        // Patlamanýn etkisiyle her bir Rigidbody'ye kuvvet uygula
        DialoguesAus.PlayOneShot(explosionSFX);
        foreach (GameObject obj in rigidbodies)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, explosionPosition, explosionRadius, upwardsModifier, ForceMode.Impulse);
            }
        }

        StartCoroutine(AfterFight());
    }

    IEnumerator AfterFight()
    {
        GameManager.Instance.playerController.playerAnimator.SetBool("PlayingPiano", false);
        animator.SetTrigger("Lose");
        fightCam.gameObject.SetActive(false);
        spiderLoseCam.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        aus.PlayOneShot(annaVictory);
        DialoguesAus.PlayOneShot(spiderLoseSFX);
        yield return new WaitForSeconds(10);
        AnnaCam.gameObject.SetActive(true);
        spiderLoseCam.gameObject.SetActive(false);
        yield return new WaitForSeconds(5);
        AnnaCam.gameObject.SetActive(false);
        spiderLoseCam.gameObject.SetActive(true);
        yield return new WaitForSeconds(4);
        spiderLoseCam.gameObject.SetActive(false);
        annaLoseCam.gameObject.SetActive(true);
        annaDecision.gameObject.SetActive(true);

    }

    public void LetGo()
    {
        StartCoroutine(LGC());
    }

    IEnumerator LGC()
    {
        annaDecision.gameObject.SetActive(false);
        annaLoseCam.gameObject.SetActive(false);
        spiderLoseCam.gameObject.SetActive(true);
        DialoguesAus.PlayOneShot(afterLoseGo);
        yield return new WaitForSeconds(13);
        spiderLoseCam.gameObject.SetActive(false);
        portal.gameObject.SetActive(true);
        gameCam.gameObject.SetActive(true);
        GameManager.Instance.playerController.lockControls = false;
        transform.parent.gameObject.SetActive(false);


    }

    public void StayHere()
    {
        StartCoroutine(SH());
    }

    IEnumerator SH()
    {
        annaDecision.gameObject.SetActive(false);
        annaLoseCam.gameObject.SetActive(false);
        spiderLoseCam.gameObject.SetActive(true);
        DialoguesAus.PlayOneShot(afterLoseDont);
        yield return new WaitForSeconds(11);
        spiderLoseCam.gameObject.SetActive(false);
        portal.gameObject.SetActive(true);
        gameCam.gameObject.SetActive(true);
        GameManager.Instance.playerController.lockControls = false;
        transform.parent.gameObject.SetActive(false);

    }
}
