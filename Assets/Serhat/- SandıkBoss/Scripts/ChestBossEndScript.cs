using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class ChestBossEndScript : ChestBossBaseState
{
    public override void Enter()
    {
        boss.StartCoroutine(EndAnim());
    }

    public override void Perform()
    {

    }

    public override void Exit()
    {
    }

    private IEnumerator EndAnim()
    {
        boss.GetComponent<PlayMusicBoss>().BlowMusic();
        //boss'un local scale'ini yavaşça 2f yap
        while (boss.transform.localScale.x < 1.9f)
        {
            boss.transform.localScale = Vector3.Lerp(boss.transform.localScale, Vector3.one * 2, Time.deltaTime * 2);
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);
        boss.GetComponent<PlayMusicBoss>().PopMusic();

        // boss.popParticleEffectObject'i spawnla
        popParticleEffect();


        //boss'un local scale'ini yavaşça 0.01f yap
        while (boss.transform.localScale.x > 0.01f)
        {
            boss.transform.localScale = Vector3.Lerp(boss.transform.localScale, Vector3.zero, Time.deltaTime * 2);
            yield return null;
        }
    }

    private void popParticleEffect(){
        GameObject popParticleEffect = GameObject.Instantiate(boss.popParticleEffectObject, boss.transform.position, Quaternion.identity);
        GameObject.Destroy(popParticleEffect, 2f);
    }
}
