using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChestBossEnemy : MonoBehaviour
{
    public GameObject player;
    private ChestBossStateMachine stateMachine;
    private NavMeshAgent agent;
    
    public NavMeshAgent Agent { get => agent; }

    [SerializeField]
    private string currentState;

    public List<GameObject> walls = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        stateMachine = GetComponent<ChestBossStateMachine>();
        agent = GetComponent<NavMeshAgent>();
        stateMachine.Initialise();
    }
}
