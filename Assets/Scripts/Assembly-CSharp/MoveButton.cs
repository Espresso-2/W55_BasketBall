using System;
using UnityEngine;

[Serializable]
public class MoveButton : MonoBehaviour
{
	public PlayerController playerController;

	public PlayerController playerController2;

	public SpriteRenderer leftButton;

	public SpriteRenderer leftButtonBG;

	public SpriteRenderer rightButton;

	public SpriteRenderer rightButtonBG;

	public Animator leftButtonAnimator;

	public Animator rightButtonAnimator;

	public Camera singlePlayerCam;

	public Camera twoPlayerCam;

	private float mouseStartX;

	private float deadZoneX;

	private float buttonCenter;

	private SessionVars sessionVars;

	public virtual void Start()
	{
		sessionVars = SessionVars.GetInstance();
		deadZoneX = (float)Screen.width * 0.012f;
		UpdatePosition();
	}

	private void UpdatePosition()
	{
		Vector2 vector = default(Vector2);
		if (sessionVars.twoPlayerMode)
		{
			vector = twoPlayerCam.ViewportToWorldPoint(new Vector2(0.85f, 0.25f));
			buttonCenter = twoPlayerCam.ViewportToScreenPoint(new Vector2(0.85f, 0.25f)).y;
		}
		else
		{
			vector = singlePlayerCam.ViewportToWorldPoint(new Vector2(0.2f, 0.13f));
			buttonCenter = singlePlayerCam.ViewportToScreenPoint(new Vector2(0.2f, 0.13f)).x;
		}
		Vector2 vector2 = new Vector2(1.2f, 0f);
		base.transform.position = vector;
	}

	public virtual void Update()
	{
		if (Time.timeScale != 0f)
		{
			if (sessionVars.twoPlayerMode)
			{
				DetectTwoPlayerMove();
			}
			else
			{
				DetectOnePlayerMove();
			}
		}
	}

	public virtual void DetectTwoPlayerMove()
	{
		int num = 0;
		int num2 = 0;
		if (Input.touchCount > 0)
		{
			for (int i = 0; i < Input.touchCount; i++)
			{
				Touch touch = Input.GetTouch(i);
				if (touch.position.x < (float)Screen.width / 2f)
				{
					if (touch.position.y > (float)Screen.height - buttonCenter)
					{
						num = -1;
					}
					else if (touch.position.y > (float)Screen.height / 2f)
					{
						num = 1;
					}
				}
				else if (touch.position.y < buttonCenter)
				{
					num2 = -1;
				}
				else if (touch.position.y < (float)Screen.height / 2f)
				{
					num2 = 1;
				}
			}
		}
		else if (Input.GetMouseButton(0) && Input.mousePosition.x < (float)Screen.width / 2f)
		{
			if (Input.mousePosition.y > (float)Screen.height - buttonCenter)
			{
				num = -1;
			}
			else if (Input.mousePosition.y > (float)Screen.height / 2f)
			{
				num = 1;
			}
		}
		else if (Input.GetMouseButton(0))
		{
			if (Input.mousePosition.y < buttonCenter)
			{
				num2 = -1;
			}
			else if (Input.mousePosition.y < (float)Screen.height / 2f)
			{
				num2 = 1;
			}
		}
		playerController.SetMove(num);
		playerController2.SetMove(num2);
	}

	public virtual void DetectOnePlayerMove()
	{
		int num = 0;
		if (Input.touchCount > 0)
		{
			for (int i = 0; i < Input.touchCount; i++)
			{
				Touch touch = Input.GetTouch(i);
				if (touch.position.y < (float)Screen.height / 2f)
				{
					if (touch.position.x < buttonCenter)
					{
						num = -1;
					}
					else if (touch.position.x < (float)Screen.width / 2f)
					{
						num = 1;
					}
				}
			}
		}
		else
		{
			if (Input.GetMouseButton(0) && Input.mousePosition.y < (float)Screen.height / 2f)
			{
				if (Input.mousePosition.x < buttonCenter)
				{
					num = -1;
				}
				else if (Input.mousePosition.x < (float)(Screen.width / 2))
				{
					num = 1;
				}
			}
			if (Input.GetKey(KeyCode.LeftArrow))
			{
				num = -1;
			}
			else if (Input.GetKey(KeyCode.RightArrow))
			{
				num = 1;
			}
		}
		string trigger = "Normal";
		string trigger2 = "Pressed";
		switch (num)
		{
		case 1:
			if (leftButtonAnimator != null && rightButtonAnimator != null)
			{
				leftButtonAnimator.SetTrigger(trigger);
				rightButtonAnimator.SetTrigger(trigger2);
				break;
			}
			leftButton.color = Color.white;
			leftButtonBG.color = Color.white;
			rightButton.color = Color.grey;
			rightButtonBG.color = Color.grey;
			break;
		case -1:
			if (leftButtonAnimator != null && rightButtonAnimator != null)
			{
				leftButtonAnimator.SetTrigger(trigger2);
				rightButtonAnimator.SetTrigger(trigger);
				break;
			}
			leftButton.color = Color.grey;
			leftButtonBG.color = Color.grey;
			rightButton.color = Color.white;
			rightButtonBG.color = Color.white;
			break;
		default:
			if (leftButtonAnimator != null && rightButtonAnimator != null)
			{
				leftButtonAnimator.SetTrigger(trigger);
				rightButtonAnimator.SetTrigger(trigger);
				break;
			}
			leftButton.color = Color.white;
			leftButtonBG.color = Color.white;
			rightButton.color = Color.white;
			rightButtonBG.color = Color.white;
			break;
		}
		playerController.SetMove(num);
	}
}
