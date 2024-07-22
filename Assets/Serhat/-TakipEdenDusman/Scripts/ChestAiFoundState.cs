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
            FoundTarget();
    }

    public override void Perform()
    {
        FoundTarget();
        //enemy objesi target objesine yaklaştıkça rengini kırmızı yapar ve durur.

        // Mesafe hesaplama
        float distance = Vector3.Distance(enemy.transform.position, target.transform.position);
        float lerpValue = Mathf.InverseLerp(5, 8, distance);
        Color targetColor = Color.Lerp(Color.red, Color.yellow, lerpValue);

        // Debugging için log ekleyelim
        Debug.Log($"Distance: {distance}, Lerp Value: {lerpValue}, Target Color: {targetColor}");

        // Renk geçişi
        enemy.transform.GetChild(0).GetComponent<VisionCone>().VisionConeMaterial.color = targetColor;

        if (distance < 5)
        {
            enemy.transform.GetChild(0).GetComponent<VisionCone>().VisionConeMaterial.color = Color.red;
            enemy.Agent.SetDestination(enemy.transform.position);
            //yeni bir state oluşturulup bu state'e geçiş yapılabilir.

            stateMachine.Initialise3();
        }

    }   

    public override void Exit()
    {

    }

    public void FoundTarget()
    {
        enemy.Agent.SetDestination(target.transform.position);
    }
}
