using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class XpLevelBar : MonoBehaviour
{
	public Text xpLevelNum;

	public Text xpLevelNum2;

	public RectTransform xpLevelBarBg;

	public GameObject xpLevelBarFill;

	public Text currentXp;

	private RectTransform xpRectTransform;

	public virtual void OnEnable()
	{
		float num = Currency.GetCurrentXpLevelProgress();
		if (num < 0.05f)
		{
			num = 0.05f;
		}
		xpRectTransform = (RectTransform)xpLevelBarFill.GetComponent(typeof(RectTransform));
		Vector2 sizeDelta = new Vector2(xpLevelBarBg.rect.width * (0f - (1f - num)), xpRectTransform.sizeDelta.y);
		xpRectTransform.sizeDelta = sizeDelta;
		int num2 = Currency.GetCurrentXp();
		int currentXpLevel = Currency.GetCurrentXpLevel();
		xpLevelNum.text = currentXpLevel.ToString("n0");
		if (xpLevelNum2 != null)
		{
			xpLevelNum2.text = currentXpLevel.ToString("n0");
		}
		currentXp.text = num2 % 500 + "/500 XP";
	}
}
