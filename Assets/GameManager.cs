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



    public void Play()
    {
        Animation anim = butterfly.GetComponent<Animation>();
        anim["Butterfly_Fly"].speed = 1f;
        anim.Play("Butterfly_Fly");
        GameObject butterflyParent = butterfly.transform.parent.gameObject;

        butterflyParent.GetComponent<Animator>().SetTrigger("Play");
        StartCoroutine(Story());

        
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
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
