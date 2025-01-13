using System;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class NotificationsPanel : MonoBehaviour
{
	public bool ignoreNotifications;

	public GameObject alertMessagePanel;

	public Localize alertMessage;

	public HeadVisual headVisual;

	public GameObject[] tabNotifications;

	public Text[] tabNotificationsText;

	public Localize[] tabNotificationsTextLocalized;

	public TabChanger tabChanger;

	private Players players;

	private Player player;

	private string actionScene;

	private SessionVars sessionVars;

	private GameSounds gameSounds;

	public virtual void Awake()
	{
		sessionVars = SessionVars.GetInstance();
		gameSounds = GameSounds.GetInstance();
		players = (Players)base.gameObject.GetComponent(typeof(Players));
		players.InstantiatePlayers();
		HideNotifications();
		float x = 1f;
		Vector3 localScale = base.gameObject.transform.localScale;
		localScale.x = x;
		base.gameObject.transform.localScale = localScale;
		float y = 1f;
		Vector3 localScale2 = base.gameObject.transform.localScale;
		localScale2.y = y;
		base.gameObject.transform.localScale = localScale2;
	}

	public virtual void ShowBoxNotification(string actionScene, int num)
	{
		if (ignoreNotifications || (actionScene == "PlayerUpgrade" && TabChanger.currentScreenNum == screenEnum.Upgrade))
		{
			return;
		}
		GameObject gameObject = GameObject.Find("TournamentPanel");
		if (gameObject != null)
		{
			((TournamentView)gameObject.GetComponent(typeof(TournamentView))).UpgradeWasCompleted();
		}
		GameObject gameObject2 = GameObject.Find("MapPanel");
		if (gameObject2 != null)
		{
			((Map)gameObject2.GetComponent(typeof(Map))).UpgradeWasCompleted();
		}
		this.actionScene = actionScene;
		if (actionScene == "PlayerUpgrade")
		{
			player = players.GetPlayerByPlayerPrefNum(num);
			alertMessage.SetTerm("YOUR {PLAYER} UPGRADE IS FINISHED", null);
			GameObject gameObject3 = GameObject.Find("CharacterSprites");
			if (gameObject3 != null)
			{
				headVisual.gameObject.SetActive(true);
				CharacterSprites cs = (CharacterSprites)gameObject3.GetComponent(typeof(CharacterSprites));
				headVisual.SetVisual(player, cs, 0);
			}
			else
			{
				headVisual.gameObject.SetActive(false);
			}
		}
		else
		{
			alertMessage.SetTerm("UNKOWN ALERT", null);
			headVisual.gameObject.SetActive(false);
		}
		alertMessagePanel.SetActive(false);
		alertMessagePanel.SetActive(true);
	}

	public virtual void ShowTabNotification(int type, bool show)
	{
		if (ignoreNotifications)
		{
			return;
		}
		for (int i = 0; i < tabNotifications.Length; i++)
		{
			if (i == 0 && type == Notification.TAB_HOME)
			{
				tabNotifications[i].SetActive(show);
				tabNotificationsTextLocalized[i].SetTerm("NEW", null);
			}
			if (i == 1 && type == Notification.TAB_PLAYERS)
			{
				tabNotifications[i].SetActive(show);
				tabNotificationsTextLocalized[i].SetTerm("NEW", null);
			}
			if (i == 2 && type == Notification.TAB_STORE)
			{
				tabNotifications[i].SetActive(show);
				tabNotificationsTextLocalized[i].SetTerm("NEW", null);
			}
			if (i == 4 && type == Notification.TAB_2_PLAYER)
			{
				tabNotifications[i].SetActive(show);
				tabNotificationsTextLocalized[i].SetTerm("NEW", null);
			}
			if (i == 5 && type == Notification.TAB_TOUR)
			{
				tabNotifications[i].SetActive(show);
				tabNotificationsTextLocalized[i].SetTerm("NEW", null);
			}
		}
	}

	private void HideNotifications()
	{
		for (int i = 0; i < tabNotifications.Length; i++)
		{
			tabNotifications[i].SetActive(false);
		}
	}

	public virtual void Update()
	{
	}

	public virtual void ContinueOnClick()
	{
		gameSounds.Play_select();
		alertMessagePanel.SetActive(false);
	}

	public virtual void ActionOnClick()
	{
		if (player != null)
		{
			sessionVars.showingBackups = player.isBackup;
			sessionVars.showingFemales = player.isFemale;
			if (player.isBackup)
			{
				if (player.isFemale)
				{
					sessionVars.selectedBackupFemaleNum = player.num;
				}
				else
				{
					sessionVars.selectedBackupNum = player.num;
				}
			}
			else if (player.isFemale)
			{
				sessionVars.selectedStarterFemaleNum = player.num;
			}
			else
			{
				sessionVars.selectedStarterNum = player.num;
			}
		}
		alertMessagePanel.SetActive(false);
		gameSounds.Play_one_dribble();
		if (actionScene == "PlayerUpgrade")
		{
			TabChanger.currentBackAction = backAction.CurrentTab;
			tabChanger.SetToScreen(screenEnum.Upgrade);
		}
	}

	public virtual void OnModifyAlertMessageLocalization()
	{
		if (!string.IsNullOrEmpty(Localize.MainTranslation))
		{
			string fullName = player.fullName;
			Localize.MainTranslation = Localize.MainTranslation.Replace("{PLAYER}", fullName);
		}
	}
}
