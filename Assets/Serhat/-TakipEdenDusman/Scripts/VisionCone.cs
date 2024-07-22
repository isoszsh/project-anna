using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VisionCone : MonoBehaviour
{
    public Material VisionConeMaterial;
    public float VisionRange;
    public float VisionAngle;
    public LayerMask VisionObstructingLayer; //layer with objects that obstruct the enemy view, like walls, for example
    public int VisionConeResolution = 120; //the vision cone will be made up of triangles, the higher this value is the pretier the vision cone will be
    Mesh VisionConeMesh;
    MeshFilter MeshFilter_;

    public bool oneShote = true;
    public GameObject player;

    public GameObject anna;

    public GameObject target;

    void Start()
    {
        transform.AddComponent<MeshRenderer>().material = VisionConeMaterial;
        MeshFilter_ = transform.AddComponent<MeshFilter>();
        VisionConeMesh = new Mesh();
        VisionAngle *= Mathf.Deg2Rad;
    }

    void Update()
    {
        DrawVisionCone(); //calling the vision cone function everyframe just so the cone is updated every frame
    }

    void DrawVisionCone() //this method creates the vision cone mesh
    {
        int[] triangles = new int[(VisionConeResolution - 1) * 3];
        Vector3[] Vertices = new Vector3[VisionConeResolution + 1];
        Vertices[0] = Vector3.zero;
        float Currentangle = -VisionAngle / 2;
        float angleIcrement = VisionAngle / (VisionConeResolution - 1);
        float Sine;
        float Cosine;
        bool playerSeen = false;

        for (int i = 0; i < VisionConeResolution; i++)
        {
            Sine = Mathf.Sin(Currentangle);
            Cosine = Mathf.Cos(Currentangle);
            Vector3 RaycastDirection = (transform.forward * Cosine) + (transform.right * Sine);
            Vector3 VertForward = (Vector3.forward * Cosine) + (Vector3.right * Sine);
            if (Physics.Raycast(transform.position, RaycastDirection, out RaycastHit hit, VisionRange, VisionObstructingLayer))
            {
                Vertices[i + 1] = VertForward * hit.distance;

            }
            else
            {
                Vertices[i + 1] = VertForward * VisionRange;
            }

            // Eğer `anna` belirli bir layerda değilse, ikinci bir raycast yapın
            if (!playerSeen)
            {
                if (Physics.Raycast(transform.position, RaycastDirection, out RaycastHit hit2, VisionRange)) // İkinci Raycast için farklı bir isim kullanıyoruz
                {
                    if (hit2.collider.gameObject == anna)
                    {
                        playerSeen = true;
                        target = hit2.collider.gameObject;
                    }
                }
            }

            Currentangle += angleIcrement;
        }

        for (int i = 0, j = 0; i < triangles.Length; i += 3, j++)
        {
            triangles[i] = 0;
            triangles[i + 1] = j + 1;
            triangles[i + 2] = j + 2;
        }
        VisionConeMesh.Clear();
        VisionConeMesh.vertices = Vertices;
        VisionConeMesh.triangles = triangles;
        MeshFilter_.mesh = VisionConeMesh;

        // Player'ı gördüyse rengini sarıya ayarla
        if (playerSeen)
        {
            if (oneShote)
                OnPlayerSeen();
        }
        else
        {
            VisionConeMaterial.color = Color.white; // Orijinal rengi beyaz kabul ediyorum, isteğe bağlı değiştirilebilir
        }
    }

    public void OnPlayerSeen()
    {
        Debug.Log("Player is seen");
        oneShote = false;
        VisionConeMaterial.color = Color.yellow;
        player.GetComponent<ChestAiEnemy>().ChangeStateToFoundState(target);
    }
}
