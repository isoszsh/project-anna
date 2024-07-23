using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBossStateMachine : MonoBehaviour
{
    public ChestBossBaseState activeState;
    public ChestBossFollowState followState;

    public ChestBossStunState stunState;


    public void Initialise()
    {
        ChangeStateToFollowState();
    }

    public void ChangeStateToStunState(){
        stunState = new ChestBossStunState();
        ChangeState(new ChestBossStunState());
    }

    public void ChangeStateToFollowState(){
        followState = new ChestBossFollowState();
        ChangeState(followState);
    }

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

    public void ChangeState(ChestBossBaseState newState)
    {
        if (activeState != null)
        {
            activeState.Exit();
        }
        activeState = newState;
        
        if (activeState != null)
        {
            activeState.bossStateMachine = this;
            activeState.boss = GetComponent<ChestBossEnemy>();
            activeState.Enter();
        }
    }
}
