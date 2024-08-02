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
        boss.GetComponent<WitchController>().StopEveryThink();
        originalCamra.SetActive(false);
        camraBoss.SetActive(true);
        yield return new WaitForSeconds(1f);
        decisionPanel.SetActive(true);
    }
}
