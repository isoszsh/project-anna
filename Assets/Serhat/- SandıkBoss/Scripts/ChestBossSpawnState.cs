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

    // public float trailMinSize = 0.4f;
    // public float trailMaxSize = 0.6f;

    public Vector3 tempPos;

    public Vector3 lavaWallTempPos;


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

        boss.Agent.SetDestination(boss.allSpawnPointParentObject.transform.position);
        boss.GetComponent<Animator>().ResetTrigger("Run");
        boss.GetComponent<Animator>().SetTrigger("ReturnChest");

        boss.StartCoroutine(RandomEvent());
        
    }

    public override void Perform()
    {

    }

    public override void Exit()
    {
    }

    private IEnumerator LookAtTarget()
    {
        while (true)
        {
            Vector3 direction = boss.bossLookPosition.transform.position - boss.transform.position;
            direction.y = 0; // Keep only the horizontal direction
            boss.transform.rotation = Quaternion.Slerp(boss.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 20);
            
            if (boss.transform.rotation == boss.bossLookPosition.transform.rotation)
            {
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

    private IEnumerator RandomEvent()
    {

        yield return new WaitForSeconds(2f);
        boss.StartCoroutine(LookAtTarget());
        yield return new WaitForSeconds(2f);

        while (bossStateMachine.spawnNumber > 0)
        {
            int randomEvent = Random.Range(0, 5);

            switch (randomEvent)
            {
                case 0:
                    yield return Last4Spawn();
                    break;
                case 1:
                    yield return SpiralSpawn();
                    break;
                case 2:
                    yield return AllSpawn();
                    break;
                case 3:
                    yield return ReversSpiralSpawn();
                    break;
                case 4:
                    yield return FastAnd3Time();
                    break;
                case 5:
                    yield return PartPartSpawn();
                    break;
            }

            bossStateMachine.spawnNumber--;
            yield return new WaitForSeconds(4f);
        }

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

    private IEnumerator Last4Spawn()
    {
        UltiSameTimeSpawn(0, topSpawnPoints.Count / 2, false, boss.spawnerHolder1 , "top");
        UltiSameTimeSpawn(0, rightSpawnPoints.Count / 2, false, boss.spawnerHolder1 , "right");
        UltiSameTimeSpawn(0, bottomSpawnPoints.Count / 2, false, boss.spawnerHolder1 , "bottom");
        UltiSameTimeSpawn(0, leftSpawnPoints.Count / 2, false, boss.spawnerHolder1 , "left");
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

            // float TrailSize = Random.Range(trailMinSize, trailMaxSize);
            // TrailClone.transform.localScale = new Vector3(TrailSize, TrailSize, TrailSize);
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

            // float TrailSize = Random.Range(trailMinSize, trailMaxSize);
            // TrailClone.transform.localScale = new Vector3(TrailSize, TrailSize, TrailSize);
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
