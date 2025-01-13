using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NumTallyUpper : MonoBehaviour
{
	public Text text;

	private GameObject gameSounds;

	private void Awake()
	{
		gameSounds = GameObject.Find("GameSounds");
	}

	public void UpdateNum(int startNum, int finishNum, int speed, bool playSound)
	{
		StartCoroutine(AnimateTextNum(text, startNum, finishNum, speed, playSound));
	}

	private IEnumerator AnimateTextNum(Text t, int startNum, int finishNum, int speed, bool playSound)
	{
		t.text = startNum.ToString("n0");
		bool scaleUp = true;
		for (int i = startNum; i <= finishNum; i += speed)
		{
			if (speed <= 0)
			{
				break;
			}
			t.text = i.ToString("n0");
			yield return new WaitForSeconds(0.025f);
			if (playSound)
			{
				gameSounds.SendMessage("Play_light_click_2");
			}
			if (i % 100 == 0)
			{
				if (scaleUp)
				{
					LeanTween.scale(t.gameObject, new Vector3(1.15f, 1.15f, 1f), 0.05f).setEase(LeanTweenType.easeOutExpo);
				}
				else
				{
					LeanTween.scale(t.gameObject, new Vector3(1f, 1f, 1f), 0.05f).setEase(LeanTweenType.easeOutExpo);
				}
				scaleUp = !scaleUp;
			}
		}
		if (playSound)
		{
			gameSounds.SendMessage("Play_light_click_2");
		}
		LeanTween.scale(t.gameObject, new Vector3(1f, 1f, 1f), 1f).setEase(LeanTweenType.easeOutExpo);
		t.text = finishNum.ToString("n0");
	}
}
