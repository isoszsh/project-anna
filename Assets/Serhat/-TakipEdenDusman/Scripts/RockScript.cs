using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockScript : MonoBehaviour
{
    public List<GameObject> enemyList = new List<GameObject>();
    public SphereCollider sphereCollider;

    public bool isTriggered = false;

    private void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        isTriggered = false;    
    }

    private void Update()
    {
        CheckEnemiesInSphere();
    }

    private void CheckEnemiesInSphere()
    {
        if(isTriggered == false)
        {
            Collider[] hitColliders = Physics.OverlapSphere(sphereCollider.transform.position, sphereCollider.radius * 10);

            foreach (Collider collider in hitColliders)
            {
                if (enemyList.Contains(collider.gameObject))
                {
                    Debug.Log("An enemy is within the sphere collider.");
                    isTriggered = true;
                    collider.gameObject.GetComponent<ChestAiEnemy>().ChangeStateToCantRockState(this.gameObject);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (sphereCollider != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(sphereCollider.transform.position, sphereCollider.radius * 10);
        }
    }
}