using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestAiEndState : ChestAiBaseState
{
    public override void Enter()
    {
        stateMachine.isPlayerFound = true;
        enemy.Agent.SetDestination(enemy.transform.position);
        enemy.GetComponent<Animator>().SetTrigger("Catch");
        EndCycle();
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
}
