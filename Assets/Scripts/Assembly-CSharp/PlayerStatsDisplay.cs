using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PlayerStatsDisplay : MonoBehaviour
{
	public Text[] statValues;

	public GameObject[] statBarBg;

	public GameObject[] statBarFills;

	public GameObject[] statMaxBarFills;

	private float totalFillSize;

	public PlayerStatsDisplay()
	{
		totalFillSize = 60f;
	}

	public virtual void Start()
	{
	}

	public virtual void UpdateDisplay(Player p)
	{
		for (int i = 0; i < statValues.Length; i++)
		{
			RectTransform rectTransform = (RectTransform)statBarBg[i].GetComponent(typeof(RectTransform));
			float statByNum = p.GetStatByNum(i);
			statValues[i].text = statByNum.ToString("n0");
			float num = statByNum / totalFillSize;
			RectTransform rectTransform2 = (RectTransform)statBarFills[i].GetComponent(typeof(RectTransform));
			rectTransform2.sizeDelta = new Vector2(rectTransform.rect.width * (0f - (1f - num)), rectTransform2.sizeDelta.y);
			float maxStatByNum = p.GetMaxStatByNum(i);
			float num2 = maxStatByNum / totalFillSize;
			RectTransform rectTransform3 = (RectTransform)statMaxBarFills[i].GetComponent(typeof(RectTransform));
			rectTransform3.sizeDelta = new Vector2(rectTransform.rect.width * (0f - (1f - num2)), rectTransform3.sizeDelta.y);
		}
	}

	public virtual void Update()
	{
	}
}
