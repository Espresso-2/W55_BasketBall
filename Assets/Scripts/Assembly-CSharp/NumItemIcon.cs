using UnityEngine;
using UnityEngine.UI;

public class NumItemIcon : MonoBehaviour
{
	public string playerPref;

	public Image bg;

	public Text num;

	public virtual void OnEnable()
	{
		UpdateNum();
	}

	public virtual void UpdateNum()
	{
		int @int = PlayerPrefs.GetInt(playerPref);
		if (@int > 0)
		{
			bg.gameObject.SetActive(true);
			num.text = @int.ToString();
		}
		else
		{
			bg.gameObject.SetActive(false);
			num.text = string.Empty;
		}
	}
}
