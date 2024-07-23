using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LizardEndTrigger : Event
{

    public DialogueStarter oopsieDS;
    public GameObject dialogueCanvas;
    // Start is called before the first frame update
    public override void TriggerStartEvent()
    {
       oopsieDS.readyToTalk = true;
       dialogueCanvas.SetActive(true);
    }
}
