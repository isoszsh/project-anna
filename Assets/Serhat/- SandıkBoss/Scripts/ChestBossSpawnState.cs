using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChestBossSpawnState : ChestBossBaseState
{
    public List<GameObject> topSpawnPoints = new List<GameObject>();
    public List<GameObject> bottomSpawnPoints = new List<GameObject>();
    public List<GameObject> leftSpawnPoints = new List<GameObject>();
    public List<GameObject> rightSpawnPoints = new List<GameObject>();
    public List<GameObject> spawnObject = new List<GameObject>();
    public GameObject trailParent ;
    public Vector3 tempPos;
    public Vector3 lavaWallTempPos;
    public bool youShouldLook = true;
    public float spownPointTopY;
    public float spownPointMidY;
    public float spownPointDownY;

    public void Start() {

        topSpawnPoints = boss.topSpawnPoints;
        bottomSpawnPoints = boss.bottomSpawnPoints;
        leftSpawnPoints = boss.leftSpawnPoints;
        rightSpawnPoints = boss.rightSpawnPoints;
        spawnObject = boss.spawnObject;

        trailParent = boss.TrailParent;
    }

    public override void Enter()
    {
        topSpawnPoints = boss.topSpawnPoints;
        bottomSpawnPoints = boss.bottomSpawnPoints;
        leftSpawnPoints = boss.leftSpawnPoints;
        rightSpawnPoints = boss.rightSpawnPoints;
        spawnObject = boss.spawnObject;

        trailParent = boss.TrailParent;

        spownPointTopY = boss.spawnPointTop.transform.position.y;
        spownPointMidY = boss.spawnPointMid.transform.position.y;
        spownPointDownY = boss.spawnPointDown.transform.position.y;

        boss.Agent.isStopped = false;
        boss.Agent.SetDestination(boss.allSpawnPointParentObject.transform.position);
        boss.GetComponent<Animator>().ResetTrigger("Run");
        boss.GetComponent<Animator>().SetTrigger("ReturnChest");

        
        if(bossStateMachine.spawnNumber == 2){
            boss.StartCoroutine(EventLine1());
        }
        else if(bossStateMachine.spawnNumber == 1){
            boss.StartCoroutine(EventLine2());
        }        
    }

    public override void Perform()
    {
    }

    public override void Exit()
    {
    }

    private IEnumerator LookAtTarget()
    {
        while (youShouldLook)
        {
            Vector3 direction = boss.bossLookPosition.transform.position - boss.transform.position;
            direction.y = 0; // Keep only the horizontal direction
            boss.transform.rotation = Quaternion.Slerp(boss.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 20);
            
            if (boss.transform.rotation == boss.bossLookPosition.transform.rotation)
            {
                youShouldLook = false;
                break;
            }

            yield return null;
        }
    }

    public void SetAllSpawnPointParentObject(GameObject spawnerHolder, GameObject spawnerPoint){
        spawnerHolder.transform.position = spawnerPoint.transform.position; 
    }

    public IEnumerator LavaWallShow(){
        // while döngüsü ile yavaşca yukarı çık
        var newPos = lavaWallTempPos;
        newPos.y += 2;

        while (boss.lavaWall.transform.position != newPos)
        {
            boss.lavaWall.transform.position = Vector3.Lerp(boss.lavaWall.transform.position, newPos , Time.deltaTime * 2);
            yield return null;
        }
    }

    public IEnumerator LavaWallHide(){

        while (boss.lavaWall.transform.position != lavaWallTempPos)
        {
            boss.lavaWall.transform.position = Vector3.Lerp(boss.lavaWall.transform.position, lavaWallTempPos, Time.deltaTime * 2);
            yield return null;
        }
    }

    private IEnumerator EventLine1()
    {

        yield return new WaitForSeconds(2f);
        youShouldLook = true;
        boss.StartCoroutine(LookAtTarget());
        yield return new WaitForSeconds(2f);


        yield return Last4Spawn(boss.spawnerHolder1, spownPointDownY);
        yield return new WaitForSeconds(1f);
        yield return First4Spawn(boss.spawnerHolder2, spownPointMidY);
        yield return new WaitForSeconds(1f);
        yield return Last4Spawn(boss.spawnerHolder3, spownPointTopY);
        yield return new WaitForSeconds(2f);
        
        yield return AllSpawn(spownPointDownY);
        yield return new WaitForSeconds(1f);
        yield return AllSpawn(spownPointMidY);
        yield return new WaitForSeconds(1f);
        yield return AllSpawn(spownPointTopY);
        yield return new WaitForSeconds(1f);

        yield return new WaitForSeconds(1f);
        yield return SpiralSpawn(spownPointMidY);
        yield return ReversSpiralSpawn(spownPointMidY);
        
        yield return new WaitForSeconds(3f);

        youShouldLook = false;
        
        bossStateMachine.stunNumber = 2;
        boss.Agent.speed = 10;
        bossStateMachine.spawnNumber--;

        boss.GetComponent<Animator>().SetTrigger("Stun");
        boss.GetComponent<Animator>().ResetTrigger("ReturnChest");

        boss.Agent.isStopped = true;

        boss.GetComponent<ChestBossStateMachine>().ChangeStateToStunState();
    }

    private IEnumerator EventLine2()
    {

        yield return new WaitForSeconds(2f);
        youShouldLook = true;
        boss.StartCoroutine(LookAtTarget());
        yield return new WaitForSeconds(2f);

        
        yield return AllSpawn(spownPointTopY);
        yield return new WaitForSeconds(0.5f);
        yield return AllSpawn(spownPointMidY);
        yield return new WaitForSeconds(0.5f);
        yield return AllSpawn(spownPointDownY);

        yield return new WaitForSeconds(1f);

        yield return SpiralSpawn(spownPointDownY);
        yield return ReversSpiralSpawn(spownPointDownY);
        yield return SpiralSpawn(spownPointMidY);
        yield return ReversSpiralSpawn(spownPointMidY);
        yield return SpiralSpawn(spownPointTopY);
        yield return ReversSpiralSpawn(spownPointTopY);

        yield return new WaitForSeconds(1f);

        yield return FastAnd3Time();
        
        yield return new WaitForSeconds(1f);

        yield return FastAnd3TimeWithTime();

        youShouldLook = false;

        yield return new WaitForSeconds(2f);

        boss.Agent.isStopped = true;
        bossStateMachine.spawnNumber--;
        boss.GetComponent<ChestBossStateMachine>().GameLoop();

    }

    private IEnumerator FastAnd3Time()
    {
        PlayEffectMusic();
        UltiSameTimeSpawn(0, topSpawnPoints.Count, false, boss.spawnerHolder1 , "top", spownPointDownY);
        UltiSameTimeSpawn(0, bottomSpawnPoints.Count, false, boss.spawnerHolder1 , "bottom", spownPointDownY);
        yield return new WaitForSeconds(0.5f);
        UltiSameTimeSpawn(0, rightSpawnPoints.Count, false, boss.spawnerHolder1 , "right", spownPointDownY);
        UltiSameTimeSpawn(0, leftSpawnPoints.Count, false, boss.spawnerHolder1 , "left", spownPointDownY);

        PlayEffectMusic();
        yield return new WaitForSeconds(0.5f);
        UltiSameTimeSpawn(0, topSpawnPoints.Count, false, boss.spawnerHolder2 , "top", spownPointMidY);
        UltiSameTimeSpawn(0, bottomSpawnPoints.Count, false, boss.spawnerHolder2 , "bottom", spownPointMidY);
        yield return new WaitForSeconds(0.5f);
        UltiSameTimeSpawn(0, rightSpawnPoints.Count, false, boss.spawnerHolder2 , "right", spownPointMidY);
        UltiSameTimeSpawn(0, leftSpawnPoints.Count, false, boss.spawnerHolder2 , "left", spownPointMidY);
        SetAllSpawnPointParentObject(boss.spawnerHolder2, boss.spawnPointMid);

        PlayEffectMusic();
        yield return new WaitForSeconds(0.5f);
        UltiSameTimeSpawn(0, topSpawnPoints.Count, false, boss.spawnerHolder3 , "top", spownPointTopY);
        UltiSameTimeSpawn(0, bottomSpawnPoints.Count, false, boss.spawnerHolder3 , "bottom", spownPointTopY);
        yield return new WaitForSeconds(0.5f);
        UltiSameTimeSpawn(0, rightSpawnPoints.Count, false, boss.spawnerHolder3 , "right", spownPointTopY);
        UltiSameTimeSpawn(0, leftSpawnPoints.Count, false, boss.spawnerHolder3 , "left", spownPointTopY);
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator FastAnd3TimeWithTime(){
        PlayEffectMusic();
        RespectivelySpawn(0, topSpawnPoints.Count, false, boss.spawnerHolder1 , "top", spownPointDownY);
        RespectivelySpawn(0, bottomSpawnPoints.Count, false, boss.spawnerHolder1 , "bottom", spownPointDownY);
        yield return new WaitForSeconds(0.5f);
        RespectivelySpawn(0, rightSpawnPoints.Count, false, boss.spawnerHolder1 , "right", spownPointDownY);
        RespectivelySpawn(0, leftSpawnPoints.Count, false, boss.spawnerHolder1 , "left", spownPointDownY);

        PlayEffectMusic();
        yield return new WaitForSeconds(0.5f);
        RespectivelySpawn(0, topSpawnPoints.Count, false, boss.spawnerHolder2 , "top", spownPointMidY);
        RespectivelySpawn(0, bottomSpawnPoints.Count, false, boss.spawnerHolder2 , "bottom", spownPointMidY);
        yield return new WaitForSeconds(0.5f);
        RespectivelySpawn(0, rightSpawnPoints.Count, false, boss.spawnerHolder2 , "right", spownPointMidY);
        RespectivelySpawn(0, leftSpawnPoints.Count, false, boss.spawnerHolder2 , "left", spownPointMidY);
        SetAllSpawnPointParentObject(boss.spawnerHolder2, boss.spawnPointMid);

        PlayEffectMusic();
        yield return new WaitForSeconds(0.5f);
        RespectivelySpawn(0, topSpawnPoints.Count, false, boss.spawnerHolder3 , "top", spownPointTopY);
        RespectivelySpawn(0, bottomSpawnPoints.Count, false, boss.spawnerHolder3 , "bottom", spownPointTopY);
        yield return new WaitForSeconds(0.5f);
        RespectivelySpawn(0, rightSpawnPoints.Count, false, boss.spawnerHolder3 , "right", spownPointTopY);
        RespectivelySpawn(0, leftSpawnPoints.Count, false, boss.spawnerHolder3 , "left", spownPointTopY);
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator Last4Spawn(GameObject spanwerHolder,float spawnPointY)
    {
        PlayEffectMusic();
        UltiSameTimeSpawn(0, topSpawnPoints.Count / 2, false, spanwerHolder , "top", spawnPointY);
        UltiSameTimeSpawn(0, rightSpawnPoints.Count / 2, false, spanwerHolder , "right", spawnPointY);
        UltiSameTimeSpawn(0, bottomSpawnPoints.Count / 2, false, spanwerHolder , "bottom", spawnPointY);
        UltiSameTimeSpawn(0, leftSpawnPoints.Count / 2, false, spanwerHolder , "left", spawnPointY);
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator First4Spawn(GameObject spanwerHolder,float spawnPointY)
    {
        PlayEffectMusic();
        UltiSameTimeSpawn(topSpawnPoints.Count / 2, topSpawnPoints.Count, false, spanwerHolder , "top", spawnPointY);
        UltiSameTimeSpawn(rightSpawnPoints.Count / 2, rightSpawnPoints.Count, false, spanwerHolder , "right", spawnPointY);
        UltiSameTimeSpawn(bottomSpawnPoints.Count / 2, bottomSpawnPoints.Count, false, spanwerHolder , "bottom", spawnPointY);
        UltiSameTimeSpawn(leftSpawnPoints.Count / 2, leftSpawnPoints.Count, false, spanwerHolder , "left", spawnPointY);
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator AllSpawn(float spawnPointY)
    {
        PlayEffectMusic();
        UltiSameTimeSpawn(0, topSpawnPoints.Count, false, boss.spawnerHolder1 , "top",spawnPointY);
        UltiSameTimeSpawn(0, rightSpawnPoints.Count, false, boss.spawnerHolder1 , "right",spawnPointY);
        UltiSameTimeSpawn(0, bottomSpawnPoints.Count, false, boss.spawnerHolder1 , "bottom",spawnPointY);
        UltiSameTimeSpawn(0, leftSpawnPoints.Count, false, boss.spawnerHolder1 , "left",spawnPointY);
        yield return new WaitForSeconds(0.5f);  
    }

    private IEnumerator PartPartSpawn(float spawnPointY)
    {
        PlayEffectMusic();
        UltiSameTimeSpawn(0, topSpawnPoints.Count, false, boss.spawnerHolder1 , "top",spawnPointY);
        UltiSameTimeSpawn(0, rightSpawnPoints.Count, false, boss.spawnerHolder1 , "right",spawnPointY);
        yield return new WaitForSeconds(0.1f);
        UltiSameTimeSpawn(0, bottomSpawnPoints.Count, false, boss.spawnerHolder1 , "bottom",spawnPointY);
        UltiSameTimeSpawn(0, leftSpawnPoints.Count, false, boss.spawnerHolder1 , "left",spawnPointY);
        yield return new WaitForSeconds(0.5f);  
    }

    private IEnumerator SpiralSpawn(float spawnPointY)
    {
        PlayEffectMusic();
        yield return RespectivelySpawn(0, topSpawnPoints.Count,false, boss.spawnerHolder1 ,  "top",spawnPointY);
        yield return RespectivelySpawn(0, leftSpawnPoints.Count,false, boss.spawnerHolder1 ,  "left",spawnPointY);
        yield return RespectivelySpawn(0, bottomSpawnPoints.Count,false, boss.spawnerHolder1 ,  "bottom",spawnPointY);
        yield return RespectivelySpawn(0, rightSpawnPoints.Count,false, boss.spawnerHolder1 ,  "right",spawnPointY);
    }

    private IEnumerator ReversSpiralSpawn(float spawnPointY)
    {
        PlayEffectMusic();
        yield return RespectivelySpawn(topSpawnPoints.Count - 1, 0, true, boss.spawnerHolder1 , "top",spawnPointY);
        yield return RespectivelySpawn(leftSpawnPoints.Count - 1, 0, true, boss.spawnerHolder1 , "left",spawnPointY);
        yield return RespectivelySpawn(bottomSpawnPoints.Count - 1, 0, true, boss.spawnerHolder1 , "bottom",spawnPointY);
        yield return RespectivelySpawn(rightSpawnPoints.Count - 1, 0, true, boss.spawnerHolder1 , "right", spawnPointY);
    }

    public void UltiSameTimeSpawn(int firstParam, int secondParam, bool isNegative, GameObject ParentObject , string direction, float spawnPointY)
    {
        List<GameObject> spawnPoints = GetSpawnPoints(direction);
        if (spawnPoints == null) return;

        for (int i = firstParam; i < secondParam; i = isNegative ? i - 1 : i + 1)
        {
            if (i < 0 || i >= spawnPoints.Count) continue;

            GameObject spawnPointPos = spawnPoints[i];

            GameObject spawningObject = spawnObject[Random.Range(0, spawnObject.Count)];

            Vector3 newPos = spawnPointPos.transform.position;
            newPos.y = spawnPointY;

            GameObject TrailClone = GameObject.Instantiate(spawningObject, newPos, spawnPointPos.transform.rotation * Quaternion.Euler(0, 270, 0));

            TrailClone.transform.SetParent(ParentObject.transform);

            
            TrailClone.transform.localScale = new Vector3(TrailClone.transform.localScale.x * boss.shrinkageRate, 
                                                          TrailClone.transform.localScale.y * boss.shrinkageRate, 
                                                          TrailClone.transform.localScale.z * boss.shrinkageRate);

            TrailClone.GetComponentInChildren<Light>().color = DecideColor(spawnPointY);
        }
    }

    public IEnumerator RespectivelySpawn(int firstParam, int secondParam, bool isNegative, GameObject ParentObject ,string direction , float spawnPointY)
    {
        List<GameObject> spawnPoints = GetSpawnPoints(direction);
        if (spawnPoints == null) yield break;

        for (int i = firstParam; i < secondParam; i = isNegative ? i - 1 : i + 1)
        {
            if (i < 0 || i >= spawnPoints.Count) continue;

            GameObject spawnPointPos = spawnPoints[i];

            GameObject spawningObject = spawnObject[Random.Range(0, spawnObject.Count)];

            Vector3 newPos = spawnPointPos.transform.position;
            newPos.y = spawnPointY;

            GameObject TrailClone = GameObject.Instantiate(spawningObject, newPos, spawnPointPos.transform.rotation * Quaternion.Euler(0, 270, 0));

            TrailClone.transform.SetParent(ParentObject.transform);

            TrailClone.transform.localScale = new Vector3(TrailClone.transform.localScale.x * boss.shrinkageRate, 
                                                          TrailClone.transform.localScale.y * boss.shrinkageRate, 
                                                          TrailClone.transform.localScale.z * boss.shrinkageRate);
            TrailClone.GetComponentInChildren<Light>().color = DecideColor(spawnPointY);
            yield return new WaitForSeconds(0.02f);
        }
    }

    private List<GameObject> GetSpawnPoints(string direction)
    {
        switch (direction.ToLower())
        {
            case "top":
                return topSpawnPoints;
            case "bottom":
                return bottomSpawnPoints;
            case "left":
                return leftSpawnPoints;
            case "right":
                return rightSpawnPoints;
            default:
                Debug.LogError("Invalid direction specified");
                return null;
        }
    }

    public Color DecideColor(float spawnPointY)
    {
        Color orange = new Color(1, 0.5f, 0);

        if (spawnPointY == spownPointTopY)
        {
            return Color.red;
        }
        else if (spawnPointY == spownPointMidY)
        {
            return orange;
        }
        else if (spawnPointY == spownPointDownY)
        {
            return Color.yellow;
        }
        else
        {
            return Color.yellow;
        }
    }

    public void PlayEffectMusic()
    {
        boss.GetComponent<PlayMusicBoss>().SpawnMusic();
    }
}
