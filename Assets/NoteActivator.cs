using UnityEngine;

public class NoteActivator : MonoBehaviour
{
    public KeyCode keyToPress;
    public float pressWindow = 0.4f; // Basma penceresi zamaný
    public GameObject currentObject;
    public MeshRenderer mr;
    public Color ktpColor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Note"))
        {
            currentObject = other.gameObject;
        }
    }

    private void Update()
    {
        if (Input.GetKey(keyToPress))
        {
            if (currentObject != null )
            {
                Destroy(currentObject);
                currentObject = null;
            }

            mr.material.color = ktpColor;

        }
        else
        {
            if(mr.material.color != Color.white)
            {
                mr.material.color = Color.white;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Note"))
        {
                currentObject = null;
                Destroy(other.gameObject);
        }
    }
}
