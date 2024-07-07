using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    private static GameManager instance;

    public TextMeshProUGUI levelNameText;
    private Animator levelNameAnimator;

    public CinemachineVirtualCamera virtualCamera;

    // Singleton instance property
    public static GameManager Instance
    {
        get
        {
            // Eðer instance henüz atanmamýþsa, bul ve ata
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();

                // Eðer sahnede GameManager yoksa, hata ver ve konsolda uyarý göster
                if (instance == null)
                {
                    Debug.LogError("GameManager script'i sahnede bulunamadý!");
                }
            }
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Singleton instance'ý bu GameManager nesnesine ata
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            // Eðer singleton instance zaten atanmýþsa ve bu nesne farklý bir GameManager ise, bu nesneyi yok et
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        // levelNameText üzerindeki Animator bileþenini al
        levelNameAnimator = levelNameText.GetComponent<Animator>();
        levelNameAnimator.SetTrigger("LevelName");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
