/*
using System;
using System.Collections.Generic;
using UnityEngine;

public class FlurryAnalytics
{
	private class FlurryAnalyticsNativeHelper : IFlurryAnalyticsNativeHelper
	{
		private AndroidJavaObject _plugin;

		public void CreateInstance(string className, string instanceMethod)
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass(className);
			_plugin = androidJavaClass.CallStatic<AndroidJavaObject>(instanceMethod, new object[0]);
		}

		public void Call(string methodName, params object[] args)
		{
			_plugin.Call(methodName, args);
		}

		public void Call(string methodName, string signature, object arg)
		{
			IntPtr methodID = AndroidJNI.GetMethodID(_plugin.GetRawClass(), methodName, signature);
			AndroidJNI.CallObjectMethod(_plugin.GetRawObject(), methodID, AndroidJNIHelper.CreateJNIArgArray(new object[1] { arg }));
		}

		public ReturnType Call<ReturnType>(string methodName, params object[] args)
		{
			return _plugin.Call<ReturnType>(methodName, args);
		}
	}

	private static FlurryAnalytics _instance;

	private FlurryAnalytics()
	{
		FlurryAnalyticsAndroid.Instance().SetNativeHelper(new FlurryAnalyticsNativeHelper());
	}

	public static FlurryAnalytics Instance()
	{
		if (_instance == null)
		{
			_instance = new FlurryAnalytics();
		}
		return _instance;
	}

	public void Init(string apiKey, bool logUseHttps, bool enableCrashReporting, bool enableDebugging)
	{
		FlurryAnalyticsAndroid.Instance().Init(apiKey, logUseHttps, enableCrashReporting, enableDebugging);
	}

	public void LogEvent(string eventId)
	{
		FlurryAnalyticsAndroid.Instance().LogEvent(eventId, new Dictionary<string, string>(), false);
	}

	public void LogEvent(string eventId, string[] keyVals, bool timed)
	{
		FlurryAnalyticsAndroid.Instance().LogEvent(eventId, keyVals, timed);
	}

	public void LogEvent(string eventId, Dictionary<string, string> parameters, bool timed)
	{
		FlurryAnalyticsAndroid.Instance().LogEvent(eventId, parameters, timed);
	}

	public void LogTimedEvent(string eventId)
	{
		FlurryAnalyticsAndroid.Instance().LogEvent(eventId, new Dictionary<string, string>(), true);
	}

	public void LogTimedEvent(string eventId, string[] keyVals)
	{
		FlurryAnalyticsAndroid.Instance().LogEvent(eventId, keyVals, true);
	}

	public void LogTimedEvent(string eventId, Dictionary<string, string> parameters)
	{
		FlurryAnalyticsAndroid.Instance().LogEvent(eventId, parameters, true);
	}

	public void EndTimedEvent(string eventId)
	{
		FlurryAnalyticsAndroid.Instance().EndTimedEvent(eventId, new Dictionary<string, string>());
	}

	public void EndTimedEvent(string eventId, string[] keyVals)
	{
		FlurryAnalyticsAndroid.Instance().EndTimedEvent(eventId, keyVals);
	}

	public void EndTimedEvent(string eventId, Dictionary<string, string> parameters)
	{
		FlurryAnalyticsAndroid.Instance().EndTimedEvent(eventId, parameters);
	}

	public void LogError(string errorId, string message, string errorClass)
	{
		FlurryAnalyticsAndroid.Instance().LogError(errorId, message, errorClass);
	}

	public void LogPageview()
	{
		FlurryAnalyticsAndroid.Instance().LogPageview();
	}

	public void SetUserId(string userId)
	{
		FlurryAnalyticsAndroid.Instance().SetUserId(userId);
	}

	public void SetUserAge(int userAge)
	{
		FlurryAnalyticsAndroid.Instance().SetUserAge(userAge);
	}

	public void SetUserGender(bool isFemale)
	{
		FlurryAnalyticsAndroid.Instance().SetUserGender(isFemale);
	}

	public void SetUserLocation(double latitude, double longitude)
	{
		FlurryAnalyticsAndroid.Instance().SetUserLocation(latitude, longitude);
	}

	public void SetSessionTimeout(int seconds)
	{
		FlurryAnalyticsAndroid.Instance().SetSessionTimeout(seconds);
	}
}
*/
