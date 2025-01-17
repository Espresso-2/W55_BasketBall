using System;
using System.Collections;
using I2.Loc;
using UnityEngine;

[Serializable]
public class GameHintMsg : MonoBehaviour
{
    public Localize msg;

    public bool isPauseScreen;

    public bool isLoadScreen;

    private string[] hints;

    private int currentHintNum;

    private GameSounds gameSounds;

    public GameHintMsg()
    {
        hints = new string[26]
        {
            "YOU WILL MOVE SLOW", "YOUR ENERGY WILL NOT", "STEAL THE BALL WHEN IT IS BLINKING", "Upgrade your DEFENSE",
            "YOU CAN’T STEAL WHEN PLAYER IS SHOOTING", "WIN TOURNAMENTS 3 TIMES TO COLLECT TROPHIES", "SCORE 3S BY SHOOTING",
            "RECHARGE YOUR ENERGY BY", "PUMP FAKE BY QUICKLY", "YOU CAN STEAL THE",
            "TIRED DEFENDERS ARE EASIER TO GET PAST", "TIP: RELEASE AFTER PEAK OF JUMP", "GET A RUNNING START TO DUNK",
            "PLAY EVERYDAY TO OPEN YOUR DAILY BAGS", "YOU NEED FULL ENERGY TO STEEL", "GET A FREE BAG BY WINNING A TOURNAMENT 3X",
            "Meet people at the rim to block their dunks", "OPPONENT MUST BE IN RED TO STEEL", "CONSERVE YOUR ENERGY WHEN POSSIBLE",
            "JUMPING USES A LOT OF ENERGY",
            "YOU NEED FULL ENERGY TO DUNK", "GETTING PAST THE DEFENDER USES A LOT OF ENERGY", "JABSTEP by quickly tapping forward",
            "REPLAY TOURNAMENTS FOR AN INCREASING CHALLENGE", "PRESS BACK THEN FORWARD TO DO A CROSSOVER",
            "Use PUMPFAKES and JABSTEPS to confuse people"
        };
    }

    public virtual void Awake()
    {
        gameSounds = GameSounds.GetInstance();
    }

    public virtual void OnEnable()
    {
        if (isPauseScreen || isLoadScreen)
        {
            currentHintNum = UnityEngine.Random.Range(0, hints.Length);
            NextHint(false);
        }
        else
        {
            StartCoroutine(TweenEffect());
        }
    }

    public virtual void NextHint(bool goBack)
    {
        if (!isLoadScreen)
        {
            gameSounds.Play_select();
        }
        int num = 0;
        num = ((!goBack) ? (currentHintNum + 1) : (currentHintNum - 1));
        if (num < 0)
        {
            num = hints.Length - 1;
        }
        else if (num >= hints.Length)
        {
            num = 0;
        }
        currentHintNum = num;
        msg.SetTerm(hints[currentHintNum], null);
    }

    private IEnumerator TweenEffect()
    {
        LeanTween.scale(base.gameObject, new Vector3(1.05f, 1.05f, 1f), 0.16f).setEase(LeanTweenType.easeOutExpo).setUseEstimatedTime(true);
        yield return new WaitForSeconds(0.14f);
        LeanTween.scale(base.gameObject, new Vector3(1f, 1f, 1f), 0.16f).setEase(LeanTweenType.easeInExpo).setUseEstimatedTime(true);
    }

    public virtual void Start()
    {
    }

    public virtual void Update()
    {
    }

    public virtual void Fatigued()
    {
        msg.SetTerm("YOU WILL MOVE SLOW", null);
    }

    public virtual void NeedBallForTimeout()
    {
        msg.SetTerm("YOU CAN ONLY CALL", null);
    }

    public virtual void NoTimeoutsLeft()
    {
        msg.SetTerm("YOU ARE OUT OF", null);
    }

    public virtual void FirstHalftime()
    {
        msg.SetTerm("SWAP IN YOUR BACKUP", null);
    }

    public virtual void Dehydrated()
    {
        msg.SetTerm("YOUR ENERGY WILL NOT", null);
    }

    public virtual void Grip()
    {
        msg.SetTerm("GRIP SPRAY IS CURRENTLY INCREASING YOUR SPEED FOR THIS ENTIRE GAME!", null);
    }

    public virtual void Chalk()
    {
        msg.SetTerm("SHOOTING CHALK IS CURRENTLY INCREASING YOUR SHOOTING BY FOR THIS ENTIRE GAME!", null);
    }

    public virtual void Protein()
    {
        msg.SetTerm("PROTEIN BAR IS CURRENTLY INCREASING YOUR ENERGY RECOVERY RATE BY 35%!", null);
    }

    public virtual void Threes()
    {
        msg.SetTerm("SCORE 3S BY SHOOTING", null);
    }

    public virtual void HoldDownShootLonger()
    {
        msg.SetTerm("YOU NEED TO HOLD DOWN SHOOT LONGER", null);
    }

    public virtual void DontSwipe()
    {
        int num = UnityEngine.Random.Range(0, 100);
        if (num >= 66)
        {
            msg.SetTerm("DONT SWIPE OR FLICK TO SHOOT", null);
        }
        else if (num >= 33)
        {
            msg.SetTerm("DO NOT SWIPE WHEN SHOOTING OR JUMPING", null);
        }
        else
        {
            msg.SetTerm("ALWAYS PRESS AND RELEASE IN SAME SPOT", null);
        }
    }

    public virtual void EnergyRecovery()
    {
        msg.SetTerm("RECHARGE YOUR ENERGY BY", null);
    }

    public virtual void Layup()
    {
        msg.SetTerm("HIT LAYUPS BY SHOOTING", null);
    }

    public virtual void PumpFake()
    {
        msg.SetTerm("PUMP FAKE BY QUICKLY", null);
    }

    public virtual void Stealing()
    {
        msg.SetTerm("YOU CAN STEAL THE", null);
    }

    public virtual void Stealing2()
    {
        msg.SetTerm("STEAL THE BALL WHEN IT IS BLINKING", null);
    }

    public virtual void DoubleDribble()
    {
        msg.SetTerm("YOU CANT MOVE BECAUSE", null);
    }
}