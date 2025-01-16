using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class TeamDetails : MonoBehaviour
{
    public GameObject teamNameText;

    public TopNavBar topNavBar;

    public Text inputField;

    public virtual void OnEnable()
    {
        string teamName = GetTeamName();
        if (teamNameText != null && teamName != string.Empty)
        {
            if ((bool)(Text)teamNameText.GetComponent(typeof(Text)))
            {
                ((Text)teamNameText.GetComponent(typeof(Text))).text = GetTeamName();
            }
            inputField.text = GetTeamName();
        }
        else
        {
            string text = "你的队伍" /*SocialPlatform.Instance.GetUserDisplayName()*/;
            if (text != null && text.Length > 0)
            {
                if (text.Length > 15)
                {
                    text = text.Substring(0, 15);
                }
                /*inputField = (InputField)teamNameText.GetComponent(typeof(InputField));*/
                inputField.text = text;
                SetTeamName(text);
            }
        }
        /*FlurryAnalytics.Instance().LogEvent("SCREEN_DETAILS", new string[4]
        {
            "num_wins:" + Stats.GetNumWins() + string.Empty,
            "num_losses:" + Stats.GetNumLosses() + string.Empty,
            "current_tour:" + Tournaments.GetCurrentTournamentNum() + string.Empty,
            "sessions:" + Stats.GetNumSessions() + string.Empty
        }, false);*/
    }

    public static string GetTeamName()
    {
        return PlayerPrefs.GetString("TEAM_NAME").ToUpper();
    }

    public virtual void SetTeamName(string name)
    {
        /*FlurryAnalytics.Instance().LogEvent("SET_TEAM_NAME", new string[5]
        {
            "name:" + name + string.Empty,
            "num_wins:" + Stats.GetNumWins() + string.Empty,
            "num_losses:" + Stats.GetNumLosses() + string.Empty,
            "current_tour:" + Tournaments.GetCurrentTournamentNum() + string.Empty,
            "sessions:" + Stats.GetNumSessions() + string.Empty
        }, false);*/
        PlayerPrefsHelper.SetString("TEAM_NAME", name, true);
        //  PlayFabManager.Instance().UpdateDisplayName(name);
        if (topNavBar != null)
        {
            topNavBar.teamName.text = name;
        }
    }
}