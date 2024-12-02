using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TAB
{
    LOGIN, SIGNUP, LOBBY, MSG
}


public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField]
    [Header("Login")]
    public GameObject loginTab;
    [Header("Sign Up")]
    public GameObject signUpTab;
    [Header("Lobby")]
    public GameObject lobbyTab;
    [Header("Message")]
    public GameObject msgTab;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;

        
        loginTab.SetActive(true);
        signUpTab.SetActive(false);
        lobbyTab.SetActive(false);
        msgTab.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenMSGBox(string message)
    {
        msgTab.SetActive(true);
        msgTab.GetComponent<MSGBox>().DisplayText(message);
    }

    public void closeTab(TAB closeTab)
    {
        switch (closeTab)
        {
            case TAB.LOBBY:
                lobbyTab.SetActive(false);
                break;
            case TAB.MSG:
                msgTab.SetActive(false);
                break;
            case TAB.SIGNUP:
                signUpTab.SetActive(false);
                break;
            case TAB.LOGIN:
                loginTab.SetActive(false);
                break;

        }
    }

    public void OpenTab(TAB closeTab)
    {
        switch (closeTab)
        {
            case TAB.LOBBY:
                lobbyTab.SetActive(true);
                break;
            case TAB.MSG:
                msgTab.SetActive(true);
                break;
            case TAB.SIGNUP:
                signUpTab.SetActive(true);
                break;
            case TAB.LOGIN:
                loginTab.SetActive(true);
                break;

        }
    }

    public void ChangeTab(TAB fromTab, TAB toTab)
    {
        closeTab(fromTab);
        OpenTab(toTab);
    }

    public void EnterLobby(int id)
    {
        ChangeTab(TAB.LOGIN, TAB.LOBBY);
        lobbyTab.GetComponent<Lobby>().EnterLobby(id);
    }

    public void WorkingInProgressPopUp()
    {
        OpenMSGBox("Working in progress...");
    }
}
