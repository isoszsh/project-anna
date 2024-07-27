using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChestBossEnemy : MonoBehaviour
{
    public GameObject player;
    public BoxCollider boxCollider;
    private ChestBossStateMachine stateMachine;
    private NavMeshAgent agent;
    
    public NavMeshAgent Agent { get => agent; }

    [SerializeField]
    private string currentState;

    public List<GameObject> walls = new List<GameObject>();

    public GameObject TrailParent;

    public List<GameObject> topSpawnPoints = new List<GameObject>();
    public List<GameObject> bottomSpawnPoints = new List<GameObject>();
    public List<GameObject> leftSpawnPoints = new List<GameObject>();
    public List<GameObject> rightSpawnPoints = new List<GameObject>();

    public GameObject spawnPointLookPosition;

    public List<GameObject> spawnObject = new List<GameObject>();

    public GameObject lavaWall;

    public GameObject allSpawnPointParentObject;

    public GameObject spawnPointTop;
    public GameObject spawnPointMid;
    public GameObject spawnPointDown;


    public GameObject spawnerHolder1;
    public GameObject spawnerHolder2;
    public GameObject spawnerHolder3;

    public GameObject bossLookPosition;

    // Start is called before the first frame update
    void Start()
    {
        LookAwayFrom(spawnPointLookPosition.transform, topSpawnPoints);
        LookAwayFrom(spawnPointLookPosition.transform, bottomSpawnPoints);
        LookAwayFrom(spawnPointLookPosition.transform, leftSpawnPoints);
        LookAwayFrom(spawnPointLookPosition.transform, rightSpawnPoints);


        stateMachine = GetComponent<ChestBossStateMachine>();
        agent = GetComponent<NavMeshAgent>();
        
        // tüm spawn pointler 
        
        
        stateMachine.Initialise();
    }


     void LookAwayFrom(Transform target, List<GameObject> points)
    {
        foreach (var point in points)
        {
            Vector3 directionToTarget = target.position - point.transform.position;
            Vector3 oppositeDirection = -directionToTarget;
            point.transform.rotation = Quaternion.LookRotation(oppositeDirection);
        }
    }
}
