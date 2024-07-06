using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseController : MonoBehaviour
{

    public GameObject[] nonVisionPanels;
    public GameObject[] collidersToClose;
    public GameObject colliders;

    public Transform door;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") )
        {
            colliders.SetActive(true);
            foreach (var item in nonVisionPanels)
            {
                Renderer renderer = item.GetComponent<Renderer>();
                renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;

            }

            foreach (var item in collidersToClose)
            {
                item.GetComponent<MeshCollider>().enabled = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            colliders.SetActive(false);
            foreach (var item in nonVisionPanels)
            {
                Renderer renderer = item.GetComponent<Renderer>();
                renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            }

            foreach (var item in collidersToClose)
            {
                item.GetComponent<MeshCollider>().enabled = true;
            }
        }
    }
}
