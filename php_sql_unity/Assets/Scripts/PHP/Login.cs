using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Networking;


public class LoginJson
{
    public string status;
    public int id;
}

public class Login : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField usernameField, passwordField;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GoToSignUp()
    {
        if (UIManager.instance != null)
        {
            UIManager.instance.ChangeTab(TAB.LOGIN, TAB.SIGNUP);
        }
    }
    public void LoginToLobby()
    {
        if (usernameField.text.Length <= 0)
        {
            if (UIManager.instance != null)
                UIManager.instance.OpenMSGBox("Please insert username");
            return;
        }
        if (passwordField.text.Length <= 0)
        {
            if (UIManager.instance != null)
                UIManager.instance.OpenMSGBox("Please insert password");
            return;
        }

        UserData userData = new UserData
        {
            playerName = usernameField.text,
            playerPassword = passwordField.text,

        };
        string jsonData = JsonUtility.ToJson(userData);
        StartCoroutine(PostRequest("https://test-piggy.codedefeat.com/worktest/dev06/php/Login.php", jsonData));
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

                    if (webRequest.downloadHandler.text == "[\"Invalid username or password\"]")
                    {
                        UIManager.instance.OpenMSGBox("Invalid username or password");
                    }
                    else
                    {

                        LoginJson receiveJson = JsonUtility.FromJson<LoginJson>(webRequest.downloadHandler.text);
                        //UIManager.instance.OpenMSGBox("Login successful");
                        UIManager.instance.EnterLobby(receiveJson.id);
          
                    }

                    usernameField.text = string.Empty;
                    passwordField.text = string.Empty;


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

}
