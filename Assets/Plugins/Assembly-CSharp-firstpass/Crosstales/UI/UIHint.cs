using System.Collections;
using UnityEngine;

namespace Crosstales.UI
{
	public class UIHint : MonoBehaviour
	{
		[Tooltip("Group to fade.")]
		public CanvasGroup Group;

		[Tooltip("Delay in seconds before fading (default: 2).")]
		public float Delay = 2f;

		[Tooltip("Fade time in seconds (default: 2).")]
		public float FadeTime = 2f;

		[Tooltip("Disable UI element after the fade (default: true).")]
		public bool Disable = true;

		[Tooltip("Fade at Start (default: true).")]
		public bool FadeAtStart = true;

		public void Start()
		{
			if (FadeAtStart)
			{
				FadeDown();
			}
		}

		public void FadeUp()
		{
			StartCoroutine(lerpAlphaUp(0f, 1f, FadeTime, Delay, Group));
		}

		public void FadeDown()
		{
			StartCoroutine(lerpAlphaDown(1f, 0f, FadeTime, Delay, Group));
		}

		private IEnumerator lerpAlphaDown(float startAlphaValue, float endAlphaValue, float time, float delay, CanvasGroup gameObjectToFade)
		{
			gameObjectToFade.gameObject.SetActive(true);
			Group.alpha = Mathf.Clamp01(startAlphaValue);
			endAlphaValue = Mathf.Clamp01(endAlphaValue);
			yield return new WaitForSeconds(delay);
			while (Group.alpha >= endAlphaValue + 0.01f)
			{
				Group.alpha -= (1f - endAlphaValue) / time * Time.deltaTime;
				yield return null;
			}
			gameObjectToFade.gameObject.SetActive(!Disable);
		}

		private IEnumerator lerpAlphaUp(float startAlphaValue, float endAlphaValue, float time, float delay, CanvasGroup gameObjectToFade)
		{
			gameObjectToFade.gameObject.SetActive(true);
			Group.alpha = Mathf.Clamp01(startAlphaValue);
			endAlphaValue = Mathf.Clamp01(endAlphaValue);
			yield return new WaitForSeconds(delay);
			while (Group.alpha <= endAlphaValue - 0.01f)
			{
				Group.alpha += endAlphaValue / time * Time.deltaTime;
				yield return null;
			}
			gameObjectToFade.gameObject.SetActive(!Disable);
		}
	}
}
