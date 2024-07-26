using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBossStateMachine : MonoBehaviour
{
    public ChestBossBaseState activeState;
    public ChestBossFollowState followState;

    public ChestBossSpawnState spawnState;

    public ChestBossStunState stunState;

    public ChestBossStarterState starterState;


    public void Initialise()
    {
        ChangeStateToStarterState();
    }

    public void ChangeStateToStunState(){
        stunState = new ChestBossStunState();
        ChangeState(new ChestBossStunState());
    }

    public void ChangeStateToFollowState(){
        followState = new ChestBossFollowState();
        ChangeState(followState);
    }

    public void ChangeStateToSpawnState(){
        spawnState = new ChestBossSpawnState();
        ChangeState(spawnState);
    }

    public void ChangeStateToStarterState(){
        starterState = new ChestBossStarterState();
        ChangeState(starterState);
    }

    void Start()
    {
        this.GetComponent<Animator>().speed = 0f;
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
