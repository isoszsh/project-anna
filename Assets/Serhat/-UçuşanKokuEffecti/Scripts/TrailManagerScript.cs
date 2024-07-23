using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailManagerScript : MonoBehaviour
{
    public bool isTrailActive = true;

    public GameObject TargetGameObject;
    public float spawnMinTime = 1f;
    public float spawnMaxTime = 2f;

    public int TrailCountForOneShot = 3;

    public float TrailMinSize = 0.2f;
    public float TrailMaxSize = 0.5f;

    [SerializeField]
    private GameObject TrailParent;

    public List<GameObject> TrailSpawnPointList = new List<GameObject>();
    public List<GameObject> TrailList = new List<GameObject>();

    public void Start()
    {
        InitializeTrailChildeParentObject();
        LookAllTrailSpawnPointToTargetObject();
        StartCoroutine(SpawnTrailEveryXSeconds());
    }
    
    private IEnumerator SpawnTrailEveryXSeconds()
    {
        while (isTrailActive)
        {
            for (int i = 0; i < TrailCountForOneShot; i++)
                TrailSpawn();
            float spawnInterval = Random.Range(spawnMinTime, spawnMaxTime);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void InitializeTrailChildeParentObject()
    {
        // Create one empty GameObject to be the parent of all the trail objects
        TrailParent = new GameObject("TrailParent");
        // kendi childe objen yap
        TrailParent.transform.SetParent(this.transform);
    }

    public void TrailSpawn()
    {
        GameObject spawnPoint = TrailSpawnPointList[Random.Range(0, TrailSpawnPointList.Count)];

        GameObject Trail = TrailList[Random.Range(0, TrailList.Count)];

        GameObject TrailClone = Instantiate(Trail, spawnPoint.transform.position, spawnPoint.transform.rotation * Quaternion.Euler(0, 270, 0));

        TrailClone.transform.SetParent(TrailParent.transform); 

        float TrailSize = Random.Range(TrailMinSize, TrailMaxSize);
        TrailClone.transform.localScale = new Vector3(TrailSize, TrailSize, TrailSize);
    }
    

    public void LookAllTrailSpawnPointToTargetObject()
    {
        foreach (GameObject spawnPoint in TrailSpawnPointList)
        {
            // bakarken x kordinatları değişmesin 

            Vector3 targetPosition = new Vector3(TargetGameObject.transform.position.x, spawnPoint.transform.position.y, TargetGameObject.transform.position.z);

            spawnPoint.transform.LookAt(targetPosition);
            
        }
    }
}
