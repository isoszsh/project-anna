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
            Debug.Log("Note Entered: " + other.gameObject.name);
            currentObject = other.gameObject;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(keyToPress) && currentObject != null)
        {
            Destroy(currentObject);
            currentObject = null;
            Debug.Log("Key Pressed: " + keyToPress);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Note"))
        {
            Debug.Log("Note Exited: " + other.gameObject.name);
            if (currentObject == other.gameObject)
            {
                currentObject = null;
            }
        }
    }
}
