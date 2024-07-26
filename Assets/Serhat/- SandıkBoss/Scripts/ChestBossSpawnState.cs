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

    public float trailMinSize = 0.5f;
    public float trailMaxSize = 1.5f;


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

        boss.StartCoroutine(RandomEvent());
    }

    public override void Perform()
    {

    }

    public override void Exit()
    {
        
    }

    private IEnumerator RandomEvent()
    {
        while (true)
        {
            yield return new WaitForSeconds(15);
            //random bir sayı salla 1 ise last4Spawn çalışsın 2 ise SpiralSpawn

            int randomEvent = Random.Range(0,2);
            if(randomEvent == 0){
                boss.StartCoroutine(Last4Spawn());
            }
            else if(randomEvent == 1){
                boss.StartCoroutine(SpiralSpawn());
            }
            else if(randomEvent == 2){
                boss.StartCoroutine(AllSpawn());
            }
        }
    }

    private IEnumerator Last4Spawn()
    {
        UltiTopSameTimeSpawn(0, (int)topSpawnPoints.Count/2, false);
        yield return new WaitForSeconds(1f);
        UltiRightSameTimeSpawn(0, (int)rightSpawnPoints.Count/2, false);
        yield return new WaitForSeconds(1f);
        UltiBottomSameTimeSpawn(0, (int)bottomSpawnPoints.Count/2, false);
        yield return new WaitForSeconds(1f);
        UltiLeftSameTimeSpawn(0, (int)leftSpawnPoints.Count/2, false);
    }

    private IEnumerator First4Spawn()
    {
        UltiTopSameTimeSpawn(0, (int)topSpawnPoints.Count/2, true);
        yield return new WaitForSeconds(1f);
        UltiRightSameTimeSpawn(0, (int)rightSpawnPoints.Count/2, true);
        yield return new WaitForSeconds(1f);
        UltiBottomSameTimeSpawn(0, (int)bottomSpawnPoints.Count/2, true);
        yield return new WaitForSeconds(1f);
        UltiLeftSameTimeSpawn(0, (int)leftSpawnPoints.Count/2, true);
    }

    private IEnumerator AllSpawn()
    {
        UltiTopSameTimeSpawn(0, (int)topSpawnPoints.Count, false);
        yield return new WaitForSeconds(1f);
        UltiRightSameTimeSpawn(0, (int)rightSpawnPoints.Count, false);
        yield return new WaitForSeconds(1f);
        UltiBottomSameTimeSpawn(0, (int)bottomSpawnPoints.Count, false);
        yield return new WaitForSeconds(1f);
        UltiLeftSameTimeSpawn(0, (int)leftSpawnPoints.Count, false);
    }

    private IEnumerator SpiralSpawn()
    {
        boss.StartCoroutine(RespectivelyTopSpawn(0, (int)topSpawnPoints.Count, false));
        yield return new WaitForSeconds(3.5f);
        boss.StartCoroutine(RespectivelyRightSpawn(0, (int)rightSpawnPoints.Count, false));
        yield return new WaitForSeconds(3.5f);
        boss.StartCoroutine(RespectivelyBottomSpawn(0, (int)bottomSpawnPoints.Count, false));
        yield return new WaitForSeconds(3.5f);
        boss.StartCoroutine(RespectivelyLeftSpawn(0, (int)leftSpawnPoints.Count, false));
    }

    private IEnumerator RespectivelyTopSpawn(int firstParam, int secondParam, bool isNegative)
    {
        for (int i = firstParam; i < secondParam; i = isNegative ? i - 1 : i + 1)
        {
            GameObject topSpawnPointsPos = topSpawnPoints[i];

            GameObject spawningObject = spawnObject[Random.Range(0, spawnObject.Count)];

            GameObject TrailClone = GameObject.Instantiate(spawningObject, topSpawnPointsPos.transform.position, topSpawnPointsPos.transform.rotation * Quaternion.Euler(0, 270, 0));

            TrailClone.transform.SetParent(trailParent.transform);

            float TrailSize = Random.Range(trailMinSize, trailMaxSize);
            TrailClone.transform.localScale = new Vector3(TrailSize, TrailSize, TrailSize);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator RespectivelyBottomSpawn(int firstParam, int secondParam, bool isNegative)
    {
        for (int i = firstParam; i < secondParam; i = isNegative ? i - 1 : i + 1)
        {
            GameObject bottomSpawnPointsPos = bottomSpawnPoints[i];

            GameObject spawningObject = spawnObject[Random.Range(0, spawnObject.Count)];

            GameObject TrailClone = GameObject.Instantiate(spawningObject, bottomSpawnPointsPos.transform.position, bottomSpawnPointsPos.transform.rotation * Quaternion.Euler(0, 270, 0));

            TrailClone.transform.SetParent(trailParent.transform);

            float TrailSize = Random.Range(trailMinSize, trailMaxSize);
            TrailClone.transform.localScale = new Vector3(TrailSize, TrailSize, TrailSize);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator RespectivelyLeftSpawn(int firstParam, int secondParam, bool isNegative)
    {
        for (int i = firstParam; i < secondParam; i = isNegative ? i - 1 : i + 1)
        {
            GameObject leftSpawnPointsPos = leftSpawnPoints[i];

            GameObject spawningObject = spawnObject[Random.Range(0, spawnObject.Count)];

            GameObject TrailClone = GameObject.Instantiate(spawningObject, leftSpawnPointsPos.transform.position, leftSpawnPointsPos.transform.rotation * Quaternion.Euler(0, 270, 0));

            TrailClone.transform.SetParent(trailParent.transform);

            float TrailSize = Random.Range(trailMinSize, trailMaxSize);
            TrailClone.transform.localScale = new Vector3(TrailSize, TrailSize, TrailSize);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator RespectivelyRightSpawn(int firstParam, int secondParam, bool isNegative)
    {
        for (int i = firstParam; i < secondParam; i = isNegative ? i - 1 : i + 1)
        {
            GameObject rightSpawnPointsPos = rightSpawnPoints[i];

            GameObject spawningObject = spawnObject[Random.Range(0, spawnObject.Count)];

            GameObject TrailClone = GameObject.Instantiate(spawningObject, rightSpawnPointsPos.transform.position, rightSpawnPointsPos.transform.rotation * Quaternion.Euler(0, 270, 0));

            TrailClone.transform.SetParent(trailParent.transform);

            float TrailSize = Random.Range(trailMinSize, trailMaxSize);
            TrailClone.transform.localScale = new Vector3(TrailSize, TrailSize, TrailSize);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void UltiTopSameTimeSpawn(int firstParam, int secondParam, bool isNegative)
    {
        for (int i = firstParam; i < secondParam; i = isNegative ? i - 1 : i + 1)
        {
            GameObject topSpawnPointsPos = topSpawnPoints[i];

            GameObject spawningObject = spawnObject[Random.Range(0, spawnObject.Count)];

            GameObject TrailClone = GameObject.Instantiate(spawningObject, topSpawnPointsPos.transform.position, topSpawnPointsPos.transform.rotation * Quaternion.Euler(0, 270, 0));

            TrailClone.transform.SetParent(trailParent.transform);

            float TrailSize = Random.Range(trailMinSize, trailMaxSize);
            TrailClone.transform.localScale = new Vector3(TrailSize, TrailSize, TrailSize);
        }
    }

    public void UltiBottomSameTimeSpawn(int firstParam, int secondParam, bool isNegative)
    {
        for (int i = firstParam; i < secondParam; i = isNegative ? i - 1 : i + 1)
        {
            GameObject bottomSpawnPointsPos = bottomSpawnPoints[i];

            GameObject spawningObject = spawnObject[Random.Range(0, spawnObject.Count)];

            GameObject TrailClone = GameObject.Instantiate(spawningObject, bottomSpawnPointsPos.transform.position, bottomSpawnPointsPos.transform.rotation * Quaternion.Euler(0, 270, 0));

            TrailClone.transform.SetParent(trailParent.transform);

            float TrailSize = Random.Range(trailMinSize, trailMaxSize);
            TrailClone.transform.localScale = new Vector3(TrailSize, TrailSize, TrailSize);
        }
    }

    public void UltiLeftSameTimeSpawn(int firstParam, int secondParam, bool isNegative)
    {
        for (int i = firstParam; i < secondParam; i = isNegative ? i - 1 : i + 1)
        {
            GameObject leftSpawnPointsPos = leftSpawnPoints[i];

            GameObject spawningObject = spawnObject[Random.Range(0, spawnObject.Count)];

            GameObject TrailClone = GameObject.Instantiate(spawningObject, leftSpawnPointsPos.transform.position, leftSpawnPointsPos.transform.rotation * Quaternion.Euler(0, 270, 0));

            TrailClone.transform.SetParent(trailParent.transform);

            float TrailSize = Random.Range(trailMinSize, trailMaxSize);
            TrailClone.transform.localScale = new Vector3(TrailSize, TrailSize, TrailSize);
        }
    }

    public void UltiRightSameTimeSpawn(int firstParam, int secondParam, bool isNegative)
    {
        for (int i = firstParam; i < secondParam; i = isNegative ? i - 1 : i + 1)
        {
            GameObject rightSpawnPointsPos = rightSpawnPoints[i];

            GameObject spawningObject = spawnObject[Random.Range(0, spawnObject.Count)];

            GameObject TrailClone = GameObject.Instantiate(spawningObject, rightSpawnPointsPos.transform.position, rightSpawnPointsPos.transform.rotation * Quaternion.Euler(0, 270, 0));

            TrailClone.transform.SetParent(trailParent.transform);

            float TrailSize = Random.Range(trailMinSize, trailMaxSize);
            TrailClone.transform.localScale = new Vector3(TrailSize, TrailSize, TrailSize);
        }
    }
}
