using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class TournamentPin : MonoBehaviour
{
	public Map map;

	public bool isPractice;

	public ScrollRectScroller scrollRectScroller;

	public float scrollX;

	public float scrollY;

	public Button button;

	public PracticeView practiceView;

	public TournamentView tournamentView;

	public int tournamentNum;

	public Sprite completedGraphic;

	public Sprite completedGraphic2;

	public Sprite completedGraphic3;

	public Sprite activeGraphic;

	public Sprite unselectedGraphic;

	public Sprite selectedGraphic;

	public Sprite hiddenGraphic;

	public GameObject continueTournamentIconPrefab;

	public GameObject upgradeReqTournamentIconPrefab;

	private GameObject upradeReqTournamentIcon;

	private bool isActive;

	private int numCompletions;

	public virtual void Awake()
	{
		float x = ((RectTransform)base.gameObject.GetComponent(typeof(RectTransform))).anchorMin.x;
		scrollX = x * 1.2f;
		float y = ((RectTransform)base.gameObject.GetComponent(typeof(RectTransform))).anchorMin.y;
		scrollY = y * 1.2f;
		if (scrollY > 1f)
		{
			scrollY = 1f;
		}
	}

	public virtual void UpdateState(int starterReq, int starterAbility, int backupReq, int backupAbility)
	{
		numCompletions = Tournaments.GetNumCompletions(tournamentNum);
		bool flag = true;
		if (numCompletions == 0 && (starterReq > starterAbility || backupReq > backupAbility))
		{
			flag = false;
		}
		int currentRound = Tournaments.GetCurrentRound(tournamentNum);
		if (currentRound == 2 || currentRound == 3)
		{
			isActive = true;
			GameObject gameObject = UnityEngine.Object.Instantiate(continueTournamentIconPrefab, base.gameObject.transform);
		}
		if (numCompletions >= 3)
		{
			unselectedGraphic = completedGraphic3;
		}
		else if (numCompletions >= 2)
		{
			unselectedGraphic = completedGraphic2;
		}
		else if (numCompletions >= 1)
		{
			unselectedGraphic = completedGraphic;
		}
		button.image.sprite = unselectedGraphic;
		if (flag)
		{
			button.image.color = Color.white;
			if (upradeReqTournamentIcon != null)
			{
				upradeReqTournamentIcon.SetActive(false);
			}
		}
		else
		{
			button.image.color = new Color(1f, 0.64f, 0.64f, 1f);
			if (upradeReqTournamentIcon != null)
			{
				upradeReqTournamentIcon.SetActive(true);
			}
			else
			{
				upradeReqTournamentIcon = UnityEngine.Object.Instantiate(upgradeReqTournamentIconPrefab, base.gameObject.transform);
			}
		}
	}

	public virtual void GoToPin(bool showTournament)
	{
		map.MakeAllPinsUnselected();
		StartCoroutine(MakeSelected());
		ScrollTo();
		if (showTournament)
		{
			StartCoroutine(SetTournamentViewActive());
		}
		else
		{
			Tournaments.SetCurrentTournamentNum(tournamentNum);
		}
	}

	public virtual void ScrollTo()
	{
		Debug.Log("this.gameObject.GetComponent(RectTransform).anchorMin.x;: " + ((RectTransform)base.gameObject.GetComponent(typeof(RectTransform))).anchorMin.x);
		Debug.Log("this.scrollX " + scrollX);
		Debug.Log("this.scrollY " + scrollY);
		scrollRectScroller.SetPos(scrollX, scrollY);
	}

	private IEnumerator SetTournamentViewActive()
	{
		yield return new WaitForSeconds(0.5f);
		if (isPractice)
		{
			practiceView.gameObject.SetActive(true);
			yield break;
		}
		tournamentView.SetViewToTournament(tournamentNum);
		Tournaments.SetCurrentTournamentNum(tournamentNum);
		tournamentView.gameObject.SetActive(true);
	}

	public virtual IEnumerator MakeSelected()
	{
		yield return new WaitForSeconds(0.25f);
		LeanTween.scale(base.gameObject, new Vector3(1.5f, 1.5f, 1f), 0.085f).setEase(LeanTweenType.easeOutQuad).setUseEstimatedTime(true);
		button.image.sprite = unselectedGraphic;
	}

	public virtual void MakeUnselected()
	{
		LeanTween.scale(base.gameObject, new Vector3(1f, 1f, 1f), 0.085f).setEase(LeanTweenType.easeOutQuad).setUseEstimatedTime(true);
		button.image.sprite = unselectedGraphic;
	}

	public virtual void MakeHidden()
	{
	}
}
