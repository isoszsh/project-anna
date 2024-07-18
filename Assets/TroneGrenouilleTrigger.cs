using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroneGrenouilleTrigger : MonoBehaviour
{
    public Animator OopsieQueenAnimator;
    public AudioSource OopsieQueenAudioSource;
    public Transform OopsieQueenNewPosition;
    public GameObject dialogueCanvas;


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            OopsieQueenAnimator.transform.position = OopsieQueenNewPosition.position;
            OopsieQueenAnimator.transform.rotation = OopsieQueenNewPosition.rotation;
            OopsieQueenAnimator.GetComponent<DialogueStarter>().readyToTalk = true;
            dialogueCanvas.SetActive(true);
            Destroy(this.gameObject);
        }
    }
}
