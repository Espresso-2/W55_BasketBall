using UnityEngine;

public class AnimIfNeverClicked : MonoBehaviour
{
	public string playerPrefName;

	public int minWins;

	public Animator anim;

	public GameObject exclamationIcon;

	private void OnEnable()
	{
		bool flag = false;
		flag = (((minWins == 0 || PlayerPrefs.GetInt("NUM_WINS") >= minWins) && PlayerPrefs.GetInt(playerPrefName) == 0) ? true : false);
		if (anim != null)
		{
			anim.enabled = flag;
		}
		if (exclamationIcon != null)
		{
			exclamationIcon.SetActive(flag);
		}
	}

	public void OnClick()
	{
		PlayerPrefs.SetInt(playerPrefName, 1);
		if (anim != null && anim.enabled)
		{
			anim.SetTrigger("Stop");
		}
		if (exclamationIcon != null)
		{
			exclamationIcon.SetActive(false);
		}
	}
}
