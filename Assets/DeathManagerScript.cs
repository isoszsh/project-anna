using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathManagerScript : MonoBehaviour
{
    public GameObject spawnPoint;
    public GameObject player;
    public GameObject witch;

    public GameObject darkenPanel;

    public GameObject reTrigger;

    public GameObject witchMusicManager;

    public void DeathCircle()
    {
        StartCoroutine(DeathCircleRoutine());
    }

    IEnumerator DeathCircleRoutine()
    {
        Debug.Log("Player hit by witch weapon");
        GameManager.Instance.playerController.lockControls = true;
        GameManager.Instance.playerController.ResetVelocity();
        witch.GetComponent<WitchController>().StopEveryThink();
        darkenPanel.SetActive(true);
        darkenPanel.GetComponent<Animator>().SetTrigger("Darken");
        yield return new WaitForSeconds(2.5f);
        witchMusicManager.GetComponent<WitchMusicManagerScript>().PlayBossLaugh();
        
        player.transform.position = spawnPoint.transform.position;
        PlayerController pc = player.GetComponent<PlayerController>();
        pc.lastCheckPoint = spawnPoint.transform;
        yield return new WaitForSeconds(3f);
        darkenPanel.SetActive(false);
        pc.WakeUp(true);
        reTrigger.SetActive(true);
    }
}
