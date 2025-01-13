using UnityEngine;

public class ChampAward_C : MonoBehaviour
{
	public GameObject award1;

	public GameObject award2;

	public GameObject msgBox;

	public GameObject confetti;

	public GameObject cheerleaderAnchor;

	public GameObject[] cheerleaders;

	private Animator m_Animator;

	private Vector3 m_CheerleaderOrigin;

	private void Awake()
	{
		m_Animator = GetComponent<Animator>();
	}

	public void ShowAward(bool won)
	{
		if (Application.isEditor && msgBox != null)
		{
			msgBox.SetActive(false);
		}
		award2.SetActive(false);
		award1.SetActive(false);
		if (won)
		{
			award1.SetActive(true);
		}
		else
		{
			award2.SetActive(true);
		}
		if (Random.Range(0, 100) >= 50)
		{
			cheerleaderAnchor.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
			cheerleaders[0].transform.localScale = new Vector3(0.98f, 0.98f, 1f);
			cheerleaders[1].transform.localScale = new Vector3(1.05f, 1.05f, 1f);
		}
		else
		{
			cheerleaderAnchor.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
			cheerleaders[0].transform.localScale = new Vector3(1f, 1f, 1f);
			cheerleaders[1].transform.localScale = new Vector3(1f, 1f, 1f);
		}
		GameObject gameObject = GameObject.Find("GameSounds");
		if (gameObject != null)
		{
			gameObject.SendMessage("Play_air_pump");
			gameObject.SendMessage("Play_quick_applause");
		}
		m_Animator.SetTrigger("ChampAward");
	}
}
