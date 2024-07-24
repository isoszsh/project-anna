using System.Collections;
using UnityEngine;

public class ChestBossStunState : ChestBossBaseState
{
    private int stunTime = 3;
    private GameObject player;
    private bool readyToLook = false;

    public override void Enter()
    {
        boss.GetComponent<Animator>().ResetTrigger("Run");
        boss.GetComponent<Animator>().SetTrigger("Stun");

        player = boss.GetComponent<ChestBossEnemy>().player;

        // Coroutine'i başlat
        boss.StartCoroutine(StunRoutine());
    }

    public override void Perform()
    {
        if (readyToLook)
        {
            SmooteLookAtPlayer();
        }
    }

    public override void Exit()
    {
    }

    private void SmooteLookAtPlayer()
    {
        Vector3 direction = player.transform.position - boss.transform.position;
        direction.y = 0; // Y ekseninde dönmeyi engellemek için
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        boss.transform.rotation = Quaternion.Slerp(boss.transform.rotation, targetRotation, Time.deltaTime * 2 );
    }

    private IEnumerator StunRoutine()
    {
        // Stun süresi boyunca bekle
        yield return new WaitForSeconds(stunTime);
        // 3 saniye boyunca oyuncuya dön
        readyToLook = true;
        yield return new WaitForSeconds(3);
        // Takip state'ine geçiş yap
        boss.GetComponent<ChestBossStateMachine>().ChangeStateToFollowState();
    }
}
