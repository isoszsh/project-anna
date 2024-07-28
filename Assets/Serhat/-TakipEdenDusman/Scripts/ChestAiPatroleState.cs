using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestAiPatroleState : ChestAiBaseState
{
    public bool isPatroling;
    public override void Enter()
    {  
        Debug.Log("Patrole State");
        enemy.GetComponent<Animator>().SetTrigger("Walk");
        enemy.Agent.SetDestination(enemy.path.waypoints[stateMachine.activeWaypointIndex].position);
    }
    public override void Perform()
    {
        PatrolCycle();
    }   

    public override void Exit()
    {
    }

    public void PatrolCycle()
    {
        //Patrol the path
        if(enemy.Agent.remainingDistance < 0.2f)
        {
            if (stateMachine.activeWaypointIndex < enemy.path.waypoints.Count - 1)
            {
                stateMachine.activeWaypointIndex++;
                
            }
            else
            {
                stateMachine.activeWaypointIndex = 0;
            }
            stateMachine.IdleState();
        }
    }
}
