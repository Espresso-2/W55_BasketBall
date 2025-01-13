using UnityEngine;
using UnityEngine.UI;

public class LeaderboardEntry : MonoBehaviour
{
	public Image bg;

	public Color bgColor;

	public Color bgColorCurrentUser;

	public Image rankHolder;

	public Text rankNum;

	public Text displayName;

	public Text playfabId;

	public Text statAmount;

	public Button button;

	private string playFabId;

	private string playFabDisplayName;

	private bool isCurrentUser;

	private GameObject gameSounds;

	private void Start()
	{
	}

	public void UpdateDisplay(int pos, string displayName, int statAmount, string playFabId, string currentUserPlayFabId, bool friends)
	{
		if (displayName == "HIDEEMPTYCELL")
		{
			base.gameObject.transform.localPosition = new Vector3(-999f, base.gameObject.transform.localPosition.y, 0f);
			return;
		}
		if ((displayName == null || displayName == string.Empty) && playFabId != null && playFabId.Length > 9)
		{
			displayName = "PLAYER_" + playFabId.Substring(playFabId.Length - 7);
		}
		isCurrentUser = playFabId == currentUserPlayFabId;
		base.gameObject.transform.localPosition = new Vector3(-0.25f, base.gameObject.transform.localPosition.y, 0f);
		if (bg != null)
		{
			if (isCurrentUser)
			{
				bg.color = bgColorCurrentUser;
			}
			else
			{
				bg.color = bgColor;
			}
		}
		if (rankHolder != null)
		{
			Color color;
			switch (pos)
			{
			case 0:
				color = new Color32(231, 193, 60, byte.MaxValue);
				break;
			case 1:
				color = new Color32(146, 146, 146, byte.MaxValue);
				break;
			case 2:
				color = new Color32(byte.MaxValue, 96, 24, byte.MaxValue);
				break;
			default:
				color = new Color32(195, 195, 195, byte.MaxValue);
				break;
			}
			rankHolder.color = color;
		}
		if (playfabId != null)
		{
			playfabId.text = playFabId;
		}
		rankNum.text = (pos + 1).ToString("n0");
		this.statAmount.text = statAmount.ToString("n0");
		this.displayName.text = displayName;
		this.playFabId = playFabId;
		playFabDisplayName = displayName;
	}

	public void OnClick()
	{
		if (gameSounds == null)
		{
			gameSounds = GameObject.Find("GameSounds");
		}
		if (isCurrentUser)
		{
			GameObject gameObject = GameObject.Find("ShowPlayerProfileButton");
			if (gameObject != null)
			{
				gameObject.SendMessage("ShowProfile");
			}
			return;
		}
		gameSounds.SendMessage("Play_select");
		GameObject gameObject2 = GameObject.Find("LeaderboardPanel");
		if (gameObject2 != null)
		{
			gameObject2.SendMessage("ShowChallengeBox", new string[2] { playFabId, playFabDisplayName });
		}
	}
}
