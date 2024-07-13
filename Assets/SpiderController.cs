using System.Collections;
using System.Collections.Generic;
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

    public AudioClip SpiderClip;
    public AudioClip annaMotherClip;
    public Texture annaFaceTexture;
    public Animator capsuleAnimator;
    public AudioSource DialoguesAus;
    public Animator notesAnimator;
    public NoteLoader notesLoader;

    

    public void Out()
    {
        animator.SetTrigger("Out");
        aus.Play();
        StartCoroutine(SpiderSequence());
    }



    IEnumerator SpiderSequence()
    {
        GameManager.Instance.playerController.lockControls = true;
        gameCam.gameObject.SetActive(false);
        spiderCam.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        DialoguesAus.PlayOneShot(SpiderClip);
        yield return new WaitForSeconds(4f);
        spiderCam.gameObject.SetActive(false );
        AnnaCam.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        animator.SetBool("Walk",true);
        yield return new WaitForSeconds(.5f);
        capsuleAnimator.SetTrigger("CapsuleMove");
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
        gameCam.gameObject.SetActive(true);
        spiderCam.gameObject.SetActive(false);
        AnnaCam.gameObject.SetActive(false);
        aus.Stop();
        notesLoader.StartFight(); 
    }
}
