using UnityEngine;
using UnityEngine.UI;

public class SetPlayfabLoginId : MonoBehaviour
{
	public InputField inputField;

	public GameObject completedDialog;

	public virtual void OnEnable()
	{
		inputField.text = PlayerPrefs.GetString("PLAYFAB_LOGIN_ID");
	}

	public void SetToInputText()
	{
		Debug.Log("SetPlayfabLoginId.SetToInputText(): " + inputField.text);
		PlayerPrefs.DeleteAll();
		PlayerPrefs.SetString("LOAD_PLAYFAB_LOGIN_ID", inputField.text);
		completedDialog.SetActive(true);
		base.gameObject.SetActive(false);
	}
}
