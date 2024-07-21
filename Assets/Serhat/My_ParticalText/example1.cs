using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class example1 : MonoBehaviour
{
    public GameObject sprite;
    public ParticleSystem particals;

    public GameObject CameraPov;
    public GameObject CameraAnim;

    public int waitTime;

    public void Play()
    {
        CameraPov.SetActive(true);
        CameraAnim.SetActive(false);

        StartCoroutine(Wait(waitTime));
    }

    IEnumerator Wait(int i)
    {
        yield return new WaitForSeconds(i);

        sprite.gameObject.SetActive(false);
        particals.Emit(9999);
    }

}
