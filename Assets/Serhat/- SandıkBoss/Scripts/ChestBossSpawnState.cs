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


        yield return Last4Spawn(boss.spawnerHolder1);
        yield return new WaitForSeconds(1f);
        yield return First4Spawn(boss.spawnerHolder2);
        yield return new WaitForSeconds(1f);
        yield return Last4Spawn(boss.spawnerHolder3);
        yield return new WaitForSeconds(2f);
        
        yield return AllSpawn();
        yield return new WaitForSeconds(1f);
        yield return SpiralSpawn();
        
        yield return new WaitForSeconds(4f);

        youShouldLook = false;
        
        bossStateMachine.stunNumber = 2;
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

        
        yield return AllSpawn();
        yield return new WaitForSeconds(0.5f);
        yield return SpiralSpawn();
        yield return ReversSpiralSpawn();
        yield return SpiralSpawn();
        yield return new WaitForSeconds(1f);
        yield return FastAnd3Time();
        
        yield return new WaitForSeconds(4f);

        youShouldLook = false;

        boss.Agent.isStopped = true;

        bossStateMachine.spawnNumber--;
        boss.GetComponent<ChestBossStateMachine>().GameLoop();

    }


    private IEnumerator FastAnd3Time()
    {
        UltiSameTimeSpawn(0, topSpawnPoints.Count, false, boss.spawnerHolder1 , "top");
        UltiSameTimeSpawn(0, bottomSpawnPoints.Count, false, boss.spawnerHolder1 , "bottom");
        yield return new WaitForSeconds(0.5f);
        UltiSameTimeSpawn(0, rightSpawnPoints.Count, false, boss.spawnerHolder1 , "right");
        UltiSameTimeSpawn(0, leftSpawnPoints.Count, false, boss.spawnerHolder1 , "left");
        MoveTowards(boss.spawnerHolder1, boss.spawnPointTop, 0.1f);

        
        yield return new WaitForSeconds(0.5f);
        UltiSameTimeSpawn(0, topSpawnPoints.Count, false, boss.spawnerHolder2 , "top");
        UltiSameTimeSpawn(0, bottomSpawnPoints.Count, false, boss.spawnerHolder2 , "bottom");
        yield return new WaitForSeconds(0.5f);
        UltiSameTimeSpawn(0, rightSpawnPoints.Count, false, boss.spawnerHolder2 , "right");
        UltiSameTimeSpawn(0, leftSpawnPoints.Count, false, boss.spawnerHolder2 , "left");
        SetAllSpawnPointParentObject(boss.spawnerHolder2, boss.spawnPointMid);
        MoveTowards(boss.spawnerHolder2, boss.spawnPointMid, 0.1f);

        
        yield return new WaitForSeconds(0.5f);
        UltiSameTimeSpawn(0, topSpawnPoints.Count, false, boss.spawnerHolder3 , "top");
        UltiSameTimeSpawn(0, bottomSpawnPoints.Count, false, boss.spawnerHolder3 , "bottom");
        yield return new WaitForSeconds(0.5f);
        UltiSameTimeSpawn(0, rightSpawnPoints.Count, false, boss.spawnerHolder3 , "right");
        UltiSameTimeSpawn(0, leftSpawnPoints.Count, false, boss.spawnerHolder3 , "left");
        MoveTowards(boss.spawnerHolder2, boss.spawnPointDown, 0.1f);
        yield return new WaitForSeconds(0.5f);
    }

    public void MoveTowards(GameObject movingObject, GameObject targetObject, float speed)
    {
        // Calculate the step size based on speed and time
        float step = speed * Time.deltaTime;
        
        // Move the movingObject towards the targetObject
        movingObject.transform.position = Vector3.MoveTowards(movingObject.transform.position, targetObject.transform.position, step);
    }

    private IEnumerator Last4Spawn(GameObject spanwerHolder)
    {
        UltiSameTimeSpawn(0, topSpawnPoints.Count / 2, false, spanwerHolder , "top");
        UltiSameTimeSpawn(0, rightSpawnPoints.Count / 2, false, spanwerHolder , "right");
        UltiSameTimeSpawn(0, bottomSpawnPoints.Count / 2, false, spanwerHolder , "bottom");
        UltiSameTimeSpawn(0, leftSpawnPoints.Count / 2, false, spanwerHolder , "left");
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator First4Spawn(GameObject spanwerHolder)
    {
        UltiSameTimeSpawn(topSpawnPoints.Count / 2, topSpawnPoints.Count, false, spanwerHolder , "top");
        UltiSameTimeSpawn(rightSpawnPoints.Count / 2, rightSpawnPoints.Count, false, spanwerHolder , "right");
        UltiSameTimeSpawn(bottomSpawnPoints.Count / 2, bottomSpawnPoints.Count, false, spanwerHolder , "bottom");
        UltiSameTimeSpawn(leftSpawnPoints.Count / 2, leftSpawnPoints.Count, false, spanwerHolder , "left");
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator AllSpawn()
    {
        UltiSameTimeSpawn(0, topSpawnPoints.Count, false, boss.spawnerHolder1 , "top");
        UltiSameTimeSpawn(0, rightSpawnPoints.Count, false, boss.spawnerHolder1 , "right");
        UltiSameTimeSpawn(0, bottomSpawnPoints.Count, false, boss.spawnerHolder1 , "bottom");
        UltiSameTimeSpawn(0, leftSpawnPoints.Count, false, boss.spawnerHolder1 , "left");
        yield return new WaitForSeconds(0.5f);  
    }

    private IEnumerator PartPartSpawn()
    {
        UltiSameTimeSpawn(0, topSpawnPoints.Count, false, boss.spawnerHolder1 , "top");
        UltiSameTimeSpawn(0, rightSpawnPoints.Count, false, boss.spawnerHolder1 , "right");
        yield return new WaitForSeconds(0.1f);
        UltiSameTimeSpawn(0, bottomSpawnPoints.Count, false, boss.spawnerHolder1 , "bottom");
        UltiSameTimeSpawn(0, leftSpawnPoints.Count, false, boss.spawnerHolder1 , "left");
        yield return new WaitForSeconds(0.5f);  
    }

    private IEnumerator SpiralSpawn()
    {
        yield return RespectivelySpawn(0, topSpawnPoints.Count,false, boss.spawnerHolder1 ,  "top");
        yield return RespectivelySpawn(0, leftSpawnPoints.Count,false, boss.spawnerHolder1 ,  "left");
        yield return RespectivelySpawn(0, bottomSpawnPoints.Count,false, boss.spawnerHolder1 ,  "bottom");
        yield return RespectivelySpawn(0, rightSpawnPoints.Count,false, boss.spawnerHolder1 ,  "right");
    }

    private IEnumerator ReversSpiralSpawn()
    {
        yield return RespectivelySpawn(topSpawnPoints.Count - 1, 0, true, boss.spawnerHolder1 , "top");
        yield return RespectivelySpawn(leftSpawnPoints.Count - 1, 0, true, boss.spawnerHolder1 , "left");
        yield return RespectivelySpawn(bottomSpawnPoints.Count - 1, 0, true, boss.spawnerHolder1 , "bottom");
        yield return RespectivelySpawn(rightSpawnPoints.Count - 1, 0, true, boss.spawnerHolder1 , "right");
    }


    public void UltiSameTimeSpawn(int firstParam, int secondParam, bool isNegative, GameObject ParentObject , string direction)
    {
        List<GameObject> spawnPoints = GetSpawnPoints(direction);
        if (spawnPoints == null) return;

        for (int i = firstParam; i < secondParam; i = isNegative ? i - 1 : i + 1)
        {
            if (i < 0 || i >= spawnPoints.Count) continue;

            GameObject spawnPointPos = spawnPoints[i];

            GameObject spawningObject = spawnObject[Random.Range(0, spawnObject.Count)];

            GameObject TrailClone = GameObject.Instantiate(spawningObject, spawnPointPos.transform.position, spawnPointPos.transform.rotation * Quaternion.Euler(0, 270, 0));

            TrailClone.transform.SetParent(ParentObject.transform);

            
            TrailClone.transform.localScale = new Vector3(TrailClone.transform.localScale.x * boss.shrinkageRate, 
                                                          TrailClone.transform.localScale.y * boss.shrinkageRate, 
                                                          TrailClone.transform.localScale.z * boss.shrinkageRate);
        }
    }

    public IEnumerator RespectivelySpawn(int firstParam, int secondParam, bool isNegative, GameObject ParentObject ,string direction)
    {
        List<GameObject> spawnPoints = GetSpawnPoints(direction);
        if (spawnPoints == null) yield break;

        for (int i = firstParam; i < secondParam; i = isNegative ? i - 1 : i + 1)
        {
            if (i < 0 || i >= spawnPoints.Count) continue;

            GameObject spawnPointPos = spawnPoints[i];

            GameObject spawningObject = spawnObject[Random.Range(0, spawnObject.Count)];

            GameObject TrailClone = GameObject.Instantiate(spawningObject, spawnPointPos.transform.position, spawnPointPos.transform.rotation * Quaternion.Euler(0, 270, 0));

            TrailClone.transform.SetParent(ParentObject.transform);

            TrailClone.transform.localScale = new Vector3(TrailClone.transform.localScale.x * boss.shrinkageRate, 
                                                          TrailClone.transform.localScale.y * boss.shrinkageRate, 
                                                          TrailClone.transform.localScale.z * boss.shrinkageRate);
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
}
