using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyAnimationSpeed : MonoBehaviour
{

    private Animation anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animation>();

        // Animasyonun var olup olmadýðýný kontrol edin
        if (anim == null)
        {
            Debug.LogError("Animation component not found!");
            return;
        }

        // Animasyon klibinin var olup olmadýðýný kontrol edin
        if (anim["Butterfly_Land"] == null)
        {
            Debug.LogError("Animation clip 'Butterfly_land' not found!");
            return;
        }

        // Hýzý ayarlayýn
        anim["Butterfly_Land"].speed = 0.025f;

        // Animasyonu oynatýn (gerekirse)
        anim.Play("Butterfly_Land");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
