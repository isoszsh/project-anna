using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepSoundsController : MonoBehaviour
{

    public PlayerController controller;

    public void PlayStepSound()
    {
        controller.HandleFootsteps();
    }
}
