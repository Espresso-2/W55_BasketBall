using UnityEngine;
using UnityEngine.UI;

public class CollectingVidRewButton : MonoBehaviour
{
	public Text debugText;

	public Text tapToContinueText;

	private float timeEnabled;

	public void OnEnable()
	{
		timeEnabled = 0f;
		debugText.gameObject.SetActive(false);
		tapToContinueText.gameObject.SetActive(false);
	}

	public void FixedUpdate()
	{
		timeEnabled += Time.deltaTime;
		if (timeEnabled > 20f && !tapToContinueText.IsActive())
		{
			tapToContinueText.gameObject.SetActive(true);
		}
	}

	public void OnClick()
	{
		if (timeEnabled >= 5f)
		{
			//FlurryAnalytics.Instance().LogEvent("CLICKED_CollectingVidRewButton");
			ForceCompleteVid();
		}
	}

	private void ForceCompleteVid()
	{
		AdMediation.CompletedVid();
	}
}
