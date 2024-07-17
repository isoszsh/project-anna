using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class example1 : MonoBehaviour
{
    public GameObject sprite;
    public ParticleSystem particals;

    private void Start()
    {
        StartCoroutine(waitForIt());
    }

    IEnumerator waitForIt()
    {
        yield return new WaitForSeconds(2);
        Play();
    }

    public void Play()
    {
        sprite.gameObject.SetActive(false);
        particals.Emit(9999);
    }
}
