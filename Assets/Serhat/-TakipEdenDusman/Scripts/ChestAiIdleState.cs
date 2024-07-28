using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestAiIdleState : ChestAiBaseState
{
    public override void Enter()
    {  
        enemy.GetComponent<Animator>().SetTrigger("Idle");
        enemy.StartCoroutine(IdleCycle());
    }
    public override void Perform()
    {
    }   

    public override void Exit()
    {
    }

    IEnumerator IdleCycle()
    {
        yield return new WaitForSeconds(1);
        if(stateMachine.activeState == this){
            stateMachine.PatroleState();
        }
    }
}
