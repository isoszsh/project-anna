using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChestAiEnemy : MonoBehaviour
{
    private ChestAiStateMachine stateMachine;
    private NavMeshAgent agent;
    
    public NavMeshAgent Agent { get => agent; }

    [SerializeField]
    private string currentState;
    public Path path;

    // Start is called before the first frame update
    void Start()
    {
        stateMachine = GetComponent<ChestAiStateMachine>();
        if (stateMachine == null)
        {
            Debug.LogError("ChestAiStateMachine component is missing.");
        }

        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component is missing.");
        }

        stateMachine.Initialise();
    }

    public void ChangeStateToFoundState(GameObject target)
    {
        stateMachine.Initialise2(target);
    }
}
