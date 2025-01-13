using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class SlideInEffect : MonoBehaviour
{
	private Vector3 originalLoc;

	private Vector3 startLocation;

	private Vector3 slideOutLocation;

	public bool dontDeactivateAtTheStart;

	public bool dontAutoDeactivateOnSlideout;

	public bool slideOutAndInIfAlreadyShowing;

	private bool slideOutAndInBecauseAlreadyShowing;

	public float xOffset;

	public float yOffset;

	public bool slideOutOtherSide;

	public float time;

	public bool slideInAtStart;

	public Image img;

	private bool isSlidIn;

	public virtual IEnumerator Start()
	{
		originalLoc = new Vector3(base.transform.position.x, base.transform.position.y, base.transform.position.z);
		startLocation = new Vector3(base.transform.position.x + xOffset, base.transform.position.y + yOffset, originalLoc.z);
		slideOutLocation = new Vector3(base.transform.position.x - xOffset, base.transform.position.y - yOffset, originalLoc.z);
		base.gameObject.transform.position = startLocation;
		if (img != null)
		{
			img.enabled = true;
		}
		if (slideInAtStart)
		{
			StartCoroutine(SlideIn());
			yield break;
		}
		yield return new WaitForSeconds(0.5f);
		if (!dontDeactivateAtTheStart)
		{
			base.gameObject.SetActive(false);
		}
	}

	public virtual void OnEnable()
	{
	}

	public virtual IEnumerator SlideIn()
	{
		if (!slideOutAndInBecauseAlreadyShowing)
		{
			if (isSlidIn && slideOutAndInIfAlreadyShowing)
			{
				slideOutAndInBecauseAlreadyShowing = true;
				LeanTween.move(base.gameObject, new Vector2(startLocation.x, startLocation.y), time / 2f);
				yield return new WaitForSeconds(time / 2f);
				LeanTween.move(base.gameObject, new Vector2(originalLoc.x, originalLoc.y), time).setEase(LeanTweenType.easeOutExpo);
				yield return new WaitForSeconds(time / 2f);
				slideOutAndInBecauseAlreadyShowing = false;
			}
			else
			{
				isSlidIn = true;
				LeanTween.move(base.gameObject, new Vector2(originalLoc.x, originalLoc.y), time).setEase(LeanTweenType.easeOutExpo);
			}
		}
	}

	public virtual IEnumerator SlideOut()
	{
		if (isSlidIn)
		{
			isSlidIn = false;
			yield return new WaitForSeconds(0.1f);
			if (slideOutOtherSide)
			{
				LeanTween.move(base.gameObject, new Vector2(slideOutLocation.x, slideOutLocation.y), time / 2f).setEase(LeanTweenType.easeOutSine);
				yield return new WaitForSeconds(time / 2f);
				LeanTween.move(base.gameObject, new Vector2(startLocation.x, startLocation.y), 0f);
			}
			else
			{
				LeanTween.move(base.gameObject, new Vector2(startLocation.x, startLocation.y), time / 2f);
			}
			if (!dontAutoDeactivateOnSlideout)
			{
				yield return new WaitForSeconds(2f);
				base.gameObject.SetActive(false);
			}
		}
	}
}
