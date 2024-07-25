using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundReflector : MonoBehaviour
{

   public GameObject scannerPrefab;

    public bool readyToPick;


    private int digCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.playerController.currentReflector = this;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.playerController.currentReflector = null;
        }
    }
    public IEnumerator Reflect()
    {
        Vector3 instantiatePos = new Vector3(transform.position.x, 0, transform.position.z);
        GameObject ip = Instantiate(scannerPrefab, instantiatePos, transform.rotation);
        ip.transform.localScale = transform.localScale;
        yield return null;

    }


    public IEnumerator DigReflector()
    {
        if(digCount == 0)
        {
            yield return new WaitForSeconds(1f);
            Instantiate(GameManager.Instance.holePrefab, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
        }

        if (transform.position.y < 0f)
        {
            yield return new WaitForSeconds(2f);
            transform.position = new Vector3(transform.position.x,transform.position.y + .2f,transform.position.z);
            if(transform.position.y > 0.1f)
            {
                readyToPick = true;
            }
        }
        else
        {
            readyToPick = true;
        }
        digCount++;
    }
}
