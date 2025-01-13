using UnityEngine;

public class TestAdsButton : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	public void ShowSuite()
	{
		AdMediation.ShowMediationTestSuite();
	}

	public void FetchInt()
	{
		AdMediation.ReqInt();
	}

	public void ShowInt()
	{
		AdMediation.ShowInt();
	}

	public void ReqVid()
	{
		AdMediation.ReqVid();
	}

	public void PlayVid()
	{
		AdMediation.PlayVid();
	}
}
