using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundReflector : MonoBehaviour
{

   public GameObject scannerPrefab;
   public IEnumerator Reflect()
    {
        Vector3 instantiatePos = new Vector3(transform.position.x, 0, transform.position.z);
        GameObject ip = Instantiate(scannerPrefab, instantiatePos, transform.rotation);
        ip.transform.localScale = transform.localScale;
        yield return null;

    }
}
