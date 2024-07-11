using UnityEngine;

public class NoteMovement : MonoBehaviour
{
    public float speed = 5f;
    public MeshRenderer noteRenderer;
    public Material[] materials; // Bu materyaller Unity Edit�r'de atanmal�

    void Start()
    {
    }

    void Update()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime); // Negatif z ekseninde hareket
    }

    public void SetColor(string colorName)
    {
 

        // Renk ad�na g�re Material nesnesi olu�turma
        switch (colorName.ToLower())
        {
            case "red":
                noteRenderer.material = materials[0];
                break;
            case "green":
                noteRenderer.material = materials[1];
                break;
            case "blue":
                noteRenderer.material = materials[2];
                break;
            default:
                noteRenderer.material = materials[3];
                break;
        }
    }
}
