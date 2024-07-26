using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBossStarterState : ChestBossBaseState
{
    public override void Enter()
    {
        //MIMIC-RIG|COMBO adındaki animasyonun speed değerini 1 yap
        boss.GetComponent<Animator>().speed = 1f;


    }

    public override void Perform()
    {
        // animasyon bittiği zaman state i değiştir
        if (boss.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !boss.GetComponent<Animator>().IsInTransition(0))
        {
            boss.GetComponent<ChestBossStateMachine>().ChangeStateToStunState();
            boss.GetComponent<Animator>().SetTrigger("Stun");
        }
    }

    public override void Exit()
    {

    }

}
