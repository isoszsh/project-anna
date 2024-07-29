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

    public GameObject visionCone;

    public Material redMaterial;

    public Material defouldMaterial;
    public Material yellowMaterial;

    // Start is called before the first frame update
    void Start()
    {
        stateMachine = GetComponent<ChestAiStateMachine>();
        agent = GetComponent<NavMeshAgent>();
        stateMachine.PatroleState();
    }

    public void ChangeStateToFoundState(GameObject target)
    {
        if(stateMachine.isPlayerFound == false){
            stateMachine.FoundPlayerState(target);
        }
    }

    public void ChangeStateToCantRockState(GameObject target)
    {
        stateMachine.RockState(target);
    }
}
