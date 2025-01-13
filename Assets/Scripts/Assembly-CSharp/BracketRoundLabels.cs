using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class BracketRoundLabels : MonoBehaviour
{
	public Text[] roundNames;

	public Image[] roundArrows;

	public Image[] roundDimmers;

	public Color roundArrowColor;

	public Color currentRoundArrowColor;

	public Color completedRoundArrowColor;

	public Color failedRoundArrowColor;

	public Color roundNameColor;

	public Color currentRoundNameColor;

	public Color completedRoundNameColor;

	public Color failedRoundNameColor;

	private int currentRound;

	public virtual void Start()
	{
	}

	public virtual void SetCurrentRound(int r, bool failed, bool allowDimmingOfChampionship)
	{
		currentRound = r;
		for (int i = 0; i < roundNames.Length; i++)
		{
			Image image = roundArrows[i];
			Text text = roundNames[i];
			Image image2 = roundDimmers[i];
			if (i == currentRound - 1)
			{
				if (failed)
				{
					image.color = failedRoundArrowColor;
					text.color = failedRoundNameColor;
					image2.gameObject.SetActive(true);
				}
				else
				{
					image.color = currentRoundArrowColor;
					text.color = currentRoundNameColor;
					image2.gameObject.SetActive(false);
				}
			}
			else if (i < currentRound - 1)
			{
				image.color = completedRoundArrowColor;
				text.color = completedRoundNameColor;
				image2.gameObject.SetActive(true);
			}
			else if (failed)
			{
				image.color = failedRoundArrowColor;
				text.color = failedRoundNameColor;
				image2.gameObject.SetActive(true);
			}
			else
			{
				image.color = roundArrowColor;
				text.color = roundNameColor;
				image2.gameObject.SetActive(true);
			}
		}
		if (!allowDimmingOfChampionship)
		{
			roundDimmers[2].gameObject.SetActive(false);
		}
	}

	public virtual void Update()
	{
	}
}
