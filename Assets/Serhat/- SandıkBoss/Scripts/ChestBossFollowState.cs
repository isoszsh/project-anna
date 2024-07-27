using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBossFollowState : ChestBossBaseState
{
    private Vector3 playerTransform;

    private Vector3 direction;

    private List<GameObject> walls = new List<GameObject>();

    public override void Enter()
    {
        boss.GetComponent<Animator>().ResetTrigger("Stun");
        boss.GetComponent<Animator>().SetTrigger("Run");
        
        FindThePlayerTransform();
        walls = boss.GetComponent<ChestBossEnemy>().walls;
    }

    public override void Perform()
    {
        RunToPlayer();
    }

    public override void Exit()
    {
    }

    public void RunToPlayer()
    {
        FollowCycle();
        // coliision check
        RaycastHit hit;
        // raycast'i görünür yap
        Debug.DrawRay(boss.transform.position + boss.transform.forward, boss.transform.forward * 0.5f, Color.black);

        // raycast'in boyunu ayarla

        if (Physics.Raycast(boss.transform.position + boss.transform.forward, boss.transform.forward * 0.5f, out hit, 0.5f))
        {
            if ( walls.Contains(hit.collider.gameObject) )
            {
                
                boss.GetComponent<ChestBossStateMachine>().ChangeStateToStunState();
            }
        }

        else if (Vector3.Distance(boss.transform.position, playerTransform) < 0.5f)
        {
            // Calculate the new player transform using the forward direction of the boss
            playerTransform = boss.transform.position + boss.transform.forward * 2;
        }

    }

    public void FollowCycle()
    {
        if (playerTransform != null)
        {
            boss.Agent.SetDestination(playerTransform);
        }
    }

    public void FindThePlayerTransform()
    {
        playerTransform = boss.GetComponent<ChestBossEnemy>().player.transform.position;
        
        // player'a doğru döndüğündeki rotasyonunu kaydet
        direction = playerTransform - boss.transform.position;

    }
}
