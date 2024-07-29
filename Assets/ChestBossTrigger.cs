using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBossTrigger : MonoBehaviour
{
    
    public List<GameObject> outDoorWall;
    public List<GameObject> lights;
    public GameObject wallWithAnimation;

    public GameObject cameraOriginal;
    public GameObject cameraChestBoss;

    void Start()
    {
        if (outDoorWall == null || outDoorWall.Count == 0)
        {
            Debug.LogError("outDoorWall is not assigned or empty!");
        }

        if (wallWithAnimation == null)
        {
            Debug.LogError("wallWithAnimation is not assigned!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // eğer j harfine basılır sa
        if (Input.GetKeyDown(KeyCode.J))
        {
            foreach (var item in outDoorWall)
            {
                item.SetActive(true);
            }
            this.GetComponent<ChestBossStateMachine>().Initialise();
        }
        if(Input.GetKeyDown(KeyCode.K)){
            wallWithAnimation.GetComponent<Animator>().SetTrigger("Start");
        }
        if(Input.GetKeyDown(KeyCode.L)){
            cameraOriginal.SetActive(false);
            cameraChestBoss.SetActive(true);
        }
        if(Input.GetKeyDown(KeyCode.P)){
            StartCoroutine(LightOn());
        }
    }

    IEnumerator LightOn(){
        // light on
        lights[0].SetActive(true);
        lights[1].SetActive(true);
        yield return new WaitForSeconds(1f);
        lights[2].SetActive(true);
        lights[3].SetActive(true);
        yield return new WaitForSeconds(1f);
        lights[4].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        lights[5].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        lights[6].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        lights[7].SetActive(true);
    }


}
