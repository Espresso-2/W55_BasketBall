using System;

[Serializable]
public class Notification
{
	public static int TAB_HOME;

	public static int TAB_PLAYERS;

	public static int TAB_STORE;

	public static int TAB_DEALS;

	public static int TAB_2_PLAYER;

	public static int TAB_TOUR;

	public static int UPGRADE_FINISHED;

	public int time;

	public float type;

	public int num;

	public Notification()
	{
		type = 10f;
	}

	static Notification()
	{
		TAB_PLAYERS = 1;
		TAB_STORE = 2;
		TAB_DEALS = 3;
		TAB_2_PLAYER = 4;
		TAB_TOUR = 5;
		UPGRADE_FINISHED = 6;
	}

	public virtual void Start()
	{
	}

	public virtual void Update()
	{
	}
}
