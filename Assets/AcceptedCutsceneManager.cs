using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceptedCutsceneManager : MonoBehaviour
{

    public GameObject mainCam;
    public GameObject cam1;
    public GameObject cam2;
    public GameObject cam3;
    public GameObject cam4;
    public GameObject cam5;
    public GameObject cam6;
    public GameObject cam7;

    public GameObject orca;

    public GameObject ice;

    public GameObject darkPanel;

    private void Start()
    {
        StartCoroutine(AcceptedCutscene());
    }



    IEnumerator AcceptedCutscene()
    {
        PlayerController pc = GameManager.Instance.playerController;
        pc.lockControls = true;
        mainCam.gameObject.SetActive(false);
        cam1.SetActive(true);
        cam1.GetComponent<Animator>().SetTrigger("Show");
        yield return new WaitForSeconds(5f);
        pc.playerAnimator.SetTrigger("Wave");
        cam1.SetActive(false);
        cam2.SetActive(true);
        yield return new WaitForSeconds(5f);
        cam2.SetActive(false);
        cam3.SetActive(true);
        yield return new WaitForSeconds(1);
        pc.transform.rotation = Quaternion.Euler(0, -161.83f, 0);
        yield return new WaitForSeconds(0.5f);
        pc.transform.GetComponent<Rigidbody>().isKinematic = true;
        pc.transform.position = new Vector3(-42.851f, -0.207f, -94.686f);
        pc.transform.rotation = Quaternion.Euler(0, -210.83f, 0);
        pc.transform.parent = ice.transform;
        
        yield return new WaitForSeconds(0.5f);
        cam3.SetActive(false);
        cam4.SetActive(true);
        ice.GetComponent<Animator>().SetTrigger("Move");
        yield return new WaitForSeconds(8f);
        darkPanel.SetActive(true);
        darkPanel.GetComponent<Animator>().SetTrigger("Darken");
        yield return new WaitForSeconds(5f);
        orca.gameObject.SetActive(false);
        transform.position = new Vector3(10.74f, 0, -32.96f);
        cam4.gameObject.SetActive(false);
        cam5.gameObject.SetActive(true);
        ice.GetComponent<Animator>().SetTrigger("Stop");
        darkPanel.GetComponent<Animator>().SetTrigger("Whiten");
        darkPanel.gameObject.SetActive(false);
        yield return new WaitForSeconds(10);
        pc.transform.parent = null;
        pc.transform.position = new Vector3(11.883f, 0, -35.187f);
        pc.transform.GetComponent<Rigidbody>().isKinematic = false;
        pc.lockControls = false;
        cam5.gameObject.SetActive(false);
        mainCam.gameObject.SetActive(true);
        
        

    }
}
