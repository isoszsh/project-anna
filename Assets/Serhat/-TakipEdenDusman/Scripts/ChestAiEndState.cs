using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChestAiEndState : ChestAiBaseState
{

    public override void Enter()
    {
        stateMachine.isPlayerFound = true;
        enemy.Agent.SetDestination(enemy.transform.position);
        enemy.GetComponent<Animator>().SetTrigger("Catch");
        enemy.StartCoroutine(EndCycleWait());
    }
    public override void Perform()
    {
    }   

    public override void Exit()
    {
    }

    public void EndCycle()
    {
        Debug.Log("Player is too close \n Ismail hocam burası değişecek");
        //senin kullandığına benzer bir state mantığı var o yüzden yeni bir state açıp ona geçiş yapılabilir.
        //ve onun içinden bir fonkisyon tetiklenebilir. 
        //eğer karmaşık bir yapı varsa ben 10 dk ya özetlerim sana.
    }

    IEnumerator EndCycleWait()
    {
        enemy.darkenPanel.SetActive(true);
        enemy.darkenPanel.GetComponent<Animator>().SetTrigger("Darken");

        yield return new WaitForSeconds(2);
        // Geçerli sahnenin adını alır
        string sceneName = SceneManager.GetActiveScene().name;
        // Sahneyi yeniden yükler
        SceneManager.LoadScene(sceneName);
    }
}
