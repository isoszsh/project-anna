using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestAiHearRock : ChestAiBaseState
{
    public GameObject target;


    public ChestAiHearRock(GameObject Target)
    {
        target = Target;
    }
    
    public override void Enter()
    {
        enemy.GetComponent<Animator>().ResetTrigger("Idle");
        enemy.GetComponent<Animator>().ResetTrigger("Walk");        
        enemy.GetComponent<Animator>().SetTrigger("Run");
    }

    public override void Perform()
    {
        float distance = Vector3.Distance(enemy.transform.position, target.transform.position);
        enemy.Agent.SetDestination(target.transform.position);

        if (distance > 1)
        {
            enemy.GetComponent<Animator>().SetTrigger("Run");
            stateMachine.CantFoundState();
        }

    }   

    public override void Exit()
    {
    }
}
