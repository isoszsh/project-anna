using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestAiCantFound : ChestAiBaseState
{
    public override void Enter()
    { 
    }
    public override void Perform()
    {
        CantFound();
    }   

    public override void Exit()
    {
    }

    public void CantFound()
    {
        //Patrol the path
        if(enemy.Agent.remainingDistance < 0.2f)
        {
            enemy.GetComponent<Animator>().ResetTrigger("Idle");
            enemy.GetComponent<Animator>().SetTrigger("LookAround");
            enemy.StartCoroutine(CantFoundCycle());
        }
    }

    IEnumerator CantFoundCycle()
    {
        yield return new WaitForSeconds(5);
        if(stateMachine.activeState == this){
            enemy.GetComponent<Animator>().ResetTrigger("LookAround");
            enemy.GetComponent<Animator>().SetTrigger("Walk");
            stateMachine.PatroleState();
        }
    }
}
