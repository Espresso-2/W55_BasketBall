using System;
using System.Collections;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Tutorial : MonoBehaviour
{
	public GameObject leftThumb;

	public GameObject rightThumb;

	public GameObject messageBox;

	public Text messageText;

	public Localize messageTextLocalize;

	private string prevTerm;

	public GameObject tutorialComplete;

	public GameObject ballPrefab;

	public Tail ballTail;

	public IkBall ikBall;

	public GameController gameController;

	private int numJumpBlocks;

	private int numShots;

	private int numMadeShots;

	private int numMadeLayups;

	private bool enteredUnderHoop;

	private float timeSinceEnteredUnderHooop;

	private bool gotBall;

	private float seconds;

	private GameSounds gameSounds;

	public virtual IEnumerator Start()
	{
		gameSounds = GameSounds.GetInstance();
		leftThumb.SetActive(false);
		rightThumb.SetActive(false);
		messageBox.SetActive(false);
		messageText.text = string.Empty;
		yield return new WaitForSeconds(0.1f);
		leftThumb.SetActive(true);
		leftThumb.SendMessage("SlideIn");
		yield return new WaitForSeconds(0.5f);
		messageBox.SetActive(true);
		AdTotalManager.Instance.ShowWhiteAd();
		StartCoroutine(SetMessage("USE LEFT THUMB TO MOVE"));
		//FlurryAnalytics.Instance().LogEvent("SHOWED_FIRST_TUT_MSG");
	}

	public virtual void FixedUpdate()
	{
		if (enteredUnderHoop)
		{
			timeSinceEnteredUnderHooop += Time.deltaTime;
		}
	}

	public virtual IEnumerator EnteredUnderHoop()
	{
		if (!enteredUnderHoop)
		{
			enteredUnderHoop = true;
			/*FlurryAnalytics.Instance().LogEvent("tut_enteredUnderHoop", new string[2]
			{
				"numJumpBlocks:" + numJumpBlocks + string.Empty,
				"seconds:" + DoubleTapUtils.GetRoundedSeconds((int)seconds) + string.Empty
			}, false);*/
			yield return new WaitForSeconds(1f);
			StartCoroutine(SetMessage(string.Empty));
			yield return new WaitForSeconds(1f);
			messageBox.SetActive(true);
			AdTotalManager.Instance.ShowWhiteAd();
			StartCoroutine(SetMessage("HOLD DOWN RIGHT THUMB TO JUMP"));
			rightThumb.SetActive(true);
			rightThumb.SendMessage("SlideIn");
		}
	}

	public virtual IEnumerator JumpBlocked()
	{
		if (!(timeSinceEnteredUnderHooop < 3f))
		{
			numJumpBlocks++;
			/*FlurryAnalytics.Instance().LogEvent("tut_jumpblock", new string[2]
			{
				"numJumpBlocks:" + numJumpBlocks + string.Empty,
				"seconds:" + DoubleTapUtils.GetRoundedSeconds((int)seconds) + string.Empty
			}, false);*/
			if (numJumpBlocks == 1)
			{
				rightThumb.SendMessage("SlideOut");
				leftThumb.SendMessage("SlideOut");
				yield return new WaitForSeconds(1f);
				StartCoroutine(SetMessage(string.Empty));
				yield return new WaitForSeconds(1f);
				messageBox.SetActive(true);
				AdTotalManager.Instance.ShowWhiteAd();
				StartCoroutine(SetMessage("THE BAR ABOVE YOUR HEAD IS YOUR ENERGY"));
				yield return new WaitForSeconds(4f);
				leftThumb.SetActive(false);
				StartCoroutine(SetMessage(string.Empty));
				StartCoroutine(gameController.NewTutorialPlay());
			}
		}
	}

	public virtual IEnumerator GotBall()
	{
		/*FlurryAnalytics.Instance().LogEvent("tut_gotball", new string[2]
		{
			"numJumpBlocks:" + numJumpBlocks + string.Empty,
			"seconds:" + DoubleTapUtils.GetRoundedSeconds((int)seconds) + string.Empty
		}, false);*/
		if (!gotBall)
		{
			gotBall = true;
			yield return new WaitForSeconds(0.5f);
			messageBox.SetActive(true);
			AdTotalManager.Instance.ShowWhiteAd();
			StartCoroutine(SetMessage("HOLD DOWN RIGHT THUMB TO SHOOT"));
			rightThumb.SetActive(true);
			rightThumb.SendMessage("SlideIn");
		}
	}

	public virtual IEnumerator ShotBall()
	{
		numShots++;
		/*FlurryAnalytics.Instance().LogEvent("tut_shotball", new string[2]
		{
			"numJumpBlocks:" + numJumpBlocks + string.Empty,
			"seconds:" + DoubleTapUtils.GetRoundedSeconds((int)seconds) + string.Empty
		}, false);*/
		if (numShots == 1)
		{
			yield return new WaitForSeconds(3f);
			StartCoroutine(SetMessage(string.Empty));
		}
		yield return new WaitForSeconds(1f);
		if (numShots == 2)
		{
			messageBox.SetActive(true);
			AdTotalManager.Instance.ShowWhiteAd();
			StartCoroutine(SetMessage("TIP: RELEASE AFTER PEAK OF JUMP"));
		}
	}

	public virtual IEnumerator MadeShot(bool isLayup)
	{
		numMadeShots++;
		if (isLayup)
		{
			numMadeLayups++;
		}
		/*FlurryAnalytics.Instance().LogEvent("tut_madeshot", new string[2]
		{
			"numJumpBlocks:" + numJumpBlocks + string.Empty,
			"seconds:" + DoubleTapUtils.GetRoundedSeconds((int)seconds) + string.Empty
		}, false);*/
		if (numMadeShots == 1)
		{
			rightThumb.SendMessage("SlideOut");
		}
		else if (numMadeShots == 3)
		{
			StartCoroutine(SetMessage(string.Empty));
		}
		else if (numMadeShots == 4)
		{
			yield return new WaitForSeconds(0.25f);
			messageBox.SetActive(true);
			AdTotalManager.Instance.ShowWhiteAd();
			StartCoroutine(SetMessage("TIP: GO UNDER HOOP TO SHOOT A LAYUP"));
		}
		else if (isLayup && numMadeShots == 5)
		{
			StartCoroutine(SetMessage(string.Empty));
			yield return new WaitForSeconds(1f);
			messageBox.SetActive(true);
			AdTotalManager.Instance.ShowWhiteAd();
			StartCoroutine(SetMessage("YOU NEED FULL ENERGY TO DUNK"));
			yield return new WaitForSeconds(6f);
			StartCoroutine(SetMessage(string.Empty));
		}
	}

	public virtual IEnumerator TutorialCompleted()
	{
		/*FlurryAnalytics.Instance().LogEvent("tut_completed", new string[3]
		{
			"numShots:" + numShots + string.Empty,
			"numJumpBlocks:" + numJumpBlocks + string.Empty,
			"seconds:" + DoubleTapUtils.GetRoundedSeconds((int)seconds) + string.Empty
		}, false);*/
		yield return new WaitForSeconds(0.25f);
		tutorialComplete.SetActive(true);
		AdTotalManager.Instance.ShowWhiteAd();
	}

	private IEnumerator SetMessage(string newTerm)
	{
		if (newTerm == string.Empty)
		{
			LeanTween.scale(messageBox, new Vector3(0.25f, 0.25f), 0.16f).setEase(LeanTweenType.easeInQuad);
			yield return new WaitForSeconds(0.14f);
			messageBox.SetActive(false);
			AdTotalManager.Instance.ShowBlackAd();
		}
		else if (newTerm != prevTerm)
		{
			messageTextLocalize.SetTerm(newTerm, null);
			StartCoroutine(TweenMessageBox());
			prevTerm = newTerm;
		}
	}

	private IEnumerator TweenMessageBox()
	{
		LeanTween.scale(messageBox, new Vector3(1.08f, 1.08f), 0.16f).setEase(LeanTweenType.easeOutQuad);
		yield return new WaitForSeconds(0.14f);
		LeanTween.scale(messageBox, new Vector3(1f, 1f), 0.16f).setEase(LeanTweenType.easeInQuad);
	}

	public virtual void Update()
	{
		seconds += Time.deltaTime;
	}
}
