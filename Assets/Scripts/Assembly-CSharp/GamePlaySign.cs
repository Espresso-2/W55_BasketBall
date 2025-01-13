using UnityEngine;
using UnityEngine.UI;

public class GamePlaySign : MonoBehaviour
{
	public Text signText;

	private void Start()
	{
		signText.text = ArenaChooser.GetSignText();
	}
}
