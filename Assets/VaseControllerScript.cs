using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaseControllerScript : MonoBehaviour
{
    public int howManyFlowers = 5;

    public GameObject boss;

    public GameObject originalCamra;
    public GameObject camraBoss;

    public GameObject decisionPanel;

    public AudioSource bossAus;
    public AudioClip wdeath;

    public Transform annaPosition;

    public GameObject annaCam;
    public GameObject witchItself;

    public GameObject[] vases;

    public GameObject cauldronCam;

    public GameObject cauldronItself;

    public GameObject[] cauldronItems;
    public GameObject cauldronInside;

    public Material redMaterial;

    public GameObject potion;
    public GameObject potionDC;


    public void FlowerCountMinus()
    {
        howManyFlowers--;
        if (howManyFlowers == 0)
        {
            StartCoroutine(EndRoutine());
            Debug.Log("Oyun bitti");
        }
    }

    public void FlowerAnimDown()
    {
        this.gameObject.GetComponent<Animator>().SetTrigger("Down");
    }

    public void FlowerAnimUp()
    {
        this.gameObject.GetComponent<Animator>().SetTrigger("Up");
    }

    IEnumerator EndRoutine()
    {
        foreach (GameObject go in vases)
        {
            go.SetActive(false);
        }
        GameManager.Instance.playerController.lockControls = true;
        boss.GetComponent<WitchController>().StopEveryThink();
        boss.GetComponent<WitchController>().enabled = false;
        originalCamra.SetActive(false);
        camraBoss.SetActive(true);
        bossAus.PlayOneShot(wdeath);
        GameManager.Instance.playerController.transform.position = annaPosition.position;
        GameManager.Instance.playerController.transform.rotation = Quaternion.Euler(0, 0, 0);
        yield return new WaitForSeconds(13f);
        witchItself.SetActive(false);
        yield return new WaitForSeconds(3);
        camraBoss.SetActive(false);
        
        annaCam.SetActive(true );
        decisionPanel.SetActive(true);
    }

    public void Select1()
    {
        GameManager.Instance.pr.SaveDecision("Level_5", 2);
        StartCoroutine(DestroyCauldron());
    }


    IEnumerator DestroyCauldron()
    {
        decisionPanel.SetActive(false);
        annaCam.SetActive(false );
        cauldronCam.SetActive(true);
        yield return new WaitForSeconds(1);
        cauldronItself.SetActive(false);
        yield return new WaitForSeconds(2);
        cauldronCam.SetActive(false) ;
        originalCamra.SetActive(true);
        potion.SetActive(true);
        potionDC.SetActive(true);

    }

    public void Select2()
    {
        GameManager.Instance.pr.SaveDecision("Level_5", 2);
        StartCoroutine(UseCauldron());
        
    }

    IEnumerator UseCauldron()
    {
        decisionPanel.SetActive(false);
        annaCam.SetActive(false);
        cauldronCam.SetActive(true);
        yield return new WaitForSeconds(1);
        foreach (GameObject go in cauldronItems)
        {
            yield return new WaitForSeconds(.25f);
            go.SetActive(false);
        }
        cauldronInside.GetComponent<MeshRenderer>().material = redMaterial;
        yield return new WaitForSeconds(2);
        cauldronCam.SetActive(false);
        originalCamra.SetActive(true);
        potion.SetActive(true );
        potionDC.SetActive(true);
        
    }
}
