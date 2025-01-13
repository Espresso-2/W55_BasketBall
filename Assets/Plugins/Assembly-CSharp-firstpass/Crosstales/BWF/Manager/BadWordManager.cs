using System;
using System.Collections.Generic;
using Crosstales.BWF.Data;
using Crosstales.BWF.Filter;
using Crosstales.BWF.Provider;
using Crosstales.Common.Util;
using UnityEngine;

namespace Crosstales.BWF.Manager
{
	[DisallowMultipleComponent]
	[HelpURL("https://www.crosstales.com/media/data/assets/badwordfilter/api/class_crosstales_1_1_b_w_f_1_1_manager_1_1_bad_word_manager.html")]
	public class BadWordManager : BaseManager
	{
		[Header("Specific Settings")]
		[Tooltip("Replace characters for bad words (default: *).")]
		public string ReplaceChars = "*";

		[Tooltip("Replace Leet speak in the input string (default: true).")]
		public bool ReplaceLeetSpeak;

		[Tooltip("Use simple detection algorithm. This is the way to check for Chinese, Japanese, Korean and Thai bad words (default: false).")]
		public bool SimpleCheck;

		[Header("Bad Word Providers")]
		[Tooltip("List of all left-to-right providers.")]
		public List<BadWordProvider> BadWordProviderLTR;

		[Tooltip("List of all right-to-left providers.")]
		public List<BadWordProvider> BadWordProviderRTL;

		private static BadWordFilter filter;

		private static BadWordManager instance;

		private static bool loggedFilterIsNull;

		private static bool loggedOnlyOneInstance;

		private const string clazz = "BadWordManager";

		public static string ReplaceCharacters
		{
			get
			{
				if (filter != null)
				{
					return filter.ReplaceCharacters;
				}
				if (instance != null)
				{
					return instance.ReplaceChars;
				}
				return "*";
			}
			set
			{
				if (filter != null)
				{
					filter.ReplaceCharacters = value;
					instance.ReplaceChars = value;
				}
				else if (instance != null)
				{
					instance.ReplaceChars = value;
				}
			}
		}

		public static bool isReplaceLeetSpeak
		{
			get
			{
				if (filter != null)
				{
					return filter.ReplaceLeetSpeak;
				}
				if (instance != null)
				{
					return instance.ReplaceLeetSpeak;
				}
				return false;
			}
			set
			{
				if (filter != null)
				{
					filter.ReplaceLeetSpeak = value;
					instance.ReplaceLeetSpeak = value;
				}
				else if (instance != null)
				{
					instance.ReplaceLeetSpeak = value;
				}
			}
		}

		public static bool isSimpleCheck
		{
			get
			{
				if (filter != null)
				{
					return filter.SimpleCheck;
				}
				if (instance != null)
				{
					return instance.SimpleCheck;
				}
				return false;
			}
			set
			{
				if (filter != null)
				{
					filter.SimpleCheck = value;
					instance.SimpleCheck = value;
				}
				else if (instance != null)
				{
					instance.SimpleCheck = value;
				}
			}
		}

		public static bool isReady
		{
			get
			{
				bool result = false;
				if (filter != null)
				{
					result = filter.isReady;
				}
				else
				{
					logFilterIsNull("BadWordManager");
				}
				return result;
			}
		}

		public static List<Source> Sources
		{
			get
			{
				List<Source> result = new List<Source>();
				if (filter != null)
				{
					result = filter.Sources;
				}
				else
				{
					logFilterIsNull("BadWordManager");
				}
				return result;
			}
		}

		public void OnEnable()
		{
			if (instance == null)
			{
				instance = this;
				Load();
				if (!BaseHelper.isEditorMode && DontDestroy)
				{
					UnityEngine.Object.DontDestroyOnLoad(base.transform.root.gameObject);
				}
			}
			else if (!BaseHelper.isEditorMode && DontDestroy && instance != this)
			{
				if (!loggedOnlyOneInstance)
				{
					loggedOnlyOneInstance = true;
					Debug.LogWarning("Only one active instance of 'BadWordManager' allowed in all scenes!" + Environment.NewLine + "This object will now be destroyed.");
				}
				UnityEngine.Object.Destroy(base.transform.root.gameObject, 0.2f);
			}
		}

		public static void Reset()
		{
			filter = null;
			instance = null;
			loggedFilterIsNull = false;
			loggedOnlyOneInstance = false;
		}

		public static void Load()
		{
			if (instance != null)
			{
				filter = new BadWordFilter(instance.BadWordProviderLTR, instance.BadWordProviderRTL, instance.ReplaceChars, instance.ReplaceLeetSpeak, instance.SimpleCheck);
			}
		}

		public static bool Contains(string text, params string[] sourceNames)
		{
			bool result = false;
			if (!string.IsNullOrEmpty(text))
			{
				if (filter != null)
				{
					result = filter.Contains(text, sourceNames);
				}
				else
				{
					logFilterIsNull("BadWordManager");
				}
			}
			return result;
		}

		public static void ContainsMT(out bool result, string text, params string[] sourceNames)
		{
			result = Contains(text, sourceNames);
		}

		public static List<string> GetAll(string text, params string[] sourceNames)
		{
			List<string> result = new List<string>();
			if (!string.IsNullOrEmpty(text))
			{
				if (filter != null)
				{
					result = filter.GetAll(text, sourceNames);
				}
				else
				{
					logFilterIsNull("BadWordManager");
				}
			}
			return result;
		}

		public static void GetAllMT(out List<string> result, string text, params string[] sourceNames)
		{
			result = GetAll(text, sourceNames);
		}

		public static string ReplaceAll(string text, bool markOnly = false, string prefix = "", string postfix = "", params string[] sourceNames)
		{
			string result = text;
			if (!string.IsNullOrEmpty(text))
			{
				if (filter != null)
				{
					result = filter.ReplaceAll(text, markOnly, prefix, postfix, sourceNames);
				}
				else
				{
					logFilterIsNull("BadWordManager");
				}
			}
			return result;
		}

		public static void ReplaceAllMT(out string result, string text, bool markOnly = false, string prefix = "", string postfix = "", params string[] sourceNames)
		{
			result = ReplaceAll(text, markOnly, prefix, postfix, sourceNames);
		}

		public static string Mark(string text, bool replace = false, string prefix = "<b><color=red>", string postfix = "</color></b>", params string[] sourceNames)
		{
			string result = text;
			if (!string.IsNullOrEmpty(text))
			{
				if (filter != null)
				{
					result = filter.Mark(text, replace, prefix, postfix, sourceNames);
				}
				else
				{
					logFilterIsNull("BadWordManager");
				}
			}
			return result;
		}

		public static string Unmark(string text, string prefix = "<b><color=red>", string postfix = "</color></b>")
		{
			string result = text;
			if (!string.IsNullOrEmpty(text))
			{
				if (filter != null)
				{
					result = filter.Unmark(text, prefix, postfix);
				}
				else
				{
					logFilterIsNull("BadWordManager");
				}
			}
			return result;
		}

		private static void logFilterIsNull(string clazz)
		{
			if (!loggedFilterIsNull)
			{
				Debug.LogWarning("'filter' is null!" + Environment.NewLine + "Did you add the '" + clazz + "' to the current scene?");
				loggedFilterIsNull = true;
			}
		}
	}
}
