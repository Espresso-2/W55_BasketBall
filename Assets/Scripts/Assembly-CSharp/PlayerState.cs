using UnityEngine;

public class PlayerState : MonoBehaviour
{
	public enum Location
	{
		VeryFarInFrontOfBall = 0,
		FarInFrontOfBall = 1,
		DirectlyInFrontOfBall = 2,
		DirectlyOnTopOfBall = 3,
		DirectlyBehindBall = 4,
		FarBehindBall = 5
	}

	public enum Momemtum
	{
		None = 0,
		MovingToBall = 1,
		MovingAwayFromBall = 2
	}

	public enum Action
	{
		None = 0,
		WindingUpJump = 1,
		JumpingOnTheWayUp = 2,
		JumpingOnTheWayDown = 3
	}

	public PlayerController pc;

	public Rigidbody2D rigidBody;

	public float distance;

	public Location currentLocation;

	public Momemtum currentMomemtum;

	public Action currentAction;

	public void FixedUpdate()
	{
		float x = base.transform.position.x;
		float x2 = pc.opponent.transform.position.x;
		distance = x2 - x;
		if (distance >= 7f)
		{
			currentLocation = Location.VeryFarInFrontOfBall;
		}
		else if (distance >= 4f)
		{
			currentLocation = Location.FarInFrontOfBall;
		}
		else if (distance >= 1f)
		{
			currentLocation = Location.DirectlyInFrontOfBall;
		}
		else if (distance >= -1f)
		{
			currentLocation = Location.DirectlyOnTopOfBall;
		}
		else if (distance >= -5f)
		{
			currentLocation = Location.DirectlyBehindBall;
		}
		else
		{
			currentLocation = Location.FarBehindBall;
		}
		float x3 = rigidBody.velocity.x;
		float num = 1.5f;
		if (distance >= 0f)
		{
			if (x3 >= num)
			{
				currentMomemtum = Momemtum.MovingToBall;
			}
			else if (x3 <= 0f - num)
			{
				currentMomemtum = Momemtum.MovingAwayFromBall;
			}
			else
			{
				currentMomemtum = Momemtum.None;
			}
		}
		else if (x3 >= num)
		{
			currentMomemtum = Momemtum.MovingAwayFromBall;
		}
		else if (x3 <= 0f - num)
		{
			currentMomemtum = Momemtum.MovingToBall;
		}
		else
		{
			currentMomemtum = Momemtum.None;
		}
		if (pc.springing)
		{
			currentAction = Action.WindingUpJump;
		}
		else if (pc.timeOnGround >= 0.05f)
		{
			currentAction = Action.None;
		}
		else if (rigidBody.velocity.y > 0f)
		{
			currentAction = Action.JumpingOnTheWayUp;
		}
		else if (rigidBody.velocity.y < 0f)
		{
			currentAction = Action.JumpingOnTheWayDown;
		}
	}
}
