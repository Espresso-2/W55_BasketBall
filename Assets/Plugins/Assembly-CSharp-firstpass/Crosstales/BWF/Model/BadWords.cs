using System;
using System.Collections.Generic;
using System.Text;
using Crosstales.BWF.Data;
using Crosstales.Common.Util;

namespace Crosstales.BWF.Model
{
	[Serializable]
	public class BadWords
	{
		public Crosstales.BWF.Data.Source Source;

		public List<string> BadWordList = new List<string>();

		public BadWords(Crosstales.BWF.Data.Source source, List<string> badWordList)
		{
			Source = source;
			foreach (string badWord in badWordList)
			{
				BadWordList.Add(badWord.Split('#')[0]);
			}
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(GetType().Name);
			stringBuilder.Append(BaseConstants.TEXT_TOSTRING_START);
			stringBuilder.Append("Source='");
			stringBuilder.Append(Source);
			stringBuilder.Append(BaseConstants.TEXT_TOSTRING_DELIMITER);
			stringBuilder.Append("BadWordList='");
			stringBuilder.Append(BadWordList);
			stringBuilder.Append(BaseConstants.TEXT_TOSTRING_DELIMITER_END);
			stringBuilder.Append(BaseConstants.TEXT_TOSTRING_END);
			return stringBuilder.ToString();
		}
	}
}
