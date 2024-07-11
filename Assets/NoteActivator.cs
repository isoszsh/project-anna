using UnityEngine;

public class NoteActivator : MonoBehaviour
{
    public KeyCode keyToPress;
    public float pressWindow = 0.4f; // Basma penceresi zaman�

    public GameObject currentObject;
    private bool canPress = false;

    void Update()
    {
        if (canPress && Input.GetKeyDown(keyToPress))
        {
            // Tu�a bas�ld� ve notay� yok et
            Destroy(currentObject);
            Debug.Log("Bast�n�z!");
            canPress = false; // Tekrar bas�lmayacak
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
            // Basma penceresini a�
            canPress = true;
            Invoke("ResetCanPress", pressWindow); // Basma penceresini s�f�rla
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Note")
        {
            // Notan�n ��kmas�n� yok et

            Destroy(currentObject);
            currentObject = null;
            canPress = false; // Tekrar bas�lmayacak
        }
    }

    void ResetCanPress()
    {
        canPress = false; // Basma penceresini s�f�rla
    }
}
