using UnityEngine;
using UnityEngine.UI;

public class LoadExistingAccount : MonoBehaviour
{
	public Text existingTeamName;

	public Text numWins;

	public Text supportId;

	public Text playFabLoginId;

	public Text deviceId;

	public GameObject loadExistingButton;

	public GameObject createNewButton;

	private GameSounds gameSounds;

	public void Start()
	{
		if (PlayerPrefs.GetInt("USER_REINSTALLED_GAME") == 0)
		{
			base.gameObject.SetActive(false);
			return;
		}
		gameSounds = GameSounds.GetInstance();
		existingTeamName.text = PlayerPrefs.GetString("EXISTING_TEAM_NAME");
		numWins.text = PlayerPrefs.GetInt("NUM_WINS_BEFORE_REINSTALL").ToString();
		supportId.text = PlayerPrefs.GetString("PLAYFAB_RESULT_ID");
		playFabLoginId.text = PlayerPrefs.GetString("PLAYFAB_LOGIN_ID");
		deviceId.text = SystemInfo.deviceUniqueIdentifier;
	}

	public void LoadExisting()
	{
		gameSounds.Play_select();
		PlayFabManager.Instance().SetPlayersLocalDataToLastResult(true);
		CustomItems customItems = (CustomItems)GameObject.Find("CustomItems").GetComponent(typeof(CustomItems));
		customItems.InstantiateCustomItems();
		loadExistingButton.SetActive(false);
		createNewButton.SetActive(false);
	}

	public void CreateNew()
	{
		gameSounds.Play_select();
		base.gameObject.SetActive(false);
	}
}
