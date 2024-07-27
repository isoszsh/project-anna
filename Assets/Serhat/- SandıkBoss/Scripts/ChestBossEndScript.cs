using System.Collections;
using UnityEngine;

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
        //boss'un local scale'ini yavaşca 2f yap
        while (boss.transform.localScale.x < 1.9f)
        {
            boss.transform.localScale = Vector3.Lerp(boss.transform.localScale, Vector3.one * 2, Time.deltaTime * 2);
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        //boss'un local scale'ini yavaşca 0.01f yap
        while (boss.transform.localScale.x > 0.01f)
        {
            boss.transform.localScale = Vector3.Lerp(boss.transform.localScale, Vector3.zero, Time.deltaTime * 2);
            yield return null;
        }
    }
}
