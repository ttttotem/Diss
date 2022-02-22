using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using PlayFab.Json;
using System;
using LoginResult = PlayFab.ClientModels.LoginResult;

public class PlayerLogin : MonoBehaviour
{
    private string userName;
    private string userPassword;
    public Text loginError, registerError;

    public GameObject loginPanel, signupPanel, loggedInPanelA,loggedInPanelB, leaderboardPanel, settingsPanel;

    public InputField email;
    public InputField password;

    GameManager gm;

    public void Start()
    {
        gm = GameManager.GM;
        SetOneActive(loginPanel);
        //Note: Setting title Id here can be skipped if you have set the value in Editor Extensions already.
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
        {
            PlayFabSettings.TitleId = "1566D"; // Please change this value to your own titleId from PlayFab Game Manager
        }
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            if(gm.SystemA == true)
            {
                SetOneActive(loggedInPanelA);
            } else
            {
                SetOneActive(loggedInPanelB);
            }
            
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PlayFabClientAPI.IsClientLoggedIn())
            {
                SetOneActive(loggedInPanelA);
            }
        }
    }

    public void SetOneActive(GameObject panel)
    {
        loginPanel.SetActive(false);
        signupPanel.SetActive(false);
        loggedInPanelA.SetActive(false);
        loggedInPanelB.SetActive(false);
        leaderboardPanel.SetActive(false);
        settingsPanel.SetActive(false);

        if(panel==loggedInPanelB && gm.SystemA == true)
        {
            panel = loggedInPanelA;
        } else if (panel == loggedInPanelA && gm.SystemA == false)
        {
            panel = loggedInPanelB;
        }

        panel.SetActive(true);
    }

    #region login

    public void SignUpTab()
    {
        SetOneActive(signupPanel);
        loginError.text = "";
        registerError.text = "";
    }

    public void LogInTab()
    {
        SetOneActive(loginPanel);
        loginError.text = "";
        registerError.text = "";
    }

    public string Encrypt(string pass)
    {
        if (pass == null || pass == "")
        {
            return "fail";
        }
        System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] bs = System.Text.Encoding.UTF8.GetBytes(pass);
        bs = x.ComputeHash(bs);
        System.Text.StringBuilder s = new System.Text.StringBuilder();
        foreach (byte b in bs)
        {
            s.Append(b.ToString("x2").ToLower());
        }
        return s.ToString();

    }

    private void OnLoginSuccess(LoginResult result)
    {
        PlayerPrefs.SetString("Username", userName);
        GameManager.GM.PlayerID = result.PlayFabId;
        SetOneActive(loggedInPanelA);
        GetStats();
    }
    private void OnLoginFailure(PlayFabError error)
    {
        string errorMessage = "Something went wrong";
        if (error.Error == PlayFabErrorCode.InvalidParams)
        {
            errorMessage = "Invalid inputs";
        } else if (error.Error == PlayFabErrorCode.InvalidPassword)
        {
            errorMessage = "Password is incorrect";
        } else if (error.Error == PlayFabErrorCode.AccountNotFound)
        {
            errorMessage = "Account not found";
        } else if (error.Error == PlayFabErrorCode.InvalidEmailOrPassword)
        {
            errorMessage = "Invalid Email or password";
        }

        Debug.LogError(error.GenerateErrorReport());
        Debug.LogError(error.Error);
        loginError.text = errorMessage;
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        PlayerPrefs.SetString("Username", userName);
        GameManager.GM.PlayerID = result.PlayFabId;
        SetOneActive(loggedInPanelA);
        GetStats();
    }
    private void OnRegisterFailure(PlayFabError error)
    {
        string errorMessage = "Something went wrong";
        if(error.Error == PlayFabErrorCode.EmailAddressNotAvailable)
        {
            errorMessage = "Email adress already taken";
        } else if (error.Error == PlayFabErrorCode.UsernameNotAvailable)
        {
            errorMessage = "Username already taken";
        } else if (error.Error == PlayFabErrorCode.InvalidParams)
        {
            errorMessage = "Invalid inputs";
        }

        Debug.LogError(error.GenerateErrorReport());
        Debug.LogError(error.Error);
        registerError.text = errorMessage;
    }
    public void GetUserPassword(string passwordIn)
    {
        userPassword = passwordIn;
    }

    public void GetUserName(string userNameIn)
    {
        userName = userNameIn;
    }

    public void OnClickLogIn()
    {
        var request = new LoginWithPlayFabRequest {Username = userName, Password = Encrypt(userPassword) };
        PlayFabClientAPI.LoginWithPlayFab(request, OnLoginSuccess, OnLoginFailure);
    }

    public void OnclickGuest()
    {
        //Guest loggin
        Guid obj = Guid.NewGuid();
        var request = new LoginWithCustomIDRequest {CustomId =obj.ToString(), CreateAccount = true };
        PlayFabClientAPI.LoginWithCustomID(request, OnGuestLoginSuccess, OnGuestLoginFailure);
    }

    private void OnGuestLoginSuccess(LoginResult result)
    {
        GameManager.GM.PlayerID = result.PlayFabId;
        SetOneActive(loggedInPanelA);
        GetStats();
    }

    private void OnGuestLoginFailure(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
        Debug.Log("Guest failure");
    }


    public void OnClickRegister()
    {
        var registerRequest = new RegisterPlayFabUserRequest {Password = Encrypt(userPassword), Username = userName, DisplayName = userName,RequireBothUsernameAndEmail = false };
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnRegisterSuccess, OnRegisterFailure);
    }

    public void Logout()
    {
        PlayFabClientAPI.ForgetAllCredentials();
        userPassword = null;
        GameManager.GM.PlayerID = "Guest";
        email.text = userName;
        password.text = "";
        SetOneActive(loginPanel);
    }

    #endregion login

    #region playerStats

    public void GetStats()
    {
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            OnGetStatistics,
            OnPlayFabError
        );
    }

    void OnGetStatistics(GetPlayerStatisticsResult result)
    {
        foreach (var eachStat in result.Statistics)
        {
            switch (eachStat.StatisticName)
            {
                case "Score": 
                    GameManager.GM.score = eachStat.Value;
                    break;
            }
        }
    }

    private static void OnPlayFabError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

    #endregion playerStats

    #region leaderBoard

    public GameObject listingPrefab;
    public Transform listingContainer;

    public void GetLeaderBoard()
    {
        var requestLeaderboard = new GetLeaderboardRequest { StartPosition = 0, StatisticName = "Score", MaxResultsCount = 20 };
        PlayFabClientAPI.GetLeaderboard(requestLeaderboard, OnGetLeaderBoard, OnPlayFabError);
    }

    void OnGetLeaderBoard(GetLeaderboardResult result)
    {
        SetOneActive(leaderboardPanel);
        foreach (PlayerLeaderboardEntry player in result.Leaderboard)
        {
            GameObject tempListing = Instantiate(listingPrefab, listingContainer);
            LeaderBoardListing LL = tempListing.GetComponent<LeaderBoardListing>();
            Image image = tempListing.GetComponent<Image>();

            if(GameManager.GM.PlayerID == player.PlayFabId)
            {
                image.color = new Color(1,1,1,0.2f);
            }

            if(player.DisplayName == "" || player.DisplayName == null)
            {
                LL.playerNameText.text = "Guest: " + player.PlayFabId.Substring(0,5);
            } else
            {
                LL.playerNameText.text = player.DisplayName;
            }
            LL.playerScoreText.text = player.StatValue.ToString();
        }
    }

    public void CloseLeaderBoardPanel()
    {
        SetOneActive(loggedInPanelA);
        for (int i = listingContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(listingContainer.GetChild(i).gameObject);
        }
    }
    #endregion leaderBoard

}