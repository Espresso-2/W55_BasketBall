using System;
using System.Collections.Generic;
using Crosstales.BWF;
using Crosstales.BWF.Model;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayFabManager : MonoBehaviour
{
    private static PlayFabManager instance;

    private string titleId = "4FBF";

    private DateTime timeFromServer;

    private float secondsSinceGettingTimeFromServer;

    private float checkLoggedInFreq = 3f;

    private float checkLoggedInTimer;

    private GetPlayerCombinedInfoResultPayload lastInfoResultPayload;

    private bool setPlayersLocalDataIsCompleted;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            UnityEngine.Object.Destroy(base.gameObject);
        }
        UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
    }

    public static PlayFabManager Instance()
    {
        return instance;
    }

    /*public bool IsClientLoggedIn()
    {
        return PlayFabClientAPI.IsClientLoggedIn();
    }*/

    /*public void LoginAsGuest(bool createAccount)
    {
        PlayFabSettings.TitleId = titleId;
        string playFabIdForLogin = GetPlayFabIdForLogin();
        Debug.Log("PLAYFAB_LOGIN_ID: " + playFabIdForLogin);
        PlayerPrefs.SetString("PLAYFAB_LOGIN_ID", playFabIdForLogin);
        GetPlayerCombinedInfoRequestParams getPlayerCombinedInfoRequestParams = new GetPlayerCombinedInfoRequestParams();
        getPlayerCombinedInfoRequestParams.GetUserAccountInfo = true;
        getPlayerCombinedInfoRequestParams.GetUserInventory = true;
        getPlayerCombinedInfoRequestParams.GetUserVirtualCurrency = true;
        getPlayerCombinedInfoRequestParams.GetUserData = true;
        getPlayerCombinedInfoRequestParams.GetUserReadOnlyData = true;
        getPlayerCombinedInfoRequestParams.GetCharacterInventories = true;
        getPlayerCombinedInfoRequestParams.GetTitleData = true;
        getPlayerCombinedInfoRequestParams.GetPlayerStatistics = true;
        GetPlayerCombinedInfoRequestParams infoRequestParameters = getPlayerCombinedInfoRequestParams;
        /*if (Application.platform == RuntimePlatform.Android)
        {
            LoginWithAndroidDeviceIDRequest loginWithAndroidDeviceIDRequest = new LoginWithAndroidDeviceIDRequest();
            loginWithAndroidDeviceIDRequest.TitleId = titleId;
            loginWithAndroidDeviceIDRequest.CreateAccount = createAccount;
            loginWithAndroidDeviceIDRequest.AndroidDeviceId = playFabIdForLogin;
            loginWithAndroidDeviceIDRequest.InfoRequestParameters = infoRequestParameters;
            LoginWithAndroidDeviceIDRequest request = loginWithAndroidDeviceIDRequest;
            PlayFabClientAPI.LoginWithAndroidDeviceID(request, delegate(LoginResult result)
            {
                LoginReqResult(result);
            }, delegate(PlayFabError error)
            {
                LoginReqError(error);
            });
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            LoginWithIOSDeviceIDRequest loginWithIOSDeviceIDRequest = new LoginWithIOSDeviceIDRequest();
            loginWithIOSDeviceIDRequest.TitleId = titleId;
            loginWithIOSDeviceIDRequest.CreateAccount = createAccount;
            loginWithIOSDeviceIDRequest.DeviceId = playFabIdForLogin;
            loginWithIOSDeviceIDRequest.InfoRequestParameters = infoRequestParameters;
            LoginWithIOSDeviceIDRequest request2 = loginWithIOSDeviceIDRequest;
            PlayFabClientAPI.LoginWithIOSDeviceID(request2, delegate(LoginResult result)
            {
                LoginReqResult(result);
            }, delegate(PlayFabError error)
            {
                LoginReqError(error);
            });
        }
        else
        {#1#
        LoginWithCustomIDRequest loginWithCustomIDRequest = new LoginWithCustomIDRequest();
        loginWithCustomIDRequest.TitleId = titleId;
        loginWithCustomIDRequest.CreateAccount = createAccount;
        loginWithCustomIDRequest.CustomId = playFabIdForLogin;
        loginWithCustomIDRequest.InfoRequestParameters = infoRequestParameters;
        LoginWithCustomIDRequest request3 = loginWithCustomIDRequest;
        PlayFabClientAPI.LoginWithCustomID(request3, delegate (LoginResult result)
        {
            LoginReqResult(result);
        }, delegate (PlayFabError error)
        {
            LoginReqError(error);
        });
        //}
    }*/

    /*
    private void LoginReqResult(LoginResult result)
    {
        Debug.Log("LoginWithCustomID: result.PlayFabId = " + result.PlayFabId);
        PlayerPrefs.SetString("PLAYFAB_RESULT_ID", result.PlayFabId);
        lastInfoResultPayload = result.InfoResultPayload;
        if (result.NewlyCreated)
        {
            //FlurryAnalytics.Instance().LogEvent("PLAYFAB_CREATED_NEW_ACCOUNT");
            Debug.Log("(new account)");
            string @string = PlayerPrefs.GetString("TEAM_NAME");
            if (@string != string.Empty)
            {
                UpdateDisplayName(@string);
            }
        }
        else
        {
            //FlurryAnalytics.Instance().LogEvent("PLAYFAB_LOADED_ACCOUNT");
            Debug.Log("(existing account)");
        }
        SetPlayersLocalData(result.InfoResultPayload);
        GetTime();
        string statName = "EVENT_SCORE_" + PlayerPrefs.GetInt("LEAGUE_NUM");
        PlayFabLeaderboard.GetCurrentUserLeaderBoardRank(statName, false, null);
    }

    private void LoginReqError(PlayFabError error)
    {
        Debug.Log("Error logging in player with custom ID:");
        Debug.Log(error.ErrorMessage);
        if (error.Error == PlayFabErrorCode.InternalServerError)
        {
            Debug.Log("AN INTERNET CONNECTION IS REQUIRED TO PLAY.");
            return;
        }
        Debug.Log("ERROR LOGGING INTO SERVER AS GUEST:\n" + error.ErrorMessage + " (" + (int)error.Error + ")");
    }*/

    /*public void UpdateStat(string name, int val)
    {
        if (!PlayFabClientAPI.IsClientLoggedIn())
        {
            Debug.Log("ERROR calling UpdateStats(): PlayFabClientAPI.IsClientLoggedIn() = false");
            return;
        }
        UpdatePlayerStatisticsRequest updatePlayerStatisticsRequest = new UpdatePlayerStatisticsRequest();
        updatePlayerStatisticsRequest.Statistics = new List<StatisticUpdate>
        {
            new StatisticUpdate
            {
                StatisticName = name,
                Value = val
            }
        };
        UpdatePlayerStatisticsRequest request = updatePlayerStatisticsRequest;
        PlayFabClientAPI.UpdatePlayerStatistics(request, delegate
        {
            Debug.Log("UPDATED STAT: " + name + " = " + val);
        }, delegate (PlayFabError error)
        {
            Debug.Log("Got error updating " + name + " stat:");
            Debug.Log(error.ErrorMessage);
        });
    }*/

    /*public void SetUserDataCall1()
    {
        /*if (!PlayFabClientAPI.IsClientLoggedIn())
        {
            Debug.Log("ERROR calling SetUserDataCall1(): PlayFabClientAPI.IsClientLoggedIn() = false");
            return;
        }#1#
        UpdateUserDataRequest updateUserDataRequest = new UpdateUserDataRequest();
        updateUserDataRequest.Data = new Dictionary<string, string>
        {
            {
                "NUM_SESSIONS",
                PlayerPrefs.GetInt("NUM_SESSIONS").ToString()
            },
            {
                "NUM_WINS",
                PlayerPrefs.GetInt("NUM_WINS").ToString()
            },
            {
                "NUM_LOSSES",
                PlayerPrefs.GetInt("NUM_LOSSES").ToString()
            },
            {
                "WIN_STREAK",
                PlayerPrefs.GetInt("WIN_STREAK").ToString()
            },
            {
                "WIN_STREAK_BEST",
                PlayerPrefs.GetInt("WIN_STREAK_BEST").ToString()
            },
            {
                "LEAGUE_NUM",
                PlayerPrefs.GetInt("LEAGUE_NUM").ToString()
            },
            {
                "playfabLoginId",
                PlayerPrefs.GetString("PLAYFAB_LOGIN_ID")
            },
            {
                "playfabResultId",
                PlayerPrefs.GetString("PLAYFAB_RESULT_ID")
            }
        };
        UpdateUserDataRequest request = updateUserDataRequest;
        /*PlayFabClientAPI.UpdateUserData(request, delegate
        {
            Debug.Log("Successfully executed SetUserDataCall1()");
        }, delegate (PlayFabError error)
        {
            Debug.Log("Error executing SetUserDataCall1()");
            Debug.Log(error.ErrorDetails);
        });#1#
    }
    */

    /*public void SetUserDataCall2()
    {
        /*if (!PlayFabClientAPI.IsClientLoggedIn())
        {
            Debug.Log("ERROR calling SetUserDataCall2(): PlayFabClientAPI.IsClientLoggedIn() = false");
            return;
        }#1#
        UpdateUserDataRequest updateUserDataRequest = new UpdateUserDataRequest();
        updateUserDataRequest.Data = new Dictionary<string, string>
        {
            {
                "NUM_SHOTS",
                PlayerPrefs.GetInt("NUM_SHOTS").ToString()
            },
            {
                "NUM_MAKES",
                PlayerPrefs.GetInt("NUM_MAKES").ToString()
            },
            {
                "NUM_3PTSHOTS",
                PlayerPrefs.GetInt("NUM_3PTSHOTS").ToString()
            },
            {
                "NUM_3PTMAKES",
                PlayerPrefs.GetInt("NUM_3PTMAKES").ToString()
            },
            {
                "NUM_POINTS",
                PlayerPrefs.GetInt("NUM_POINTS").ToString()
            },
            {
                "NUM_REBOUNDS",
                PlayerPrefs.GetInt("NUM_REBOUNDS").ToString()
            },
            {
                "NUM_BLOCKS",
                PlayerPrefs.GetInt("NUM_BLOCKS").ToString()
            },
            {
                "NUM_STEALS",
                PlayerPrefs.GetInt("NUM_STEALS").ToString()
            },
            {
                "NUM_SECONDS",
                PlayerPrefs.GetInt("NUM_SECONDS").ToString()
            }
        };
        UpdateUserDataRequest request = updateUserDataRequest;
        /*PlayFabClientAPI.UpdateUserData(request, delegate
        {
            Debug.Log("Successfully executed SetUserDataCall2()");
        }, delegate (PlayFabError error)
        {
            Debug.Log("Error executing SetUserDataCall2()");
            Debug.Log(error.ErrorDetails);
        });#1#
    }*/

    /*public void SetUserDataForKey(string key, int val)
    {
        SetUserDataForKey(key, val.ToString());
    }*/

    /*public void SetUserDataForKey(string key, string val)
    {
        /*if (!PlayFabClientAPI.IsClientLoggedIn())
        {
            Debug.Log("ERROR calling SetUserDataForKey(): PlayFabClientAPI.IsClientLoggedIn() = false");
            return;
        }#1#
        Debug.Log("SetUserDataForKey('" + key + "', '" + val + "')");
        UpdateUserDataRequest updateUserDataRequest = new UpdateUserDataRequest();
        updateUserDataRequest.Data = new Dictionary<string, string> { { key, val } };
        UpdateUserDataRequest request = updateUserDataRequest;
        /*PlayFabClientAPI.UpdateUserData(request, delegate
        {
            Debug.Log("Successfully executed SetUserDataForKey()");
        }, delegate (PlayFabError error)
        {
            Debug.Log("Error executing SetUserDataForKey()");
            Debug.Log(error.ErrorDetails);
        });#1#
    }*/

    public void SetPlayersLocalDataToLastResult(bool setUserDataToWhatsInPlayFab = false)
    {
        Debug.Log("SetPlayersDataToLastResult(): " + lastInfoResultPayload);
        if (lastInfoResultPayload != null)
        {
            SetPlayersLocalData(lastInfoResultPayload, setUserDataToWhatsInPlayFab);
            /*GetTime();*/
        }
        else
        {
            Debug.Log("LAST DATA RETRIEVED IS NULL");
        }
    }

    private void SetPlayersLocalData(GetPlayerCombinedInfoResultPayload infoResultPayload, bool setUserDataToWhatsInPlayFab = false)
    {
        foreach (KeyValuePair<string, int> item in infoResultPayload.UserVirtualCurrency)
        {
            if (item.Key == "GP")
            {
                PlayerPrefs.SetInt("GOLD_PRIZE", item.Value);
            }
        }
        foreach (StatisticValue playerStatistic in infoResultPayload.PlayerStatistics)
        {
            PlayerPrefs.SetInt(playerStatistic.StatisticName, playerStatistic.Value);
        }
        bool flag = false;
        string @string = PlayerPrefs.GetString("PLAYFAB_LOGIN_ID");
        foreach (KeyValuePair<string, UserDataRecord> userDatum in infoResultPayload.UserData)
        {
            if (userDatum.Key == "NUM_WINS" && @string != null && @string.Length >= 7 && DtUtils.IsAnIntLargerThanZero(userDatum.Value.Value) &&
                PlayerPrefs.GetInt("NUM_WINS") == 0)
            {
                DtUtils.SaveToPrefIntOrString("NUM_WINS_BEFORE_REINSTALL", userDatum.Value.Value);
                flag = true;
            }
            else if (userDatum.Key == "SET_USER_DATA_TO_WHATS_IN_PLAYFAB" && userDatum.Value.Value.ToUpper() == "TRUE")
            {
                setUserDataToWhatsInPlayFab = true;
            }
        }
        if (setUserDataToWhatsInPlayFab)
        {
            foreach (KeyValuePair<string, UserDataRecord> userDatum2 in infoResultPayload.UserData)
            {
                DtUtils.SaveToPrefIntOrString(userDatum2.Key, userDatum2.Value.Value);
            }
            PlayerPrefs.SetString("TEAM_NAME", infoResultPayload.AccountInfo.TitleInfo.DisplayName);
            PlayerPrefs.SetInt("USER_REINSTALLED_GAME", 0);
            //SetUserDataForKey("SET_USER_DATA_TO_WHATS_IN_PLAYFAB", "FALSE");
            /*Application.LoadLevel("MainUI");*/
            SceneManager.LoadScene("MainUI");
        }
        else if (flag)
        {
            Debug.Log("SetPlayersLocalData(): User appears to have reinstalled the game. ");
            PlayerPrefs.SetInt("USER_REINSTALLED_GAME", 1);
            PlayerPrefs.SetString("EXISTING_TEAM_NAME", infoResultPayload.AccountInfo.TitleInfo.DisplayName);
            /*FlurryAnalytics.Instance().LogEvent("USER_REINSTALLED_GAME", new string[4]
            {
                "DisplayName:" + infoResultPayload.AccountInfo.TitleInfo.DisplayName,
                "NUM_WINS:" + PlayerPrefs.GetInt("NUM_WINS"),
                "PLAYFAB_LOGIN_ID:" + PlayerPrefs.GetString("PLAYFAB_LOGIN_ID"),
                "PLAYFAB_RESULT_ID:" + PlayerPrefs.GetString("PLAYFAB_RESULT_ID")
            }, false);*/
        }
        foreach (KeyValuePair<string, string> titleDatum in infoResultPayload.TitleData)
        {
            if (titleDatum.Key == "VAR_LIST")
            {
                SaveVarListIntoPlayerPrefs(titleDatum.Value);
            }
            else
            {
                DtUtils.SaveToPrefIntOrString(titleDatum.Key, titleDatum.Value);
            }
        }
        setPlayersLocalDataIsCompleted = true;
    }

    private void SaveVarListIntoPlayerPrefs(string varList)
    {
        try
        {
            string[] array = varList.Split(';');
            for (int i = 0; i < array.Length; i++)
            {
                string text = array[i];
                Debug.Log("SaveVarListIntoPlayerPrefs individualVars[" + i + "]=" + text);
                if (text != null && text.Length >= 2)
                {
                    int num = text.IndexOf('=');
                    if (num >= 1)
                    {
                        string text2 = text.Substring(0, num);
                        Debug.Log("prefKey='" + text2 + "'");
                        string text3 = text.Substring(num + 1, text.Length - (num + 1));
                        Debug.Log("prefVal='" + text3 + "'");
                        DtUtils.SaveToPrefIntOrString(text2, text3);
                    }
                }
            }
        }
        catch (Exception exception)
        {
            Debug.Log("Error occured when trying to parse PlayFab title data: VAR_LIST");
            Debug.LogException(exception, this);
        }
    }

    /*private void GetTime()
    {
        /*if (!PlayFabClientAPI.IsClientLoggedIn())
        {
            Debug.Log("ERROR calling GetTime(): PlayFabClientAPI.IsClientLoggedIn() = false");
            return;
        }
        GetTimeRequest request = new GetTimeRequest();
        PlayFabClientAPI.GetTime(request, delegate (GetTimeResult result)
        {
            GetTimeResultHandler(result);
        }, delegate (PlayFabError error)
        {
            Debug.Log("Error calling GetTime:");
            Debug.Log(error.ErrorMessage);
        });#1#
    }*/

    /*private void GetTimeResultHandler(GetTimeResult result)
    {
        Debug.Log("GetTimeResultHandler result.Time = " + result.Time);
        timeFromServer = result.Time;
        secondsSinceGettingTimeFromServer = 0f;
    }*/

    public DateTime GetCurrentTime()
    {
        return timeFromServer.AddSeconds(secondsSinceGettingTimeFromServer);
    }

    /*public void UpdateDisplayName(string name)
    {
        /*if (!PlayFabClientAPI.IsClientLoggedIn())
        {
            Debug.Log("ERROR calling UpdateDisplayName(): PlayFabClientAPI.IsClientLoggedIn() = false");
            return;
        }#1#
        string cleanName = BWFManager.ReplaceAll(name, ManagerMask.BadWord, "english", "german", "spanish", "french", "italian", "portuguese", "russian", "chinese", "turkish");
        UpdateUserTitleDisplayNameRequest updateUserTitleDisplayNameRequest = new UpdateUserTitleDisplayNameRequest();
        updateUserTitleDisplayNameRequest.DisplayName = cleanName;
        UpdateUserTitleDisplayNameRequest request = updateUserTitleDisplayNameRequest;
        /*PlayFabClientAPI.UpdateUserTitleDisplayName(request, delegate
        {
            Debug.Log("Successfully updated display name to " + cleanName);
        }, delegate (PlayFabError error)
        {
            Debug.Log("Got updating display name to: " + cleanName);
            Debug.Log(error.ErrorMessage);
        });#1#
    }*/

    /*private string GetPlayFabIdForLogin()
    {
        string text = PlayerPrefs.GetString("PLAYFAB_LOGIN_ID");
        if (text == null || text == string.Empty)
        {
            text = ((!("n/a" != SystemInfo.deviceUniqueIdentifier) || SystemInfo.deviceUniqueIdentifier.Length < 7) ? UnityEngine.Random.Range(1E+14f, 1E+15f).ToString() : SystemInfo.deviceUniqueIdentifier);
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                text += "IO";
            }
            Debug.Log("Created PlayFab Login Id: " + text + "(SystemInfo.deviceUniqueIdentifier: " + SystemInfo.deviceUniqueIdentifier + ")");
        }
        string @string = PlayerPrefs.GetString("LOAD_PLAYFAB_LOGIN_ID");
        if (@string != null && @string.Length > 0)
        {
            text = @string;
        }
        return text;
    }*/

    public void FixedUpdate()
    {
        secondsSinceGettingTimeFromServer += Time.deltaTime;
    }

    /*public void OnApplicationPause(bool pause)
    {
        if (!pause)
        {
            GetTime();
        }
    }*/
}