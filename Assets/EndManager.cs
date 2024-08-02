using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndManager : MonoBehaviour
{

    public GameObject butterFly;

    public GameObject endPanel;
    public GameObject title;
    public GameObject fto;

    private string message = "After the accident, which marked the beginning of everything, her mother came to the poignant realization that her greatest masterpiece was still her living daughter. In the midst of her grief and reflection, she composed this song for her, a haunting tribute to the love that endures through their shared trials and tribulations.";
    public float typingSpeed = 0.05f; // Hýz ayarý

    public TextMeshProUGUI textComponent;


    private const string url = "https://isoszsh.com/get_decision.php";

    public Slider[] levelDecision1To2Sliders; 
    public Slider[] levelDecision2To1Sliders;


    public GameObject level1Data;
    public GameObject level2Data;
    public GameObject level3Data;
    public GameObject level4Data;
    public GameObject level5Data;

    public GameObject msgGO;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FetchAllDecisions());

    }

    private IEnumerator FetchAllDecisions()
    {
        // Her level için GET isteði yap
        for (int i = 1; i <= 5; i++)
        {
            string levelName = "Level_" + i;
            string requestUrl = url + "?level=" + UnityWebRequest.EscapeURL(levelName);

            using (UnityWebRequest request = UnityWebRequest.Get(requestUrl))
            {
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Error fetching " + levelName + ": " + request.error);
                }
                else
                {
                    // JSON yanýtýný al
                    string jsonResponse = request.downloadHandler.text;
                    DecisionData data = JsonUtility.FromJson<DecisionData>(jsonResponse);

                    // Toplam seçim sayýsýný hesapla
                    int totalDecisions = data.decision_1 + data.decision_2;

                    // Oranlarý hesapla
                    float decision1To2Ratio = totalDecisions == 0 ? 0 : (float)data.decision_1 / totalDecisions * 100;
                    float decision2To1Ratio = totalDecisions == 0 ? 0 : (float)data.decision_2 / totalDecisions * 100;

                    // Slider'larý güncelle
                    if (i - 1 < levelDecision1To2Sliders.Length && i - 1 < levelDecision2To1Sliders.Length)
                    {
                        levelDecision1To2Sliders[i - 1].value = Mathf.Clamp(decision1To2Ratio, 0, 100); // 0-100 arasý deðer
                        levelDecision2To1Sliders[i - 1].value = Mathf.Clamp(decision2To1Ratio, 0, 100); // 0-100 arasý deðer
                    }
                }
            }
        }

        StartCoroutine(EndRoutine());
    }

    [System.Serializable]
    public class DecisionData
    {
        public string level;
        public int decision_1;
        public int decision_2;
    }




    IEnumerator EndRoutine()
    {
        yield return new WaitForSeconds(58);
        butterFly.SetActive(true);
        yield return new WaitForSeconds(2);
        endPanel.SetActive(true);
        yield return new WaitForSeconds(3);
        title.SetActive(true);
        yield return new WaitForSeconds(2f);
        textComponent.text = ""; 
        foreach (char letter in message.ToCharArray())
        {
            textComponent.text += letter; 
            yield return new WaitForSeconds(typingSpeed); 
        }
        yield return new WaitForSeconds(2);
        msgGO.SetActive(false);
        level1Data.SetActive(true);
        yield return new WaitForSeconds(5);
        level1Data.SetActive(false);
        yield return new WaitForSeconds(2);
        fto.SetActive(true);
        level2Data.SetActive(true);
        yield return new WaitForSeconds(5);
        level2Data.SetActive(false);
        yield return new WaitForSeconds(2);
        level3Data.SetActive(true);
        yield return new WaitForSeconds(5);
        level3Data.SetActive(false);
        yield return new WaitForSeconds(2);
        level4Data.SetActive(true);
        yield return new WaitForSeconds(5);
        level4Data.SetActive(false);
        yield return new WaitForSeconds(2);
        level5Data.SetActive(true);
        yield return new WaitForSeconds(5);
        level5Data.SetActive(false);
        yield return new WaitForSeconds(2);
        yield return new WaitForSeconds(105);
        SceneManager.LoadSceneAsync("Main Menu");
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))   
        {
            SceneManager.LoadSceneAsync("Main Menu");
        }
    }
}
