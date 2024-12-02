using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

//[System.Serializable]
public class UserData
{
    public string playerName;
    public string playerPassword;
}


public class SignUp : MonoBehaviour
{
    private string userName;
    private string userPassword;
    [SerializeField]
    private TMP_InputField usernameField, passwordField, confirmField;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SignUpNewUser()
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
        if (confirmField.text.Length <= 0)
        {
            if (UIManager.instance != null)
                UIManager.instance.OpenMSGBox("Please insert same password in confirm password");
            return;
        }
        if (confirmField.text != passwordField.text)
        {
            if (UIManager.instance != null)
                UIManager.instance.OpenMSGBox("The password is not the same");
            return;
        }

        UserData userData = new UserData
        {
            playerName = usernameField.text,
            playerPassword = passwordField.text,

        };
        string jsonData = JsonUtility.ToJson(userData);
        StartCoroutine(PostRequest("https://test-piggy.codedefeat.com/worktest/dev06/php/insertNewUser.php", jsonData));
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
                    usernameField.text = string.Empty;
                    passwordField.text = string.Empty;
                    confirmField.text = string.Empty;
                    UIManager.instance.OpenMSGBox("Register conpleted!!!");
                    UIManager.instance.ChangeTab(TAB.SIGNUP, TAB.LOGIN);
                }
                if (webRequest.downloadHandler.text.Trim() == "{\"status\":\"error\",\"message\":\"Username already exists\"}")
                    if (UIManager.instance != null)
                        UIManager.instance.OpenMSGBox("Username already Exist!!");
                    else
                        Debug.Log("Can't Display");

            }
            else
            {
                Debug.Log("Request failed: " + webRequest.error);
                if (UIManager.instance != null)
                    UIManager.instance.OpenMSGBox(webRequest.error);
            }
        }
    }

    public void BackToLogin()
    {
        usernameField.text = string.Empty;
        passwordField.text = string.Empty;
        confirmField.text = string.Empty;

        if (UIManager.instance != null)
        {
            UIManager.instance.ChangeTab(TAB.SIGNUP, TAB.LOGIN);
        }
    }

}
