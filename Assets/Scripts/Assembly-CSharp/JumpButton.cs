using System;
using UnityEngine;

[Serializable]
public class JumpButton : MonoBehaviour
{
	public PlayerController playerController;

	public PlayerController playerController2;

	public Animator buttonAnimator;

	public GameController gameController;

	public Camera singlePlayerCam;

	public Camera twoPlayerCam;

	public SpriteRenderer button;

	public SpriteRenderer buttonIcon;

	private SessionVars sessionVars;

	private float startPosY;

	private float endPosY;

	public virtual void Start()
	{
		sessionVars = SessionVars.GetInstance();
		UpdatePosition();
	}

	private void UpdatePosition()
	{
		Vector2 vector = default(Vector2);
		vector = ((!sessionVars.twoPlayerMode) ? ((Vector2)singlePlayerCam.ViewportToWorldPoint(new Vector2(0.85f, 0.13f))) : ((Vector2)twoPlayerCam.ViewportToWorldPoint(new Vector2(0.85f, 0.82f))));
		base.transform.position = vector;
	}

	public virtual void Update()
	{
		if (!gameController.showingMsg && Time.timeScale != 0f)
		{
			playerController.SetJumpButtonDown(false);
			playerController.SetJumpButtonUp(false);
			if (!playerController2.isComputer)
			{
				playerController2.SetJumpButtonDown(false);
				playerController2.SetJumpButtonUp(false);
			}
			if (sessionVars.twoPlayerMode)
			{
				DetectTwoPlayerJump();
			}
			else
			{
				DetectOnePlayerJump();
			}
		}
	}

	private void DetectTwoPlayerJump()
	{
		for (int i = 0; i < Input.touchCount; i++)
		{
			Touch touch = Input.GetTouch(i);
			if (touch.position.x < (float)Screen.width / 2f)
			{
				if (touch.phase == TouchPhase.Began && touch.position.y < (float)Screen.height / 2f)
				{
					playerController.SetJumpButtonDown(true);
				}
				else if (touch.phase == TouchPhase.Ended && touch.position.y < (float)Screen.height / 2f)
				{
					playerController.SetJumpButtonUp(true);
				}
			}
			else if (touch.phase == TouchPhase.Began && touch.position.y > (float)Screen.height / 2f)
			{
				playerController2.SetJumpButtonDown(true);
			}
			else if (touch.phase == TouchPhase.Ended && touch.position.y > (float)Screen.height / 2f)
			{
				playerController2.SetJumpButtonUp(true);
			}
		}
		if (Input.touchCount == 0)
		{
			if (Input.GetMouseButtonDown(0) && Input.mousePosition.x < (float)Screen.width / 2f)
			{
				if (Input.mousePosition.y < (float)(Screen.height / 2))
				{
					playerController.SetJumpButtonDown(true);
				}
			}
			else if (Input.GetMouseButtonUp(0) && Input.mousePosition.x < (float)Screen.width / 2f && Input.mousePosition.y < (float)(Screen.height / 2))
			{
				playerController.SetJumpButtonUp(true);
			}
			if (Input.GetMouseButtonDown(0) && Input.mousePosition.x > (float)Screen.width / 2f)
			{
				if (Input.mousePosition.y > (float)(Screen.height / 2))
				{
					playerController2.SetJumpButtonDown(true);
				}
			}
			else if (Input.GetMouseButtonUp(0) && Input.mousePosition.x > (float)Screen.width / 2f && Input.mousePosition.y > (float)(Screen.height / 2))
			{
				playerController2.SetJumpButtonUp(true);
			}
		}
		if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
		{
			playerController.SetJumpButtonDown(true);
		}
		else if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.UpArrow))
		{
			playerController.SetJumpButtonUp(true);
		}
	}

	private void DetectOnePlayerJump()
	{
		bool flag = false;
		bool flag2 = false;
		for (int i = 0; i < Input.touchCount; i++)
		{
			Touch touch = Input.GetTouch(i);
			if (touch.phase == TouchPhase.Began && touch.position.x > (float)Screen.width / 2f && touch.position.y <= (float)Screen.height * 0.7f)
			{
				flag = true;
				startPosY = touch.position.y;
			}
			else if (touch.phase == TouchPhase.Ended && touch.position.x > (float)Screen.width / 2f && touch.position.y <= (float)Screen.height * 0.7f)
			{
				flag2 = true;
				endPosY = touch.position.y;
			}
		}
		if (Input.touchCount == 0)
		{
			if (Input.GetMouseButtonDown(0) && Input.mousePosition.x > (float)Screen.width / 2f && Input.mousePosition.y <= (float)Screen.height * 0.7f)
			{
				Debug.Log("Input.mousePosition.y: " + Input.mousePosition.y + " Screen.height/4.0f: " + (float)Screen.height / 4f);
				flag = true;
				startPosY = Input.mousePosition.y;
			}
			else if (Input.GetMouseButtonUp(0) && Input.mousePosition.x > (float)Screen.width / 2f && Input.mousePosition.y <= (float)Screen.height * 0.7f)
			{
				flag2 = true;
				endPosY = Input.mousePosition.y;
			}
		}
		if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
		{
			flag = true;
		}
		else if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.UpArrow))
		{
			flag2 = true;
		}
		string trigger = "Normal";
		string trigger2 = "Pressed";
		if (flag)
		{
			playerController.SetJumpButtonDown(true);
			if (buttonAnimator != null)
			{
				buttonAnimator.SetTrigger(trigger2);
				return;
			}
			button.color = Color.grey;
			buttonIcon.color = Color.grey;
		}
		else if (flag2)
		{
			playerController.SetJumpButtonUp(true);
			if (buttonAnimator != null)
			{
				buttonAnimator.SetTrigger(trigger);
			}
			else
			{
				button.color = Color.white;
				buttonIcon.color = Color.white;
			}
			if (endPosY > startPosY + (float)Screen.height * 0.35f && startPosY > 0f)
			{
				gameController.UserSwipedUp();
			}
		}
	}
}
