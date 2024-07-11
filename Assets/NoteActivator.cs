using UnityEngine;

public class NoteActivator : MonoBehaviour
{
    public KeyCode keyToPress;
    public float pressWindow = 0.4f; // Basma penceresi zamaný

    public GameObject currentObject;
    private bool canPress = false;

    void Update()
    {
        if (canPress && Input.GetKeyDown(keyToPress))
        {
            // Tuþa basýldý ve notayý yok et
            Destroy(currentObject);
            Debug.Log("Bastýnýz!");
            canPress = false; // Tekrar basýlmayacak
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (currentObject == null)
        {
            currentObject = other.gameObject;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Note")
        {
            // Basma penceresini aç
            canPress = true;
            Invoke("ResetCanPress", pressWindow); // Basma penceresini sýfýrla
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Note")
        {
            // Notanýn çýkmasýný yok et

            Destroy(currentObject);
            currentObject = null;
            canPress = false; // Tekrar basýlmayacak
        }
    }

    void ResetCanPress()
    {
        canPress = false; // Basma penceresini sýfýrla
    }
}
