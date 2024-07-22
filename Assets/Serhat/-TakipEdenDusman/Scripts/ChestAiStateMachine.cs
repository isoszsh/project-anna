using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestAiStateMachine : MonoBehaviour
{
    public ChestAiBaseState activeState;
    public ChestAiPatroleState patroleState;

    public ChestAiFoundState foundState;

    public ChestAiEndState endState;


    public void Initialise()
    {
        patroleState = new ChestAiPatroleState();
        ChangeState(patroleState);
        //setup default state
    }

    public void Initialise2(GameObject Target)
    {
        foundState = new ChestAiFoundState(Target);
        ChangeState(foundState);
    }

    public void Initialise3()
    {
        endState = new ChestAiEndState();
        ChangeState(endState);
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
