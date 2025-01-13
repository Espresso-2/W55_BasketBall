using System;
using System.Collections.Generic;
using Crosstales.BWF.Filter;
using Crosstales.Common.Util;
using UnityEngine;

namespace Crosstales.BWF.Manager
{
	[DisallowMultipleComponent]
	[HelpURL("https://www.crosstales.com/media/data/assets/badwordfilter/api/class_crosstales_1_1_b_w_f_1_1_manager_1_1_capitalization_manager.html")]
	public class CapitalizationManager : BaseManager
	{
		[Header("Specific Settings")]
		[Tooltip("Defines the number of allowed capital letters in a row. (default: 3).")]
		public int CapitalizationCharsNumber = 3;

		private static CapitalizationFilter filter;

		private static CapitalizationManager instance;

		private static bool loggedFilterIsNull;

		private static bool loggedOnlyOneInstance;

		private const string clazz = "CapitalizationManager";

		public static int CharacterNumber
		{
			get
			{
				if (filter != null)
				{
					return filter.CharacterNumber;
				}
				if (instance != null)
				{
					return instance.CapitalizationCharsNumber;
				}
				return 3;
			}
			set
			{
				int num = value;
				if (num < 2)
				{
					num = 2;
				}
				if (filter != null)
				{
					filter.CharacterNumber = num;
					instance.CapitalizationCharsNumber = num;
				}
				else if (instance != null)
				{
					instance.CapitalizationCharsNumber = num;
				}
			}
		}

		public static bool isReady
		{
			get
			{
				return filter.isReady;
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
					Debug.LogWarning("Only one active instance of 'CapitalizationManager' allowed in all scenes!" + Environment.NewLine + "This object will now be destroyed.");
				}
				UnityEngine.Object.Destroy(base.transform.root.gameObject, 0.2f);
			}
		}

		public void OnValidate()
		{
			if (CapitalizationCharsNumber < 2)
			{
				CapitalizationCharsNumber = 2;
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
				filter = new CapitalizationFilter(instance.CapitalizationCharsNumber);
			}
		}

		public static bool Contains(string text)
		{
			bool result = false;
			if (!string.IsNullOrEmpty(text))
			{
				if (filter != null)
				{
					result = filter.Contains(text, new string[0]);
				}
				else
				{
					logFilterIsNull("CapitalizationManager");
				}
			}
			return result;
		}

		public static void ContainsMT(out bool result, string text)
		{
			result = Contains(text);
		}

		public static List<string> GetAll(string text)
		{
			List<string> result = new List<string>();
			if (!string.IsNullOrEmpty(text))
			{
				if (filter != null)
				{
					result = filter.GetAll(text, new string[0]);
				}
				else
				{
					logFilterIsNull("CapitalizationManager");
				}
			}
			return result;
		}

		public static void GetAllMT(out List<string> result, string text)
		{
			result = GetAll(text);
		}

		public static string ReplaceAll(string text, bool markOnly = false, string prefix = "", string postfix = "")
		{
			string result = text;
			if (!string.IsNullOrEmpty(text))
			{
				if (filter != null)
				{
					result = filter.ReplaceAll(text, markOnly, prefix, postfix, new string[0]);
				}
				else
				{
					logFilterIsNull("CapitalizationManager");
				}
			}
			return result;
		}

		public static void ReplaceAllMT(out string result, string text, bool markOnly = false, string prefix = "", string postfix = "")
		{
			result = ReplaceAll(text, markOnly, prefix, postfix);
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
					logFilterIsNull("CapitalizationManager");
				}
			}
			return result;
		}

		public static string Mark(string text, bool replace = false, string prefix = "<b><color=red>", string postfix = "</color></b>")
		{
			string result = text;
			if (!string.IsNullOrEmpty(text))
			{
				if (filter != null)
				{
					result = filter.Mark(text, replace, prefix, postfix);
				}
				else
				{
					logFilterIsNull("CapitalizationManager");
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
