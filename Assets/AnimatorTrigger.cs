using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorTrigger : MonoBehaviour
{
    public GameObject animatorCam;

    public void AnimatorCamActive()
    {
        animatorCam.GetComponent<Animator>().SetTrigger("Play");
    }
}
