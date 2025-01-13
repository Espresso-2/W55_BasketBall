using System;
using System.Collections.Generic;
using System.Linq;
using Crosstales.BWF.Data;
using Crosstales.Common.Util;
using UnityEngine;

namespace Crosstales.BWF.Filter
{
	public abstract class BaseFilter : IFilter
	{
		protected Dictionary<string, Source> sources = new Dictionary<string, Source>();

		public virtual List<Source> Sources
		{
			get
			{
				List<Source> result = new List<Source>();
				if (isReady)
				{
					result = (from x in sources
						orderby x.Key
						select x into y
						select y.Value).ToList();
				}
				else
				{
					logFilterNotReady();
				}
				return result;
			}
		}

		public abstract bool isReady { get; }

		public abstract bool Contains(string text, params string[] sourceNames);

		public abstract List<string> GetAll(string text, params string[] sourceNames);

		public abstract string ReplaceAll(string text, bool markOnly = false, string prefix = "", string postfix = "", params string[] sourceNames);

		public virtual string Unmark(string text, string prefix = "<b><color=red>", string postfix = "</color></b>")
		{
			string text2 = text;
			string oldValue = prefix;
			string oldValue2 = postfix;
			if (string.IsNullOrEmpty(text))
			{
				if (BaseConstants.DEV_DEBUG)
				{
					Debug.LogWarning("Parameter 'text' is null or empty!" + Environment.NewLine + "=> 'Unmark()' will return an empty string.");
				}
				return string.Empty;
			}
			if (string.IsNullOrEmpty(prefix))
			{
				if (BaseConstants.DEV_DEBUG)
				{
					Debug.LogWarning("Parameter 'prefix' is null!" + Environment.NewLine + "=> Using an empty string as prefix.");
				}
				oldValue = string.Empty;
			}
			if (string.IsNullOrEmpty(postfix))
			{
				if (BaseConstants.DEV_DEBUG)
				{
					Debug.LogWarning("Parameter 'postfix' is null!" + Environment.NewLine + "=> Using an empty string as postfix.");
				}
				oldValue2 = string.Empty;
			}
			text2 = text2.Replace(oldValue, string.Empty);
			return text2.Replace(oldValue2, string.Empty);
		}

		public virtual string Mark(string text, bool replace = false, string prefix = "<b><color=red>", string postfix = "</color></b>", params string[] sourceNames)
		{
			return ReplaceAll(text, !replace, prefix, postfix, sourceNames);
		}

		protected void logFilterNotReady()
		{
			Debug.LogWarning("Filter is not ready - please wait until 'isReady' returns true.");
		}

		protected void logResourceNotFound(string res)
		{
			if (BaseConstants.DEV_DEBUG)
			{
				Debug.LogWarning("Resource not found: '" + res + "'" + Environment.NewLine + "Did you call the method with the correct resource name?");
			}
		}

		protected void logContains()
		{
			if (BaseConstants.DEV_DEBUG)
			{
				Debug.LogWarning("Parameter 'text' is null or empty!" + Environment.NewLine + "=> 'Contains()' will return 'false'.");
			}
		}

		protected void logGetAll()
		{
			if (BaseConstants.DEV_DEBUG)
			{
				Debug.LogWarning("Parameter 'text' is null or empty!" + Environment.NewLine + "=> 'GetAll()' will return an empty list.");
			}
		}

		protected void logReplaceAll()
		{
			if (BaseConstants.DEV_DEBUG)
			{
				Debug.LogWarning("Parameter 'text' is null or empty!" + Environment.NewLine + "=> 'ReplaceAll()' will return an empty string.");
			}
		}

		protected void logReplace()
		{
			if (BaseConstants.DEV_DEBUG)
			{
				Debug.LogWarning("Parameter 'text' is null or empty!" + Environment.NewLine + "=> 'Replace()' will return an empty string.");
			}
		}
	}
}
