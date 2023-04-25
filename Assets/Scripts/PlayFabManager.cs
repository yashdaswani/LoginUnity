using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.UI;

public class PlayFabManager : MonoBehaviour
{
    public static PlayFabManager instance;
    public TMP_Text messageLogin;

    [Header("Register")]
    public TMP_InputField RegisterNameInput;
    public TMP_InputField RegisterEmailInput;
    public TMP_InputField RegisterPasswordInput;
    public GameObject RegisterPage;

    [Header("Login")]
    public TMP_InputField LoginEmailInput;
    public TMP_InputField LoginPasswordInput;
    public GameObject LoginPage;

    [Header("passwordReset")]
    public TMP_InputField recoveryEmailInput;
    public GameObject recoverPage;


    public GameObject LoggedInPage;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Login();
    }

    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }

    void OnSuccess(LoginResult result)
    {
        

        messageLogin.text = "Logged in";
        LoggedInPage.SetActive(true);
        LoginPage.SetActive(false);

    }

    public void RegisterButton()
    {
        if (RegisterPasswordInput.text.Length < 6)
        {
            messageLogin.text = "password short";

        }
        var request = new RegisterPlayFabUserRequest
        {
            DisplayName = RegisterNameInput.text,
            Email = RegisterEmailInput.text,
            Password = RegisterPasswordInput.text,
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }

    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        messageLogin.text = "Registered ";
        Invoke("ClearMessage", 2f);
        openLoginpage();

    }

    public void LoginButton()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = RegisterEmailInput.text,
            Password = RegisterPasswordInput.text
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnSuccess, OnError);

    }

    public void PasswordResetButton()
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = recoveryEmailInput.text,
            TitleId = "D2CAB"
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnError);
    }

    void OnPasswordReset(SendAccountRecoveryEmailResult result)
    {
        openLoginpage();
        messageLogin.text = "Password reset mail sent";
        Invoke("ClearMessage", 2f);
        Debug.Log("Password reset mail sent");
    }

    void OnError(PlayFabError error)
    {
       // messageLogin.text = error.GenerateErrorReport();
        Invoke("ClearMessage", 2f);
        Debug.Log("error while logging in /creating acc");
        Debug.Log(error.GenerateErrorReport());
    }






    public void openLoginpage()
    {
        LoginPage.SetActive(true);
        RegisterPage.SetActive(false);
        recoverPage.SetActive(false);
    }
    
    public void openregisterpage()
    {
        LoginPage.SetActive(false);
        RegisterPage.SetActive(true);
    }
    
    public void openrecoverypage()
    {
        LoginPage.SetActive(false);
        recoverPage.SetActive(true);
    }

    public void ClearMessage()
    {
        messageLogin.text = "";
    }



}
