using UnityEngine;

public class NoteActivator : MonoBehaviour
{
    public KeyCode keyToPress;
    public float pressWindow = 0.4f; // Basma penceresi zamaný
    public GameObject currentObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Note"))
        {
            currentObject = other.gameObject;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(keyToPress) && currentObject != null)
        {
            Destroy(currentObject);
            currentObject = null;
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
