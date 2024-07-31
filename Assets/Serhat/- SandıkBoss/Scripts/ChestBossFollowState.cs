using UnityEngine;
using System.Collections.Generic;

public class ChestBossFollowState : ChestBossBaseState
{
    private List<GameObject> walls = new List<GameObject>();

    public override void Enter()
    {
        boss.GetComponent<Animator>().ResetTrigger("Stun");
        boss.GetComponent<Animator>().SetTrigger("Run");
        walls = boss.GetComponent<ChestBossEnemy>().walls;
        PlayEffectMusic();
        
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
    }

    public void FollowCycle()
    {
        if (bossStateMachine.playerTransform != null)
        {
            Vector3 newPosition = boss.transform.position + bossStateMachine.direction * Time.deltaTime * boss.Agent.speed;

            // Duvarlar ile çarpışma kontrolü
            if (!IsCollidingWithWalls(newPosition))
            {
                boss.transform.position = newPosition;
            }
            else
            {
                boss.GetComponent<ChestBossStateMachine>().ChangeStateToStunState();
            }
        }
    }

    private bool IsCollidingWithWalls(Vector3 position)
    {
        RaycastHit hit;
        // Duvarların her birine bir raycast gönder
        foreach (var wall in walls)
        {
            if (Physics.Raycast(position, bossStateMachine.direction, out hit, 1f))
            {
                if (hit.collider.gameObject == wall)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void PlayEffectMusic()
    {
        boss.GetComponent<PlayMusicBoss>().BrustMusic();
    }
}
