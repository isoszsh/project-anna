using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyesController : MonoBehaviour
{

    private Material faceMaterial;
    private bool isBlinking;
    public Texture[] faceTextures;

    // Start is called before the first frame update
    void Start()
    {
        faceMaterial = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isBlinking)
        {
            StartCoroutine(Blink());
        }
    }

    IEnumerator Blink()
    {
        isBlinking = true;
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 2f));
        faceMaterial.SetTexture("_BaseMap", faceTextures[0]);
        faceMaterial.SetTexture("_EmissionMap", faceTextures[0]);
        yield return new WaitForSeconds(.1f);
        faceMaterial.SetTexture("_BaseMap", faceTextures[1]);
        faceMaterial.SetTexture("_EmissionMap", faceTextures[1]);
        yield return new WaitForSeconds(.1f);
        faceMaterial.SetTexture("_BaseMap", faceTextures[2]);
        faceMaterial.SetTexture("_EmissionMap", faceTextures[2]);
        yield return new WaitForSeconds(.1f);
        faceMaterial.SetTexture("_BaseMap", faceTextures[3]);
        faceMaterial.SetTexture("_EmissionMap", faceTextures[3]);
        yield return new WaitForSeconds(.3f);
        faceMaterial.SetTexture("_BaseMap", faceTextures[2]);
        faceMaterial.SetTexture("_EmissionMap", faceTextures[2]);
        yield return new WaitForSeconds(.1f);
        faceMaterial.SetTexture("_BaseMap", faceTextures[1]);
        faceMaterial.SetTexture("_EmissionMap", faceTextures[1]);
        yield return new WaitForSeconds(.1f);
        faceMaterial.SetTexture("_BaseMap", faceTextures[0]);
        faceMaterial.SetTexture("_EmissionMap", faceTextures[0]);
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f,2f));
        isBlinking = false;
    }
}
