using System;

[Serializable]
public static class DoubleTapUtils
{
	public static int GetiOSVersion()
	{
		return 0;
	}

	public static string GetTimeFromSeconds(int totalSec)
	{
		/*int num = UnityBuiltins.parseInt(totalSec / 3600) % 24;
		int num2 = UnityBuiltins.parseInt(totalSec / 60) % 60;
		int num3 = totalSec % 60;
		return ((num <= 0) ? string.Empty : (num + ":")) + ((num2 >= 10 || num <= 0) ? (num2 + string.Empty) : ("0" + num2)) + ":" +
		       ((num3 >= 10) ? (num3 + string.Empty) : ("0" + num3));*/
		return string.Empty;
	}

	public static int ConvertStringToInt(string str)
	{
		if (str == null || str == string.Empty)
		{
			return 0;
		}
		return Convert.ToInt32(str);
	}

	public static int GetRoundedSeconds(int seconds)
	{
		if (seconds <= 10)
		{
			return 10;
		}
		if (seconds <= 20)
		{
			return 20;
		}
		if (seconds <= 30)
		{
			return 30;
		}
		if (seconds <= 40)
		{
			return 40;
		}
		if (seconds <= 50)
		{
			return 50;
		}
		if (seconds <= 60)
		{
			return 60;
		}
		if (seconds <= 70)
		{
			return 70;
		}
		if (seconds <= 80)
		{
			return 80;
		}
		if (seconds <= 100)
		{
			return 100;
		}
		if (seconds <= 120)
		{
			return 120;
		}
		if (seconds <= 140)
		{
			return 140;
		}
		if (seconds <= 160)
		{
			return 160;
		}
		if (seconds <= 180)
		{
			return 180;
		}
		if (seconds <= 200)
		{
			return 200;
		}
		if (seconds <= 250)
		{
			return 250;
		}
		if (seconds <= 300)
		{
			return 300;
		}
		if (seconds <= 400)
		{
			return 400;
		}
		if (seconds <= 500)
		{
			return 500;
		}
		return 600;
	}
}
