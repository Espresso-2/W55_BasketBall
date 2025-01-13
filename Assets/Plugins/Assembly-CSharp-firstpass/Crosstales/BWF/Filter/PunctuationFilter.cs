using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Crosstales.Common.Util;
using UnityEngine;

namespace Crosstales.BWF.Filter
{
	public class PunctuationFilter : BaseFilter
	{
		private int characterNumber;

		public Regex RegularExpression { get; private set; }

		public int CharacterNumber
		{
			get
			{
				return characterNumber;
			}
			set
			{
				if (value < 2)
				{
					characterNumber = 2;
				}
				else
				{
					characterNumber = value;
				}
				RegularExpression = new Regex("[?!,.;:-]{" + (characterNumber + 1) + ",}", RegexOptions.CultureInvariant);
			}
		}

		public override bool isReady
		{
			get
			{
				return true;
			}
		}

		public PunctuationFilter(int punctuationCharacterNumber)
		{
			CharacterNumber = punctuationCharacterNumber;
		}

		public override bool Contains(string text, string[] sources)
		{
			bool result = false;
			if (string.IsNullOrEmpty(text))
			{
				logContains();
			}
			else
			{
				result = RegularExpression.Match(text).Success;
			}
			return result;
		}

		public override List<string> GetAll(string text, string[] sources)
		{
			List<string> list = new List<string>();
			if (string.IsNullOrEmpty(text))
			{
				logGetAll();
			}
			else
			{
				MatchCollection matchCollection = RegularExpression.Matches(text);
				foreach (Match item in matchCollection)
				{
					foreach (Capture capture in item.Captures)
					{
						if (BaseConstants.DEV_DEBUG)
						{
							Debug.Log("Test string contains an excessive punctuation: '" + capture.Value + "'");
						}
						if (!list.Contains(capture.Value))
						{
							list.Add(capture.Value);
						}
					}
				}
			}
			return (from x in list.Distinct()
				orderby x
				select x).ToList();
		}

		public override string ReplaceAll(string text, bool markOnly = false, string prefix = "", string postfix = "", string[] sourceNames = null)
		{
			string text2 = text;
			if (string.IsNullOrEmpty(text))
			{
				logReplaceAll();
				text2 = string.Empty;
			}
			else
			{
				MatchCollection matchCollection = RegularExpression.Matches(text);
				foreach (Match item in matchCollection)
				{
					foreach (Capture capture in item.Captures)
					{
						if (BaseConstants.DEV_DEBUG)
						{
							Debug.Log("Test string contains an excessive punctuation: '" + capture.Value + "'");
						}
						text2 = text2.Replace(capture.Value, (!markOnly) ? (prefix + capture.Value.Substring(0, characterNumber) + postfix) : (prefix + capture.Value + postfix));
					}
				}
			}
			return text2;
		}
	}
}
