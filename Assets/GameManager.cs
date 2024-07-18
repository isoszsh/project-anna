using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    private static GameManager instance;

    public TextMeshProUGUI levelNameText;
    private Animator levelNameAnimator;

    public CinemachineVirtualCamera virtualCamera;
    public PlayerController playerController;
    public GameObject butterfly;
    public AudioSource StoryAudioSource;

    public GameObject holePrefab;

    public GameObject startButton;

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



    public void Play()
    {
        Animation anim = butterfly.GetComponent<Animation>();
        anim["Butterfly_Fly"].speed = 1f;
        anim.Play("Butterfly_Fly");
        GameObject butterflyParent = butterfly.transform.parent.gameObject;

        butterflyParent.GetComponent<Animator>().SetTrigger("Play");
        StartCoroutine(Story());

        //startbutton'un görünürlüğünü kapat
        startButton.SetActive(false);
        

        
    }


    IEnumerator Story()
    {
        yield return new WaitForSeconds(1);
        StoryAudioSource.Play();
        yield return new WaitForSeconds(40);
        levelNameAnimator = levelNameText.GetComponent<Animator>();
        levelNameAnimator.SetTrigger("LevelName");
        playerController.enabled = true;
        playerController.playerAnimator.SetTrigger("Play");
        
    }
    // Start is called before the first frame update
    void Start()
    {

        if(SceneManager.GetActiveScene().name == "Level1")
        {
            playerController.enabled = false;
        }
       
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
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
