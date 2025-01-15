using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class InstallDetails : MonoBehaviour
{
	public Text text;

	private SessionVars sessionVars;

	private string accountInfo;

	public virtual void Start()
	{
		/*accountInfo = "UserId='" + SocialPlatform.Instance.GetUserId() + "' UserDisplayName:'" + SocialPlatform.Instance.GetUserDisplayName() + "'";*/
		SetText();
	}

	public virtual void SetText()
	{
		sessionVars = SessionVars.GetInstance();
		text.text = "\nIsIosDeviceWithiPhoneXStyleScreen:" + DtUtils.IsIosDeviceWithiPhoneXStyleScreen();
		text.text = text.text + "\nScreen.width/Screen.height:" + Screen.width + "/" + Screen.height;
		text.text = text.text + "\nADS_OFF:" + PlayerPrefs.GetInt("ADS_OFF");
		text.text = text.text + "\nRAN_REW_OFF:" + PlayerPrefs.GetInt("RAN_REW_OFF");
		text.text = text.text + "\nNUM_PURCHASES:" + PlayerPrefs.GetInt("NUM_PURCHASES");
		text.text = text.text + "\nIAP_ADS_REMOVED:" + PlayerPrefs.GetInt("IAP_ADS_REMOVED");
		text.text = text.text + "\nLB_VERSION:" + PlayerPrefs.GetInt("LB_VERSION");
		text.text = text.text + "\nCURRENT_HOLIDAY:" + PlayerPrefs.GetInt("CURRENT_HOLIDAY");
		text.text = text.text + "\nCurrency.GetNumPrizeBalls():" + Currency.GetNumPrizeBalls();
		text.text = text.text + "\nsessionSeconds:" + sessionVars.sessionSeconds;
		text.text = text.text + "\nsecondsSinceFirstAppLaunch:" + sessionVars.secondsSinceFirstAppLaunch;
		text.text = text.text + "\ncurrentTimestamp:" + sessionVars.currentTimestamp;
		text.text = text.text + "\naccountInfo: " + accountInfo;
	}

	public virtual void Update()
	{
		SetText();
	}
}
