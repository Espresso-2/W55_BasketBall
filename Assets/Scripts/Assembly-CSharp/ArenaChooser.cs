using System.Text.RegularExpressions;
using UnityEngine;

public class ArenaChooser : MonoBehaviour
{
	public static int GetTutorialArena()
	{
		int result = 27;
		int @int = PlayerPrefs.GetInt("ARENA_TUT");
		if (@int > 0)
		{
			result = @int;
		}
		return result;
	}

	public static int GetScrimmageArena()
	{
		int result = 25;
		int @int = PlayerPrefs.GetInt("ARENA_SCRIM");
		if (@int > 0)
		{
			result = @int;
		}
		return result;
	}

	public static int GetMinnesotaArena()
	{
		int result = 73;
		int @int = PlayerPrefs.GetInt("ARENA_MINNESOTA");
		if (@int > 0)
		{
			result = @int;
		}
		return result;
	}

	public static int GetLiveEventArena()
	{
		int result = 48;
		int @int = PlayerPrefs.GetInt("ARENA_LIVE");
		if (@int > 0)
		{
			result = @int;
		}
		return result;
	}

	public static string GetLiveEventName()
	{
		string result = "现场活动!";
		string @string = PlayerPrefs.GetString("LIVE_EVENT_NAME");
		if (@string != null && @string.Length > 0)
		{
			result = @string;
		}
		return result;
	}

	public static string GetSignText()
	{
		string result = "篮球比赛";
		string @string = PlayerPrefs.GetString("SIGN_TEXT");
		if (@string != null && @string.Length > 0)
		{
			result = Regex.Unescape(@string);
		}
		return result;
	}
}
