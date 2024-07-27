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

    public ChestBossEndScript endState;

    public int stunNumber;

    public int spawnNumber;


    public void Initialise()
    {
        starterState = new ChestBossStarterState();
        spawnState = new ChestBossSpawnState();
        followState = new ChestBossFollowState();
        stunState = new ChestBossStunState();
        endState = new ChestBossEndScript();


        ChangeStateToStarterState();
    }

    public void GameLoop(){
        if(stunNumber > 0){
            ChangeStateToFollowState();
            stunNumber--;
        }
        else if(spawnNumber > 0){
            ChangeStateToSpawnState();
        }
        else{
            ChangeStateToEndState();
        }
    }

    public void ChangeStateToStunState(){
        ChangeState(new ChestBossStunState());
    }

    public void ChangeStateToFollowState(){
        ChangeState(followState);
    }

    public void ChangeStateToSpawnState(){
        ChangeState(spawnState);
    }

    public void ChangeStateToStarterState(){
        ChangeState(starterState);
    }

    public void ChangeStateToEndState(){
        ChangeState(endState);
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
