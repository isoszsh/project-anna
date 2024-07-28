using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestAiStateMachine : MonoBehaviour
{
    public ChestAiBaseState activeState;
    public ChestAiPatroleState patroleState;

    public ChestAiFoundState foundState;
    public ChestAiCantFound cantFoundState;

    public ChestAiEndState endState;

    public ChestAiIdleState idleState;

    public ChestAiHearRock rockState;

    public int activeWaypointIndex = 0;


    public void PatroleState()
    {
        patroleState = new ChestAiPatroleState();
        ChangeState(patroleState);
        //setup default state
    }

    public void FoundPlayerState(GameObject Target)
    {
        foundState = new ChestAiFoundState(Target);
        ChangeState(foundState);
    }

    public void RockState(GameObject Target)
    {
        rockState = new ChestAiHearRock(Target);
        ChangeState(rockState);
    }

    public void EndState()
    {
        endState = new ChestAiEndState();
        ChangeState(endState);
    }

    public void CantFoundState()
    {
        cantFoundState = new ChestAiCantFound();
        ChangeState(cantFoundState);
    }

    public void IdleState()
    {
        idleState = new ChestAiIdleState();
        ChangeState(idleState);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(activeState != null){
            activeState.Perform();
        }

    }

    public void ChangeState(ChestAiBaseState newState)
    {
        if (activeState != null)
        {
            activeState.Exit();
        }
        activeState = newState;
        
        if (activeState != null)
        {
            activeState.stateMachine = this;
            activeState.enemy = GetComponent<ChestAiEnemy>();
            activeState.Enter();
        }
    }
}
