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

    public PaintPuzzleManager paintPuzzleManager;

    public Transform annaHand;

    public GameObject introCam;
    public GameObject mainIntroCam;
    public GameObject playerCam;

    public GameObject butterflyVC;

    public GameObject mainButterfy;

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
        yield return new WaitForSeconds(49);
        playerController.playerAnimator.SetTrigger("Play");
        yield return new WaitForSeconds(8);

        mainIntroCam.gameObject.SetActive(false);
        introCam.gameObject.SetActive(true);
        introCam.GetComponent<Animator>().SetTrigger("Pan");
        butterflyVC.SetActive(false);
        yield return new WaitForSeconds(1);
        
        butterfly.transform.parent.transform.parent = annaHand;
        butterfly.transform.parent.GetComponent<Animator>().enabled = false;
        butterfly.transform.parent.transform.localPosition = new Vector3(0, 0, 0);
        
        
        yield return new WaitForSeconds(2);
        butterfly.gameObject.SetActive(false);
        mainButterfy.gameObject.SetActive(true);
        mainButterfy.transform.position = butterfly.transform.parent.transform.position;
        levelNameAnimator = levelNameText.GetComponent<Animator>();
        levelNameAnimator.SetTrigger("LevelName");

        playerController.enabled = true;
        introCam.gameObject.SetActive(false);
        mainIntroCam.gameObject.SetActive(false);
        playerCam.gameObject.SetActive(true);
        
        
    }
    // Start is called before the first frame update
    void Start()
    {
        if(StoryAudioSource != null)
        {
            StoryAudioSource.Play();
            StoryAudioSource.Pause();
        }
        
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            playerController.enabled = false;
            playerController.playerAnimator.SetTrigger("Sleep");
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
        
        
    }
}
