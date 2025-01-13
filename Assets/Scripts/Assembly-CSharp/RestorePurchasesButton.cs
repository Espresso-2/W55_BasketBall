using System;
using I2.Loc;
using UnityEngine;

[Serializable]
public class RestorePurchasesButton : MonoBehaviour
{
	public Localize text;

	private GameSounds gameSounds;

	public virtual void OnEnable()
	{
		gameSounds = GameSounds.GetInstance();
		text.SetTerm("RESTORE PURCHASES", null);
	}

	public virtual void OnClick()
	{
		gameSounds.Play_select();
		text.SetTerm("RESTORING", null);
		Unibiller.restoreTransactions();
	}

	public virtual void Restored()
	{
		gameSounds.Play_ascend_chime_bright();
		text.SetTerm("RESTORED", null);
	}
}
