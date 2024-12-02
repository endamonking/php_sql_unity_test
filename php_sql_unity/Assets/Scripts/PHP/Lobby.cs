using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using System.Text;
using Unity.VisualScripting;


public class UserItem
{
    public int diamond;
    public int heart;
}

public class SendingID
{
    public int id;
    public int diamond;
}

public class Lobby : MonoBehaviour
{
    [SerializeField]
    private Slider healthSlider;
    [SerializeField]
    private TextMeshProUGUI gemText;

    private int heart, gem, thisUserID;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnterLobby(int userID)
    {
        thisUserID = userID;
        SendingID sendID = new SendingID
        {
            id = userID
        };

        string jsonData = JsonUtility.ToJson(sendID);
        StartCoroutine(PostRequest("https://test-piggy.codedefeat.com/worktest/dev06/php/Lobby.php", jsonData));
    }
    IEnumerator PostRequest(string url, string jsonData)
    {
        using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            // Send the request and wait for the response
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Response: " + webRequest.downloadHandler.text);

                if (UIManager.instance != null)
                {

                    UserItem userData = JsonUtility.FromJson<UserItem>(webRequest.downloadHandler.text);
                    gem = userData.diamond;
                    heart = userData.heart;

                    UpdateHeartBar();
                    UpdateGemText(gem);

                }
                else
                    Debug.LogError("Missing UIManager");

            }
            else
            {
                Debug.Log("Request failed: " + webRequest.error);
                if (UIManager.instance != null)
                    UIManager.instance.OpenMSGBox(webRequest.error);
            }
        }
    }

    private void UpdateHeartBar()
    {
        float value = heart / 100.0f;

        healthSlider.value = value;
    }

    private void UpdateGemText(int amount)
    {
        gemText.text = amount.ToString();
    }

    public void AddGem(int amount)
    {
        Debug.Log("d");
        SendingID packageItem = new SendingID
        {
            id = thisUserID,
            diamond = 100
        };
        string package = JsonUtility.ToJson(packageItem);
        StartCoroutine(PostRequest("https://test-piggy.codedefeat.com/worktest/dev06/php/AddGem.php", package));
    }
}
