using System;
using UnityEngine;

[Serializable]
public class SessionVars : MonoBehaviour
{
    private static SessionVars instance;

    public bool goToTutorial;

    public bool goToScrimmage;

    public bool goToPractice;

    public bool twoPlayerMode;

    public bool showingBackups;

    public bool showingFemales;

    public int selectedStarterNum;

    public int selectedStarterFemaleNum;

    public int selectedBackupNum;

    public int selectedBackupFemaleNum;

    public int selectedSupply;

    public bool[] usingPowerups;

    public bool justCompletedMatch;

    public bool wonLastGame;

    public int numMapViewsThisSession;

    public int sessionSeconds;

    public int secondsSinceFirstAppLaunch;

    public int currentTimestamp;

    public Tournament currentTournament;

    private GameObject debugCanvas;

    public SessionVars()
    {
        selectedSupply = 4;
        usingPowerups = new bool[3];
    }

    public virtual void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("SessionVars").Length > 1)
        {
            UnityEngine.Object.Destroy(base.gameObject);
            return;
        }
        UnityEngine.Object.DontDestroyOnLoad(base.transform.gameObject);
        instance = this;
    }

    public static SessionVars GetInstance()
    {
        return instance;
    }

    public virtual void Start()
    {
        currentTimestamp = PlayerPrefs.GetInt("LAST_RECORDED_TIMESTAMP");
        secondsSinceFirstAppLaunch = currentTimestamp - PlayerPrefs.GetInt("FIRST_APP_LAUNCH_TIMESTAMP");
        InvokeRepeating(nameof(CountSeconds), 1f, 1f);
        SetTimeStampData();
        debugCanvas = GameObject.Find("DebugCanvas");
        if (!(debugCanvas != null))
        {
        }
    }

    private void SetTimeStampData()
    {
        int num = 0;
        num = GetTimeFromDevice();
        if (num > 1430418837)
        {
            int num2 = PlayerPrefs.GetInt("FIRST_APP_LAUNCH_TIMESTAMP");
            if (num2 == 0)
            {
                num2 = num;
                PlayerPrefsHelper.SetInt("FIRST_APP_LAUNCH_TIMESTAMP", num2);
            }
            currentTimestamp = num;
            PlayerPrefsHelper.SetInt("LAST_RECORDED_TIMESTAMP", currentTimestamp);
            secondsSinceFirstAppLaunch = currentTimestamp - num2;
            sessionSeconds = 0;
        }
        else
        {
            Debug.Log("PARSED INT IS INVALID: " + num);
        }
    }

    private int GetTimeFromDevice()
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return (int)(DateTime.UtcNow - dateTime).TotalSeconds;
    }

    public virtual void CountSeconds()
    {
        sessionSeconds++;
        secondsSinceFirstAppLaunch++;
        currentTimestamp++;
        int num = (int)(Time.realtimeSinceStartup - (float)sessionSeconds);
        if (num > 2)
        {
            if (num > 180)
            {
                SetTimeStampData();
            }
            else
            {
                sessionSeconds += num;
                secondsSinceFirstAppLaunch += num;
                currentTimestamp += num;
            }
        }
        if (sessionSeconds % 20 == 0 && sessionSeconds > 10)
        {
            PlayerPrefsHelper.SetInt("LAST_RECORDED_TIMESTAMP", currentTimestamp);
        }
    }

    public virtual void OnApplicationPause(bool pause)
    {
        if (!pause)
        {
            SetTimeStampData();
        }
    }
}