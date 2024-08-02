using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PotionEvent : Event
{
    public Image dcImg;

    public Sprite desiredSprite;

    private Sprite storedSprite;

    public AudioSource aus;
    public AudioClip auc;

    public GameObject darkenPanel;

    public GameObject potionDC;
    public GameObject mainCamera;
    public GameObject mainCamera2;
    public GameObject potionCamera;

    public Transform pickPoint;



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            storedSprite = dcImg.sprite;
            dcImg.sprite = desiredSprite;
            GameManager.Instance.playerController.currentEvent = this;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dcImg.sprite = storedSprite;
            GameManager.Instance.playerController.currentEvent = null;
        }
    }

    public override void TriggerEvent()
    {
        StartCoroutine(Drink());
    }

    IEnumerator Drink()
    {
        potionDC.SetActive(false);
        mainCamera.SetActive(false);
        mainCamera2.SetActive(false);
        potionCamera.SetActive(true);
        GameManager.Instance.playerController.lockControls = true;
        GameManager.Instance.playerController.playerAnimator.SetTrigger("Drink");
        this.GetComponent<BoxCollider>().enabled = false;
        transform.parent = pickPoint;
        transform.position = pickPoint.position;
        transform.localPosition = new Vector3(-0.00258f, -0.00487f, -0.001f);
        transform.localRotation = Quaternion.Euler(0,0,-28.49f);
        yield return new WaitForSeconds(1.5f);
        aus.PlayOneShot(auc);
        yield return new WaitForSeconds(2);
        darkenPanel.SetActive(true);
        darkenPanel.GetComponent<Animator>().SetTrigger("Darken");
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadSceneAsync("lv5_to_lv6");
    }
}
