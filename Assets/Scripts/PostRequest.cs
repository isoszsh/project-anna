using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class PostRequest : MonoBehaviour
{
    [System.Serializable]
    public class DecisionData
    {
        public string levelName;
        public int decisionData;
    }




    public void SaveDecision(string level, int decision)
    {
        DecisionData data = new DecisionData();
        data.levelName = level;
        data.decisionData = decision;

        // JSON verisini string format�na d�n��t�r
        string jsonData = JsonUtility.ToJson(data);
        StartCoroutine(PostRequestCoroutine("https://isoszsh.com/decision.php", jsonData));
    }
    IEnumerator PostRequestCoroutine(string url, string json)
    {
        // JSON verisini i�eren UnityWebRequest olu�tur
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // �stek g�nder ve yan�t� bekle
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Form upload complete!");
            Debug.Log("Response: " + request.downloadHandler.text);
        }
        else
        {
            Debug.Log("Error: " + request.error);
        }
    }
}
