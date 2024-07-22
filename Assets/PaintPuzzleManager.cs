using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaintPuzzleManager : MonoBehaviour
{

    public Material sunPlatformMaterial;
    public Material starPlatformMaterial;
    public Material crescentPlatformMaterial;
    public Material cloudPlatformMaterial;

    public Color sunReqColor;
    public Color starReqColor;
    public Color crescentReqColor;
    public Color cloudReqColor;

    public bool sunSolved;
    public bool starSolved;
    public bool crescentSolved;
    public bool cloudSolved;

    public Camera puzzleCamera;
    public Camera mainCamera;

    public Image golemDcImage;
    public Sprite golemDcSprite;


    public DialogueStarter golemDS;
    public DialogueData golemDSData;

    public GameObject maleGolem;
    public GameObject maleGolemDC;


    private void Start()
    {
        sunPlatformMaterial.color = Color.white;
        starPlatformMaterial.color = Color.white;
        crescentPlatformMaterial.color = Color.white;
        cloudPlatformMaterial.color = Color.white;
    }


    public void SetColor(Color color,string platform)
    {

        if(platform == "top")
        {

            cloudReqColor = RoundColor(cloudReqColor, 2);
            color = RoundColor(color, 2);

            Debug.Log(cloudReqColor);
            Debug.Log(color);

            if (color == cloudReqColor)
            {
                cloudPlatformMaterial.color = color;
                cloudSolved = true;
            }
            else
            {
                cloudPlatformMaterial.color = Color.white;
                cloudSolved= false;
            }
        }
        else if(platform == "right")
        {

            crescentReqColor = RoundColor(crescentReqColor, 2);
            color = RoundColor(color, 2);

            Debug.Log(crescentReqColor);
            Debug.Log(color);


            if (color == crescentReqColor)
            {
                crescentPlatformMaterial.color = color;
                crescentSolved = true;
            }
            else
            {
                crescentPlatformMaterial.color = Color.white;
                crescentSolved= false;
            }
        }
        else if (platform == "bottom")
        {
            starReqColor = RoundColor(starReqColor, 2);
            color  = RoundColor(color, 2);

            Debug.Log(starReqColor);
            Debug.Log(color);

            if (color == starReqColor)
            {
                starPlatformMaterial.color = color;
                starSolved = true;
            }
            else
            {
                starPlatformMaterial.color = Color.white;
                starSolved= false;
            }
        }
        else if (platform == "left")
        {
            sunReqColor = RoundColor(sunReqColor, 2);
            color = RoundColor(color, 2);

            Debug.Log(sunReqColor);
            Debug.Log(color);


            if (color == sunReqColor)
            {
                sunPlatformMaterial.color = color;
                sunSolved = true;
            }
            else
            {
                sunPlatformMaterial.color = Color.white;
                sunSolved= false;
            }
        }


        if(sunSolved && cloudSolved && crescentSolved && starSolved)
        {
            StartCoroutine(PuzzleSolved());
        }

    }


    IEnumerator PuzzleSolved()
    {
        GameManager.Instance.playerController.lockControls = true;
        mainCamera.gameObject.SetActive(false);
        puzzleCamera.gameObject.SetActive(true);
        puzzleCamera.GetComponent<Animator>().SetTrigger("Finish");
        GameManager.Instance.playerController.RemovePickupItem();
        golemDcImage.sprite = golemDcSprite;
        golemDS.dialogueData = golemDSData;
        yield return new WaitForSeconds(17f);
        mainCamera.gameObject.SetActive(true);
        puzzleCamera.gameObject.SetActive(false);
        Destroy(maleGolemDC);
        Destroy(maleGolem);
        yield return null;
    }

    Color RoundColor(Color color, int decimalPlaces)
    {
        float factor = Mathf.Pow(10, decimalPlaces);
        color.r = Mathf.Round(color.r * factor) / factor;
        color.g = Mathf.Round(color.g * factor) / factor;
        color.b = Mathf.Round(color.b * factor) / factor;
        color.a = Mathf.Round(color.a * factor) / factor;
        return color;
    }
}
