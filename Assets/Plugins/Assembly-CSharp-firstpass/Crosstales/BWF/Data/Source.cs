using System.Text;
using Crosstales.Common.Util;
using UnityEngine;

namespace Crosstales.BWF.Data
{
	[HelpURL("https://www.crosstales.com/media/data/assets/badwordfilter/api/class_crosstales_1_1_b_w_f_1_1_data_1_1_source.html")]
	[CreateAssetMenu(fileName = "New Source", menuName = "Bad Word Filter PRO/Source", order = 1000)]
	public class Source : ScriptableObject
	{
		[Header("Information")]
		[Tooltip("Name of the source.")]
		public string Name = string.Empty;

		[Tooltip("Description for the source (optional).")]
		public string Description = string.Empty;

		[Tooltip("Icon to represent the source (e.g. country flag, optional)")]
		public Sprite Icon;

		[Header("Settings")]
		[Tooltip("URL of a text file containing all regular expressions for this source. Add also the protocol-type ('http://', 'file://' etc.).")]
		public string URL = string.Empty;

		[Tooltip("Text file containing all regular expressions for this source.")]
		public TextAsset Resource;

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(GetType().Name);
			stringBuilder.Append(BaseConstants.TEXT_TOSTRING_START);
			stringBuilder.Append("Name='");
			stringBuilder.Append(Name);
			stringBuilder.Append(BaseConstants.TEXT_TOSTRING_DELIMITER);
			stringBuilder.Append("Description='");
			stringBuilder.Append(Description);
			stringBuilder.Append(BaseConstants.TEXT_TOSTRING_DELIMITER);
			stringBuilder.Append("Icon='");
			stringBuilder.Append(Icon);
			stringBuilder.Append(BaseConstants.TEXT_TOSTRING_DELIMITER_END);
			stringBuilder.Append(BaseConstants.TEXT_TOSTRING_END);
			return stringBuilder.ToString();
		}
	}
}
