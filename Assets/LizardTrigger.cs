using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LizardTrigger : MonoBehaviour
{

    public Animator lizardAnimator;
    public AudioSource lizardAudioSource;
    public Transform lizardNewPosition;
    public GameObject dialogueCanvas;


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            lizardAnimator.transform.position = lizardNewPosition.position;
            lizardAnimator.transform.rotation = lizardNewPosition.rotation;
            lizardAnimator.GetComponent<DialogueStarter>().readyToTalk = true;
            dialogueCanvas.SetActive(true);
            lizardAudioSource.Play();
            lizardAnimator.SetTrigger("Ready");
            Destroy(this.gameObject);
        }
    }
   
}
