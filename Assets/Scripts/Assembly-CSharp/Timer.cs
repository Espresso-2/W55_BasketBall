using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Timer : MonoBehaviour
{
    public GameObject notifyObject;

    public Text timeText;

    private GameSounds gameSounds;

    private int waitSeconds;

    private int intValPassed;

    private int secondsWaited;

    public virtual void Start()
    {
        timeText = GetComponent(typeof(Text)) as Text;
        gameSounds = GameSounds.GetInstance();
    }

    public virtual void SetSecondsToWait(int seconds, int intVal)
    {
        waitSeconds = seconds;
        intValPassed = intVal;
        secondsWaited = 0;
        if (gameObject.TryGetComponent(out Text text))
        {
            timeText = text;
            text.text = DoubleTapUtils.GetTimeFromSeconds(waitSeconds);
        }
    }

    public virtual void OnEnable()
    {
        InvokeRepeating(nameof(CountSeconds), 1f, 1f);
    }

    public virtual void OnDisable()
    {
        CancelInvoke(nameof(CountSeconds));
    }

    public virtual void CountSeconds()
    {
        if (waitSeconds == 0)
        {
            return;
        }
        secondsWaited++;
        int num = waitSeconds - secondsWaited;
        if (num <= 0)
        {
            if (notifyObject != null)
            {
                notifyObject.SendMessage("TimerComplete", intValPassed);
            }
        }
        else
        {
            timeText.text = DoubleTapUtils.GetTimeFromSeconds(num);
        }
    }
}