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
            // E�er instance hen�z atanmam��sa, bul ve ata
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();

                // E�er sahnede GameManager yoksa, hata ver ve konsolda uyar� g�ster
                if (instance == null)
                {
                    Debug.LogError("GameManager script'i sahnede bulunamad�!");
                }
            }
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Singleton instance'� bu GameManager nesnesine ata
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            // E�er singleton instance zaten atanm��sa ve bu nesne farkl� bir GameManager ise, bu nesneyi yok et
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        // levelNameText �zerindeki Animator bile�enini al
        levelNameAnimator = levelNameText.GetComponent<Animator>();
        levelNameAnimator.SetTrigger("LevelName");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
