using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

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
    public GameObject mainVC;

    public GameObject mainButterfy;
    public GameObject hintPanel;

    public GameObject letterPanel;
    public TextMeshProUGUI letterText;

    public AudioSource letterSource;

    public bool letterOpened = false;

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
        mainVC.SetActive(false);
        

        
    }

    private void Update()
    {
        if (letterOpened && Input.anyKeyDown)
        {
            LetterStop();
        }
    }

    IEnumerator Story()
    {
        yield return new WaitForSeconds(1);
        StoryAudioSource.Play();
        yield return new WaitForSeconds(49);
        playerController.playerAnimator.SetTrigger("Play");
        yield return new WaitForSeconds(8);

        mainIntroCam.gameObject.GetComponent<Camera>().enabled = false;
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
        mainVC.gameObject.SetActive(true);
        yield return new WaitForSeconds(5);
        mainIntroCam.gameObject.SetActive(false);
        
        
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
            Play();
        }
        else
        {
            StartCoroutine(LNM());
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

    public void OpenLetter(string letter, AudioClip clip)
    {
        Debug.Log("letter acildi");
        letterPanel.SetActive(true);
        letterOpened = true;
        playerController.lockControls = true;
        playerController.ResetVelocity();
        if(clip != null)
        {
            letterSource.clip = clip;
            letterSource.Play();
        }


        StartCoroutine(TypeLetter(letter));
    }

    public void LetterStop()
    {
        playerController.lockControls = false;
        letterSource.Stop();
        StopAllCoroutines();
        letterText.text = "";
        letterPanel.SetActive(false);
        letterOpened = false;
    }


     IEnumerator TypeLetter(string letter)
    {
        letterText.text = ""; 

        foreach (char letterChar in letter)
        {
            letterText.text += letterChar; 
            yield return new WaitForSeconds(0.05f); 
        }
    }
    IEnumerator LNM()
    {
        yield return new WaitForSeconds(2);
        levelNameAnimator = levelNameText.GetComponent<Animator>();
        levelNameAnimator.SetTrigger("LevelName");
    }
}
