using System.Collections;
using UnityEngine;

public class ShowAchievementsButton : MonoBehaviour
{
    private GameSounds gameSounds;

    public void Start()
    {
        if (PlayerPrefs.GetInt("SOCIAL_PLATFORM_ENABLED") == 0)
        {
            base.gameObject.SetActive(false);
        }
        else
        {
            gameSounds = GameSounds.GetInstance();
        }
    }

    public void OnClick()
    {
        gameSounds.Play_select();
        Debug.LogError("打开成就");
        /*	StartCoroutine(ShowAchievements());*/
    }

    /*	private IEnumerator ShowAchievements()
        {
            yield return new WaitForSeconds(0.25f);
            SocialPlatform.Instance.ShowAchievements();
        }*/
}
