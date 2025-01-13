using UnityEngine;
using UnityEngine.UI;

public class LoadingBox : MonoBehaviour
{
	public Text loadingText;

	public Text debugText;

	public Text tapToContinueText;

	public bool userCanClose;

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
		if (timeEnabled > 10f && !tapToContinueText.IsActive())
		{
			userCanClose = true;
			tapToContinueText.gameObject.SetActive(true);
		}
	}

	public void OnClick()
	{
		if (userCanClose)
		{
			Close();
		}
	}

	private void Close()
	{
		Object.Destroy(base.gameObject);
	}
}
