using UnityEngine;

public class RequestRatingButton : MonoBehaviour
{
	private GameObject gameSounds;

	private void Start()
	{
		gameSounds = GameObject.Find("GameSounds");
	}

	public void OnClick()
	{
		gameSounds.SendMessage("Play_select");
		NativeReviewRequest.RequestReview();
	}
}
