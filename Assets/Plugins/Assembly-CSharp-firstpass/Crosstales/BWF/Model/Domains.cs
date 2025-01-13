using System;
using System.Collections.Generic;
using System.Text;
using Crosstales.BWF.Data;
using Crosstales.Common.Util;

namespace Crosstales.BWF.Model
{
	[Serializable]
	public class Domains
	{
		public Crosstales.BWF.Data.Source Source;

		public List<string> DomainList = new List<string>();

		public Domains(Crosstales.BWF.Data.Source source, List<string> domainList)
		{
			Source = source;
			foreach (string domain in domainList)
			{
				DomainList.Add(domain.Split('#')[0]);
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
			stringBuilder.Append("DomainList='");
			stringBuilder.Append(DomainList);
			stringBuilder.Append(BaseConstants.TEXT_TOSTRING_DELIMITER_END);
			stringBuilder.Append(BaseConstants.TEXT_TOSTRING_END);
			return stringBuilder.ToString();
		}
	}
}
