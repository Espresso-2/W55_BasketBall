using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class VoiceOvers : MonoBehaviour
{
	public AudioClip HereIsTheTipoff01;

	public AudioClip TheQuarterFinalsHaveBegun01;

	public AudioClip ItRattlesIn01;

	public AudioClip ThatGoesInForTheWin01;

	public AudioClip ItsGoodForTheWin01;

	public AudioClip ItsGoodForTheWin02;

	public AudioClip ThatsTheEndOfTheFirstHalf03;

	public AudioClip ThatsTheEndOfTheGame03;

	public AudioClip ThatsTheHalf01;

	public AudioClip TheChampionshipIsUnderway02;

	public AudioClip TheSemiFinalsHaveStarted01;

	public AudioClip TheSecHalfIsUnderway01;

	public AudioClip TheSecHalfIsUnderway02;

	public AudioClip TheSecHalfIsUnderway03;

	public AudioClip FirstPlaceIsOnTheLine01;

	public AudioClip AThreeHereWinsTheGame01;

	public AudioClip CanWinWithABucketHere01;

	public AudioClip ForTheWin01;

	public AudioClip ForTheWin02;

	public AudioClip ItsGamePoint01;

	public AudioClip NextBucketWins01;

	public AudioClip WereAllTiedUp01;

	public AudioClip WereAllTiedUp02;

	public AudioClip ItsAOnePosGame01;

	public AudioClip BothTeamsLookExhausted01;

	public AudioClip BothTeamsLookExhausted03;

	public AudioClip Blocked01;

	public AudioClip Blocked02;

	public AudioClip GreatTiming03;

	public AudioClip GreatTiming04;

	public AudioClip Rejected01;

	public AudioClip Rejected02;

	public AudioClip Rejected03;

	public AudioClip GetsTheSteal02;

	public AudioClip HesGotIt01;

	public AudioClip SnagsTheBall01;

	public AudioClip Stolen01;

	public AudioClip Stolen02;

	public AudioClip Stolen04;

	public AudioClip TakesItAway01;

	public AudioClip DownItGoes01;

	public AudioClip DownItGoes02;

	public AudioClip ItsGood01;

	public AudioClip ItsGood02;

	public AudioClip NothingButNet02;

	public AudioClip PrettyShot01;

	public AudioClip FiresForThree01;

	public AudioClip FiresForThree03;

	public AudioClip FromDowntown02;

	public AudioClip FromDowntown03;

	public AudioClip FiresItUp02;

	public AudioClip FiresItUp03;

	public AudioClip FromMidRange01;

	public AudioClip SettlesForALongTwo02;

	public AudioClip ThatsTwo01;

	public AudioClip ThatsTwo02;

	public AudioClip Layup01;

	public AudioClip Layup03;

	public AudioClip ToTheBasket02;

	public AudioClip ToTheBasket03;

	public AudioClip FromInThePaint01;

	public AudioClip LaysItIn01;

	public AudioClip GetsItToGo02;

	public AudioClip GreatFootwork02;

	public AudioClip CrossesOver01;

	public AudioClip CrossesOver03;

	public AudioClip PumpFakes01;

	public AudioClip PumpFakes04;

	public AudioClip GrabsTheRebound01;

	public AudioClip NotEvenClose02;

	private float BothTeamsLookExhausted_Timer;

	private float PumpFakes_Timer;

	private float CrossesOver_Timer;

	private float WereAllTiedUp_Timer;

	private float ItsAOnePosGame_Timer;

	private float GetsRebound_Timer;

	private float NotEvenClose_Timer;

	private float SettlesForALongTwo_Timer;

	private float timeout;

	private float timer;

	private bool justSaidForTheWin;

	private AudioSource audioS;

	private bool isMuted;

	private bool gameIsOver;

	private float gameOverTimer;

	public VoiceOvers()
	{
		timeout = 1.1f;
	}

	public virtual bool Play_HereIsTheTipoff01()
	{
		return PlaySound(HereIsTheTipoff01);
	}

	public virtual bool Play_TheQuarterFinalsHaveBegun01()
	{
		return PlaySound(TheQuarterFinalsHaveBegun01);
	}

	public virtual bool Play_ItRattlesIn01()
	{
		return PlaySound(ItRattlesIn01);
	}

	public virtual bool Play_ThatGoesInForTheWin01()
	{
		return PlaySound(ThatGoesInForTheWin01);
	}

	public virtual bool Play_ItsGoodForTheWin01()
	{
		return PlaySound(ItsGoodForTheWin01);
	}

	public virtual bool Play_ItsGoodForTheWin02()
	{
		return PlaySound(ItsGoodForTheWin02);
	}

	public virtual bool Play_ThatsTheEndOfTheFirstHalf03()
	{
		return PlaySound(ThatsTheEndOfTheFirstHalf03);
	}

	public virtual bool Play_ThatsTheEndOfTheGame03()
	{
		return PlaySound(ThatsTheEndOfTheGame03);
	}

	public virtual bool Play_ThatsTheHalf01()
	{
		return PlaySound(ThatsTheHalf01);
	}

	public virtual bool Play_TheChampionshipIsUnderway02()
	{
		return PlaySound(TheChampionshipIsUnderway02);
	}

	public virtual bool Play_TheSemiFinalsHaveStarted01()
	{
		return PlaySound(TheSemiFinalsHaveStarted01);
	}

	public virtual bool Play_TheSecHalfIsUnderway01()
	{
		return PlaySound(TheSecHalfIsUnderway01);
	}

	public virtual bool Play_TheSecHalfIsUnderway02()
	{
		return PlaySound(TheSecHalfIsUnderway02);
	}

	public virtual bool Play_TheSecHalfIsUnderway03()
	{
		return PlaySound(TheSecHalfIsUnderway03);
	}

	public virtual bool Play_FirstPlaceIsOnTheLine01()
	{
		return PlaySound(FirstPlaceIsOnTheLine01);
	}

	public virtual bool Play_AThreeHereWinsTheGame01()
	{
		return PlaySound(AThreeHereWinsTheGame01);
	}

	public virtual bool Play_CanWinWithABucketHere01()
	{
		return PlaySound(CanWinWithABucketHere01);
	}

	public virtual bool Play_ForTheWin01()
	{
		return PlaySound(ForTheWin01);
	}

	public virtual bool Play_ForTheWin02()
	{
		return PlaySound(ForTheWin02);
	}

	public virtual bool Play_ItsGamePoint01()
	{
		return PlaySound(ItsGamePoint01);
	}

	public virtual bool Play_NextBucketWins01()
	{
		return PlaySound(NextBucketWins01);
	}

	public virtual bool Play_WereAllTiedUp01()
	{
		return PlaySound(WereAllTiedUp01);
	}

	public virtual bool Play_WereAllTiedUp02()
	{
		return PlaySound(WereAllTiedUp02);
	}

	public virtual bool Play_ItsAOnePosGame01()
	{
		return PlaySound(ItsAOnePosGame01);
	}

	public virtual bool Play_BothTeamsLookExhausted01()
	{
		return PlaySound(BothTeamsLookExhausted01);
	}

	public virtual bool Play_BothTeamsLookExhausted03()
	{
		return PlaySound(BothTeamsLookExhausted03);
	}

	public virtual bool Play_Blocked01()
	{
		return PlaySound(Blocked01);
	}

	public virtual bool Play_Blocked02()
	{
		return PlaySound(Blocked02);
	}

	public virtual bool Play_GreatTiming03()
	{
		return PlaySound(GreatTiming03);
	}

	public virtual bool Play_GreatTiming04()
	{
		return PlaySound(GreatTiming04);
	}

	public virtual bool Play_Rejected01()
	{
		return PlaySound(Rejected01);
	}

	public virtual bool Play_Rejected02()
	{
		return PlaySound(Rejected02);
	}

	public virtual bool Play_Rejected03()
	{
		return PlaySound(Rejected03);
	}

	public virtual bool Play_GetsTheSteal02()
	{
		return PlaySound(GetsTheSteal02);
	}

	public virtual bool Play_HesGotIt01()
	{
		return PlaySound(HesGotIt01);
	}

	public virtual bool Play_SnagsTheBall01()
	{
		return PlaySound(SnagsTheBall01);
	}

	public virtual bool Play_Stolen01()
	{
		return PlaySound(Stolen01);
	}

	public virtual bool Play_Stolen02()
	{
		return PlaySound(Stolen02);
	}

	public virtual bool Play_Stolen04()
	{
		return PlaySound(Stolen04);
	}

	public virtual bool Play_TakesItAway01()
	{
		return PlaySound(TakesItAway01);
	}

	public virtual bool Play_DownItGoes01()
	{
		return PlaySound(DownItGoes01);
	}

	public virtual bool Play_DownItGoes02()
	{
		return PlaySound(DownItGoes02);
	}

	public virtual bool Play_ItsGood01()
	{
		return PlaySound(ItsGood01);
	}

	public virtual bool Play_ItsGood02()
	{
		return PlaySound(ItsGood02);
	}

	public virtual bool Play_NothingButNet02()
	{
		return PlaySound(NothingButNet02);
	}

	public virtual bool Play_PrettyShot01()
	{
		return PlaySound(PrettyShot01);
	}

	public virtual bool Play_FiresForThree01()
	{
		return PlaySound(FiresForThree01);
	}

	public virtual bool Play_FiresForThree03()
	{
		return PlaySound(FiresForThree03);
	}

	public virtual bool Play_FromDowntown02()
	{
		return PlaySound(FromDowntown02);
	}

	public virtual bool Play_FromDowntown03()
	{
		return PlaySound(FromDowntown03);
	}

	public virtual bool Play_FiresItUp02()
	{
		return PlaySound(FiresItUp02);
	}

	public virtual bool Play_FiresItUp03()
	{
		return PlaySound(FiresItUp03);
	}

	public virtual bool Play_FromMidRange01()
	{
		return PlaySound(FromMidRange01);
	}

	public virtual bool Play_SettlesForALongTwo02()
	{
		return PlaySound(SettlesForALongTwo02);
	}

	public virtual bool Play_ThatsTwo01()
	{
		return PlaySound(ThatsTwo01);
	}

	public virtual bool Play_ThatsTwo02()
	{
		return PlaySound(ThatsTwo02);
	}

	public virtual bool Play_Layup01()
	{
		return PlaySound(Layup01);
	}

	public virtual bool Play_Layup03()
	{
		return PlaySound(Layup03);
	}

	public virtual bool Play_ToTheBasket02()
	{
		return PlaySound(ToTheBasket02);
	}

	public virtual bool Play_ToTheBasket03()
	{
		return PlaySound(ToTheBasket03);
	}

	public virtual bool Play_FromInThePaint01()
	{
		return PlaySound(FromInThePaint01);
	}

	public virtual bool Play_LaysItIn01()
	{
		return PlaySound(LaysItIn01);
	}

	public virtual bool Play_GetsItToGo02()
	{
		return PlaySound(GetsItToGo02);
	}

	public virtual bool Play_GreatFootwork02()
	{
		return PlaySound(GreatFootwork02);
	}

	public virtual bool Play_CrossesOver01()
	{
		return PlaySound(CrossesOver01);
	}

	public virtual bool Play_CrossesOver03()
	{
		return PlaySound(CrossesOver03);
	}

	public virtual bool Play_PumpFakes01()
	{
		return PlaySound(PumpFakes01);
	}

	public virtual bool Play_PumpFakes04()
	{
		return PlaySound(PumpFakes01);
	}

	public virtual bool Play_GrabsTheRebound01()
	{
		return PlaySound(GrabsTheRebound01);
	}

	public virtual bool Play_NotEvenClose02()
	{
		return PlaySound(NotEvenClose02);
	}

	public virtual void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.transform.gameObject);
	}

	public virtual void Start()
	{
		audioS = GetComponent<AudioSource>();
		if (PlayerPrefs.GetInt(MuteButton.COMMENTATOR_OFF_PREF_KEY) == 1)
		{
			isMuted = true;
		}
	}

	public virtual void Mute()
	{
		isMuted = true;
	}

	public virtual void UnMute()
	{
		if (PlayerPrefs.GetInt(MuteButton.COMMENTATOR_OFF_PREF_KEY) == 0)
		{
			isMuted = false;
		}
	}

	public virtual void Update()
	{
		if (timer > 0f)
		{
			timer -= Time.deltaTime;
		}
		if (BothTeamsLookExhausted_Timer > 0f)
		{
			BothTeamsLookExhausted_Timer -= Time.deltaTime;
		}
		if (PumpFakes_Timer > 0f)
		{
			PumpFakes_Timer -= Time.deltaTime;
		}
		if (CrossesOver_Timer > 0f)
		{
			CrossesOver_Timer -= Time.deltaTime;
		}
		if (WereAllTiedUp_Timer > 0f)
		{
			WereAllTiedUp_Timer -= Time.deltaTime;
		}
		if (ItsAOnePosGame_Timer > 0f)
		{
			ItsAOnePosGame_Timer -= Time.deltaTime;
		}
		if (GetsRebound_Timer > 0f)
		{
			GetsRebound_Timer -= Time.deltaTime;
		}
		if (NotEvenClose_Timer > 0f)
		{
			NotEvenClose_Timer -= Time.deltaTime;
		}
		if (SettlesForALongTwo_Timer > 0f)
		{
			SettlesForALongTwo_Timer -= Time.deltaTime;
		}
		if (gameIsOver)
		{
			gameOverTimer += Time.deltaTime;
			if (gameOverTimer > 4f)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	public virtual void PlayCurrentRound(int round)
	{
		switch (round)
		{
		case 1:
			Play_TheQuarterFinalsHaveBegun01();
			break;
		case 2:
			Play_TheSemiFinalsHaveStarted01();
			break;
		case 3:
			Play_TheChampionshipIsUnderway02();
			break;
		}
	}

	public virtual void PlayEndOfHalf()
	{
		switch (UnityEngine.Random.Range(0, 2))
		{
		case 0:
			Play_ThatsTheEndOfTheFirstHalf03();
			break;
		case 1:
			Play_ThatsTheHalf01();
			break;
		}
	}

	public virtual void PlayStartOfSecHalf(int round)
	{
		timer = 0f;
		if (round == 3)
		{
			Play_FirstPlaceIsOnTheLine01();
			return;
		}
		switch (UnityEngine.Random.Range(0, 3))
		{
		case 0:
			Play_TheSecHalfIsUnderway01();
			break;
		case 1:
			Play_TheSecHalfIsUnderway02();
			break;
		case 2:
			Play_TheSecHalfIsUnderway03();
			break;
		}
	}

	public virtual IEnumerator PlayTied()
	{
		if (timer > 0f)
		{
			yield return new WaitForSeconds(timer + 0.25f);
		}
		if (!(WereAllTiedUp_Timer > 0f))
		{
			switch (UnityEngine.Random.Range(0, 2))
			{
			case 0:
				Play_WereAllTiedUp01();
				break;
			case 1:
				Play_WereAllTiedUp02();
				break;
			}
			WereAllTiedUp_Timer = 20f;
		}
	}

	public virtual IEnumerator PlayOnePosGame()
	{
		if (timer > 0f)
		{
			yield return new WaitForSeconds(timer + 0.25f);
		}
		if (!(ItsAOnePosGame_Timer > 0f))
		{
			Play_ItsAOnePosGame01();
			ItsAOnePosGame_Timer = 20f;
		}
	}

	public virtual IEnumerator PlayGamePoint()
	{
		if (timer > 0f)
		{
			yield return new WaitForSeconds(timer + 0.25f);
		}
		switch (UnityEngine.Random.Range(0, 3))
		{
		case 0:
			Play_ItsGamePoint01();
			break;
		case 1:
			Play_NextBucketWins01();
			break;
		case 2:
			Play_NextBucketWins01();
			break;
		}
	}

	public virtual void PlayEndOfGame(bool userWon)
	{
		if (userWon)
		{
			int num = 0;
			if (justSaidForTheWin)
			{
				switch (UnityEngine.Random.Range(0, 5))
				{
				case 0:
					Play_ItsGood01();
					break;
				case 1:
					Play_ItsGood02();
					break;
				case 2:
					Play_ItsGoodForTheWin02();
					break;
				case 3:
					Play_DownItGoes01();
					break;
				}
			}
			else
			{
				switch (UnityEngine.Random.Range(0, 4))
				{
				case 0:
					Play_ThatGoesInForTheWin01();
					break;
				case 1:
					Play_ItsGoodForTheWin01();
					break;
				case 2:
					Play_ItsGoodForTheWin02();
					break;
				}
			}
		}
		else
		{
			Play_ThatsTheEndOfTheGame03();
		}
		gameIsOver = true;
	}

	public virtual void PlayBothTeamsTired()
	{
		if (!(BothTeamsLookExhausted_Timer > 0f))
		{
			bool flag = false;
			switch (UnityEngine.Random.Range(0, 2))
			{
			case 0:
				flag = Play_BothTeamsLookExhausted01();
				break;
			case 1:
				flag = Play_BothTeamsLookExhausted03();
				break;
			}
			if (flag)
			{
				BothTeamsLookExhausted_Timer = 30f;
			}
		}
	}

	public virtual void PlayBlocked(bool isUser)
	{
		switch (UnityEngine.Random.Range(0, 5))
		{
		case 0:
			Play_Blocked01();
			break;
		case 1:
			Play_Blocked02();
			break;
		case 2:
			Play_Rejected01();
			break;
		case 3:
			Play_Rejected02();
			break;
		case 4:
			Play_Rejected03();
			break;
		}
	}

	public virtual void PlayTipped(bool isUser)
	{
		int num = UnityEngine.Random.Range(0, 2);
		if (isUser)
		{
			switch (num)
			{
			case 0:
				Play_GreatTiming03();
				break;
			case 1:
				Play_GreatTiming04();
				break;
			}
		}
	}

	public virtual void PlayStolen(bool isUser, bool isFemale)
	{
		int num = 0;
		num = ((!isUser) ? UnityEngine.Random.Range(0, 3) : UnityEngine.Random.Range(0, 7));
		switch (num)
		{
		case 0:
			Play_Stolen01();
			return;
		case 1:
			Play_Stolen02();
			return;
		case 2:
			Play_Stolen04();
			return;
		case 3:
			Play_GetsTheSteal02();
			return;
		case 4:
			if (!isFemale)
			{
				Play_HesGotIt01();
				return;
			}
			break;
		}
		switch (num)
		{
		case 5:
			Play_SnagsTheBall01();
			break;
		case 6:
			Play_TakesItAway01();
			break;
		}
	}

	public virtual void PlayInterceptedShot(bool isUser, bool isFemale)
	{
		int num = UnityEngine.Random.Range(0, 3);
		if (isUser)
		{
			if (num == 0 && !isFemale)
			{
				Play_HesGotIt01();
				return;
			}
			switch (num)
			{
			case 1:
				Play_SnagsTheBall01();
				break;
			case 2:
				Play_TakesItAway01();
				break;
			}
			return;
		}
		switch (num)
		{
		case 0:
			Play_Blocked01();
			break;
		case 1:
			Play_Blocked02();
			break;
		case 2:
			Play_TakesItAway01();
			break;
		}
	}

	public virtual IEnumerator PlayMadeThree(int numRimHits)
	{
		if (timer > 0f)
		{
			yield return new WaitForSeconds(0.65f);
		}
		int ran = 0;
		switch ((numRimHits == 0) ? UnityEngine.Random.Range(4, 7) : ((numRimHits >= 3) ? UnityEngine.Random.Range(0, 1) : ((numRimHits < 2) ? UnityEngine.Random.Range(1, 5) : UnityEngine.Random.Range(0, 2))))
		{
		case 0:
			Play_ItRattlesIn01();
			break;
		case 1:
			Play_ItsGood01();
			break;
		case 2:
			Play_ItsGood02();
			break;
		case 3:
			Play_DownItGoes01();
			break;
		case 4:
			Play_DownItGoes02();
			break;
		case 5:
			Play_NothingButNet02();
			break;
		case 6:
			Play_PrettyShot01();
			break;
		}
	}

	public virtual IEnumerator PlayMadeTwo(int numRimHits, bool wasBlocked)
	{
		if (timer > 0f)
		{
			yield return new WaitForSeconds(0.5f);
		}
		int ran = 0;
		switch (wasBlocked ? UnityEngine.Random.Range(5, 7) : ((numRimHits == 0) ? UnityEngine.Random.Range(6, 9) : ((numRimHits >= 3) ? UnityEngine.Random.Range(0, 1) : ((numRimHits < 2) ? UnityEngine.Random.Range(3, 7) : UnityEngine.Random.Range(0, 2)))))
		{
		case 0:
			Play_ItRattlesIn01();
			break;
		case 1:
			Play_ItsGood01();
			break;
		case 2:
			Play_DownItGoes01();
			break;
		case 3:
			Play_DownItGoes02();
			break;
		case 4:
			Play_ItsGood02();
			break;
		case 5:
			Play_ThatsTwo01();
			break;
		case 6:
			Play_ThatsTwo02();
			break;
		case 7:
			Play_NothingButNet02();
			break;
		case 8:
			Play_PrettyShot01();
			break;
		}
	}

	public virtual void PlayMadeDunk()
	{
		switch (UnityEngine.Random.Range(0, 6))
		{
		case 0:
			Play_ThatsTwo01();
			break;
		case 1:
			Play_ThatsTwo02();
			break;
		case 2:
			Play_GreatFootwork02();
			break;
		case 3:
			Play_GetsItToGo02();
			break;
		}
	}

	public virtual void PlayMadeLayup()
	{
		switch (UnityEngine.Random.Range(0, 6))
		{
		case 0:
			Play_LaysItIn01();
			break;
		case 1:
			Play_GetsItToGo02();
			break;
		case 2:
			Play_GreatFootwork02();
			break;
		case 3:
			Play_ItsGood01();
			break;
		case 4:
			Play_ThatsTwo01();
			break;
		}
	}

	public virtual IEnumerator PlayShotThree(bool isUser, bool forTheWin)
	{
		yield return new WaitForSeconds(0.15f);
		justSaidForTheWin = false;
		int ran2 = 0;
		ran2 = (forTheWin ? UnityEngine.Random.Range(0, 6) : (isUser ? UnityEngine.Random.Range(-1, 4) : UnityEngine.Random.Range(-4, 4)));
		if (isUser || true)
		{
			switch (ran2)
			{
			case 0:
				Play_FiresForThree01();
				break;
			case 1:
				Play_FiresForThree03();
				break;
			case 2:
				Play_FromDowntown02();
				break;
			case 3:
				Play_FromDowntown02();
				break;
			case 4:
				Play_ForTheWin01();
				justSaidForTheWin = true;
				break;
			case 5:
				Play_ForTheWin02();
				justSaidForTheWin = true;
				break;
			}
		}
	}

	public virtual IEnumerator PlayShotTwo(bool isUser, float distance, bool forTheWin)
	{
		yield return new WaitForSeconds(0.15f);
		justSaidForTheWin = false;
		int ran2 = 0;
		if ((!isUser && 1 == 0) || !(distance - 2f > Shooter.LAYUP_DISTANCE))
		{
			yield break;
		}
		if (distance + 1.5f > Shooter.THREEPOINT_DISTANCE && SettlesForALongTwo_Timer <= 0f && !forTheWin)
		{
			if (Play_SettlesForALongTwo02())
			{
				SettlesForALongTwo_Timer = 45f;
			}
			yield break;
		}
		ran2 = ((distance + 4f > Shooter.THREEPOINT_DISTANCE) ? ((!isUser) ? UnityEngine.Random.Range(-5, 5) : UnityEngine.Random.Range(0, 5)) : ((!isUser) ? UnityEngine.Random.Range(-4, 4) : UnityEngine.Random.Range(0, 4)));
		switch (ran2)
		{
		case 0:
			if (UnityEngine.Random.Range(0, 100) >= 50)
			{
				Play_FiresItUp02();
			}
			else
			{
				Play_FiresItUp03();
			}
			yield break;
		case 1:
			yield break;
		case 2:
			if (forTheWin)
			{
				Play_ForTheWin01();
				justSaidForTheWin = true;
				yield break;
			}
			break;
		}
		if (ran2 == 3 && forTheWin)
		{
			Play_ForTheWin02();
			justSaidForTheWin = true;
		}
		else if (ran2 == 4)
		{
			Play_FromMidRange01();
		}
	}

	public virtual void PlayShotLayup(bool isUser, float distance, bool dunkTriggered)
	{
		justSaidForTheWin = false;
		int num = 0;
		if (!isUser)
		{
			return;
		}
		if (distance > Shooter.LAYUP_DISTANCE)
		{
			switch (UnityEngine.Random.Range(0, 7))
			{
			case 0:
				if (dunkTriggered)
				{
					Play_ToTheBasket02();
				}
				else
				{
					Play_Layup01();
				}
				break;
			case 1:
				if (dunkTriggered)
				{
					Play_ToTheBasket03();
				}
				else
				{
					Play_Layup03();
				}
				break;
			case 2:
				Play_ToTheBasket02();
				break;
			case 3:
				Play_ToTheBasket03();
				break;
			}
		}
		else if (UnityEngine.Random.Range(0, 5) == 0)
		{
			Play_FromInThePaint01();
		}
	}

	public virtual void PlayPumpFakes()
	{
		if (!(PumpFakes_Timer > 0f))
		{
			bool flag = false;
			switch (UnityEngine.Random.Range(0, 2))
			{
			case 0:
				flag = Play_PumpFakes01();
				break;
			case 1:
				flag = Play_PumpFakes04();
				break;
			}
			if (flag)
			{
				PumpFakes_Timer = 10f;
			}
		}
	}

	public virtual void PlayCrossesOver()
	{
		if (!(CrossesOver_Timer > 0f))
		{
			bool flag = false;
			switch (UnityEngine.Random.Range(0, 2))
			{
			case 0:
				flag = Play_CrossesOver01();
				break;
			case 1:
				flag = Play_CrossesOver03();
				break;
			}
			if (flag)
			{
				CrossesOver_Timer = 10f;
			}
		}
	}

	public virtual void PlayGetsRebound()
	{
		if (!(GetsRebound_Timer > 0f) && Play_GrabsTheRebound01())
		{
			GetsRebound_Timer = 10f;
		}
	}

	public virtual void PlayNotEvenClose()
	{
		if (!(NotEvenClose_Timer > 0f) && Play_NotEvenClose02())
		{
			NotEvenClose_Timer = 10f;
		}
	}

	private bool PlaySound(AudioClip ac)
	{
		if (isMuted || AudioListener.volume == 0f || audioS == null)
		{
			return false;
		}
		if (timer > 0f)
		{
			return false;
		}
		timer = ac.length - 0.5f;
		audioS.PlayOneShot(ac);
		return true;
	}
}
