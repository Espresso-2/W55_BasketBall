using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Crosstales.BWF.Data;
using Crosstales.BWF.Util;
using UnityEngine;

namespace Crosstales.BWF.Provider
{
	[ExecuteInEditMode]
	public abstract class BaseProvider : MonoBehaviour, IProvider
	{
		[Header("Regex Options")]
		[Tooltip("Option1 (default: RegexOptions.IgnoreCase).")]
		public RegexOptions RegexOption1 = RegexOptions.IgnoreCase;

		[Tooltip("Option2 (default: RegexOptions.CultureInvariant).")]
		public RegexOptions RegexOption2 = RegexOptions.CultureInvariant;

		[Tooltip("Option3 (default: RegexOptions.None).")]
		public RegexOptions RegexOption3;

		[Tooltip("Option4 (default: RegexOptions.None).")]
		public RegexOptions RegexOption4;

		[Tooltip("Option5 (default: RegexOptions.None).")]
		public RegexOptions RegexOption5;

		[Header("Sources")]
		[Tooltip("All sources for this provider.")]
		[ContextMenuItem("Create Source", "createSource")]
		public Source[] Sources;

		[Header("Load Behaviour")]
		[Tooltip("Clears all existing bad words on 'Load' (default: true).")]
		public bool ClearOnLoad = true;

		protected List<string> coRoutines = new List<string>();

		protected static bool loggedUnsupportedPlatform;

		protected bool loading;

		public bool isReady { get; set; }

		public abstract void Load();

		public abstract void Save();

		protected abstract void init();

		public void Awake()
		{
			Load();
		}

		protected void logNoResourcesAdded()
		{
			Debug.LogWarning("No 'Resources' for " + base.name + " added!" + Environment.NewLine + "If you want to use this functionality, please add your desired 'Resources'.");
		}

		protected void createSource()
		{
			Helper.CreateSource();
		}
	}
}
