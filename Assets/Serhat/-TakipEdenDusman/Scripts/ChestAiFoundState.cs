using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestAiFoundState : ChestAiBaseState
{
    public GameObject target;

    public ChestAiFoundState(GameObject Target)
    {
        target = Target;
    }
    
    public override void Enter()
    {
        enemy.GetComponent<Animator>().ResetTrigger("Idle");
        enemy.GetComponent<Animator>().ResetTrigger("Walk");        
        enemy.GetComponent<Animator>().SetTrigger("Run");
        FoundTarget();
    }

    public override void Perform()
    {
        if(stateMachine.isPlayerFound == false)
        {
            FoundTarget();
            // Mesafe hesaplama
            float distance = Vector3.Distance(enemy.transform.position, target.transform.position);

            // Normalize mesafe 3 ile 5 arasında
            float minDistance = 1.5f;
            float maxDistance = 4f;
            float lerpValue = Mathf.InverseLerp(minDistance, maxDistance, distance);

            Color red = enemy.redMaterial.color;
            Color yellow = enemy.yellowMaterial.color;

            Color targetColor = Color.Lerp( enemy.redMaterial.color, enemy.yellowMaterial.color, lerpValue);

            // Yeni malzeme oluşturma ve rengini ayarlama
            Material newMat = new Material(enemy.visionCone.GetComponent<MeshRenderer>().material);
            newMat.color = targetColor;
            // Renk geçişi
            enemy.visionCone.GetComponent<MeshRenderer>().material = newMat;


            if (distance < 4.5 && enemy.visionCone.GetComponent<MeshRenderer>().material != enemy.defouldMaterial)
            {
                // look at yapmamız gerek
                enemy.StartCoroutine(SmoothLookAt(target.transform.position));

                enemy.visionCone.GetComponent<MeshRenderer>().material = enemy.redMaterial;

                enemy.Agent.SetDestination(enemy.transform.position);
                //yeni bir state oluşturulup bu state'e geçiş yapılabilir.

                stateMachine.EndState();
            }

            //eğer mat eski rengine gönerse 

            if (distance > 10)
            {
                enemy.GetComponent<Animator>().ResetTrigger("Run");
                stateMachine.CantFoundState();
            }
        }
    }   

public IEnumerator SmoothLookAt(Vector3 target)
{
    Vector3 direction = target - enemy.transform.position;
    Quaternion targetRotation = Quaternion.LookRotation(direction);

    float elapsedTime = 0f;
    float rotationTime = 1f;  // This value is increased to 1 second for smoother rotation

    while (elapsedTime < rotationTime)
    {
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, elapsedTime / rotationTime);
        elapsedTime += Time.deltaTime;
        yield return null;
    }

    // Ensure final rotation is set to the target rotation after the loop
    enemy.transform.rotation = targetRotation;
}


    public override void Exit()
    {
        enemy.GetComponent<Animator>().SetTrigger("Idle");
    }

    public void FoundTarget()
    {
        enemy.Agent.SetDestination(target.transform.position);
    }
}
