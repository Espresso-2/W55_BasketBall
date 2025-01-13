using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Crosstales.BWF.Data;
using Crosstales.BWF.Provider;
using Crosstales.BWF.Util;
using Crosstales.Common.Util;
using UnityEngine;

namespace Crosstales.BWF.Filter
{
	public class BadWordFilter : BaseFilter
	{
		public string ReplaceCharacters;

		public bool ReplaceLeetSpeak;

		public bool SimpleCheck;

		private readonly List<BadWordProvider> tempBadWordProviderLTR;

		private readonly List<BadWordProvider> tempBadWordProviderRTL;

		private readonly Dictionary<string, Regex> exactBadwordsRegex = new Dictionary<string, Regex>(30);

		private readonly Dictionary<string, List<Regex>> debugExactBadwordsRegex = new Dictionary<string, List<Regex>>(30);

		private readonly Dictionary<string, List<string>> simpleBadwords = new Dictionary<string, List<string>>(30);

		private bool ready;

		private bool readyFirstime;

		private List<BadWordProvider> badWordProviderLTR = new List<BadWordProvider>();

		private List<BadWordProvider> badWordProviderRTL = new List<BadWordProvider>();

		public List<BadWordProvider> BadWordProviderLTR
		{
			get
			{
				return badWordProviderLTR;
			}
			set
			{
				badWordProviderLTR = value;
				if (badWordProviderLTR != null && badWordProviderLTR.Count > 0)
				{
					foreach (BadWordProvider item in badWordProviderLTR)
					{
						if (item != null)
						{
							if (Config.DEBUG_BADWORDS)
							{
								debugExactBadwordsRegex.CTAddRange(item.DebugExactBadwordsRegex);
							}
							else
							{
								exactBadwordsRegex.CTAddRange(item.ExactBadwordsRegex);
							}
							simpleBadwords.CTAddRange(item.SimpleBadwords);
						}
						else if (!BaseHelper.isEditorMode)
						{
							Debug.LogError("A LTR-BadWordProvider is null!");
						}
					}
					return;
				}
				badWordProviderLTR = new List<BadWordProvider>();
				if (!BaseHelper.isEditorMode)
				{
					Debug.LogWarning("No 'BadWordProviderLTR' added!" + Environment.NewLine + "If you want to use this functionality, please add your desired 'BadWordProviderLTR' in the editor or script.");
				}
			}
		}

		public List<BadWordProvider> BadWordProviderRTL
		{
			get
			{
				return badWordProviderRTL;
			}
			set
			{
				badWordProviderRTL = value;
				if (badWordProviderRTL != null && badWordProviderRTL.Count > 0)
				{
					foreach (BadWordProvider item in badWordProviderRTL)
					{
						if (item != null)
						{
							if (Config.DEBUG_BADWORDS)
							{
								debugExactBadwordsRegex.CTAddRange(item.DebugExactBadwordsRegex);
							}
							else
							{
								exactBadwordsRegex.CTAddRange(item.ExactBadwordsRegex);
							}
							simpleBadwords.CTAddRange(item.SimpleBadwords);
						}
						else if (!BaseHelper.isEditorMode)
						{
							Debug.LogError("A RTL-BadWordProvider is null!");
						}
					}
					return;
				}
				badWordProviderRTL = new List<BadWordProvider>();
				if (!BaseHelper.isEditorMode)
				{
					Debug.LogWarning("No 'BadWordProviderRTL' added!" + Environment.NewLine + "If you want to use this functionality, please add your desired 'BadWordProviderRTL' in the editor or script.");
				}
			}
		}

		public override bool isReady
		{
			get
			{
				bool flag = true;
				if (!ready)
				{
					if (tempBadWordProviderLTR != null)
					{
						foreach (BadWordProvider item in tempBadWordProviderLTR)
						{
							if (item != null && !item.isReady)
							{
								flag = false;
								break;
							}
						}
					}
					if (flag && tempBadWordProviderRTL != null)
					{
						foreach (BadWordProvider item2 in tempBadWordProviderRTL)
						{
							if (item2 != null && !item2.isReady)
							{
								flag = false;
								break;
							}
						}
					}
					if (!readyFirstime && flag)
					{
						BadWordProviderLTR = tempBadWordProviderLTR;
						BadWordProviderRTL = tempBadWordProviderRTL;
						if (BadWordProviderLTR != null)
						{
							foreach (BadWordProvider item3 in BadWordProviderLTR)
							{
								if (!(item3 != null))
								{
									continue;
								}
								Source[] array = item3.Sources;
								foreach (Source source in array)
								{
									if (source != null && !sources.ContainsKey(source.Name))
									{
										sources.Add(source.Name, source);
									}
								}
							}
						}
						if (BadWordProviderRTL != null)
						{
							foreach (BadWordProvider item4 in BadWordProviderRTL)
							{
								if (!(item4 != null))
								{
									continue;
								}
								Source[] array2 = item4.Sources;
								foreach (Source source2 in array2)
								{
									if (source2 != null && !sources.ContainsKey(source2.Name))
									{
										sources.Add(source2.Name, source2);
									}
								}
							}
						}
						readyFirstime = true;
					}
				}
				ready = flag;
				return flag;
			}
		}

		public BadWordFilter(List<BadWordProvider> badWordProviderLTR, List<BadWordProvider> badWordProviderRTL, string replaceCharacters, bool leetSpeak, bool simpleCheck)
		{
			tempBadWordProviderLTR = badWordProviderLTR;
			tempBadWordProviderRTL = badWordProviderRTL;
			ReplaceCharacters = replaceCharacters;
			ReplaceLeetSpeak = leetSpeak;
			SimpleCheck = simpleCheck;
		}

		public override bool Contains(string text, string[] sourceNames)
		{
			bool flag = false;
			if (isReady)
			{
				if (string.IsNullOrEmpty(text))
				{
					logContains();
				}
				else
				{
					string text2 = replaceLeetToText(text);
					if (Config.DEBUG_BADWORDS)
					{
						if (sourceNames == null || sourceNames.Length == 0)
						{
							if (SimpleCheck)
							{
								foreach (List<string> value5 in simpleBadwords.Values)
								{
									foreach (string item in value5)
									{
										if (text2.CTContains(item))
										{
											Debug.Log("Test string contains a bad word detected by word '" + item + "'");
											flag = true;
											break;
										}
									}
									if (flag)
									{
										break;
									}
								}
							}
							else
							{
								foreach (List<Regex> value6 in debugExactBadwordsRegex.Values)
								{
									foreach (Regex item2 in value6)
									{
										Match match = item2.Match(text2);
										if (match.Success)
										{
											Debug.Log(string.Concat("Test string contains a bad word: '", match.Value, "' detected by regex '", item2, "'"));
											flag = true;
											break;
										}
										if (flag)
										{
											break;
										}
									}
								}
							}
						}
						else
						{
							for (int i = 0; i < sourceNames.Length; i++)
							{
								if (flag)
								{
									break;
								}
								List<Regex> value2;
								if (SimpleCheck)
								{
									List<string> value;
									if (simpleBadwords.TryGetValue(sourceNames[i], out value))
									{
										foreach (string item3 in value)
										{
											if (text2.CTContains(item3))
											{
												Debug.Log("Test string contains a bad word detected by word '" + item3 + "'' from source '" + sourceNames[i] + "'");
												flag = true;
												break;
											}
										}
										if (flag)
										{
											break;
										}
									}
									else
									{
										logResourceNotFound(sourceNames[i]);
									}
								}
								else if (debugExactBadwordsRegex.TryGetValue(sourceNames[i], out value2))
								{
									foreach (Regex item4 in value2)
									{
										Match match = item4.Match(text2);
										if (match.Success)
										{
											Debug.Log(string.Concat("Test string contains a bad word: '", match.Value, "' detected by regex '", item4, "'' from source '", sourceNames[i], "'"));
											flag = true;
											break;
										}
									}
								}
								else
								{
									logResourceNotFound(sourceNames[i]);
								}
							}
						}
					}
					else if (sourceNames == null || sourceNames.Length == 0)
					{
						if (SimpleCheck)
						{
							foreach (List<string> value7 in simpleBadwords.Values)
							{
								foreach (string item5 in value7)
								{
									if (text2.CTContains(item5))
									{
										flag = true;
										break;
									}
								}
								if (flag)
								{
									break;
								}
							}
						}
						else
						{
							foreach (Regex value8 in exactBadwordsRegex.Values)
							{
								if (value8.Match(text2).Success)
								{
									flag = true;
									break;
								}
							}
						}
					}
					else
					{
						foreach (string text3 in sourceNames)
						{
							Regex value4;
							if (SimpleCheck)
							{
								List<string> value3;
								if (simpleBadwords.TryGetValue(text3, out value3))
								{
									foreach (string item6 in value3)
									{
										if (text2.CTContains(item6))
										{
											flag = true;
											break;
										}
									}
									if (flag)
									{
										break;
									}
								}
								else
								{
									logResourceNotFound(text3);
								}
							}
							else if (exactBadwordsRegex.TryGetValue(text3, out value4))
							{
								Match match = value4.Match(text2);
								if (match.Success)
								{
									flag = true;
									break;
								}
							}
							else
							{
								logResourceNotFound(text3);
							}
						}
					}
				}
			}
			else
			{
				logFilterNotReady();
			}
			return flag;
		}

		public override List<string> GetAll(string text, string[] sourceNames)
		{
			List<string> list = new List<string>();
			if (isReady)
			{
				if (string.IsNullOrEmpty(text))
				{
					logGetAll();
				}
				else
				{
					string text2 = replaceLeetToText(text);
					if (Config.DEBUG_BADWORDS)
					{
						if (sourceNames == null || sourceNames.Length == 0)
						{
							if (SimpleCheck)
							{
								foreach (List<string> value5 in simpleBadwords.Values)
								{
									foreach (string item in value5)
									{
										if (text2.CTContains(item))
										{
											Debug.Log("Test string contains a bad word detected by word '" + item + "'");
											if (!list.Contains(item))
											{
												list.Add(item);
											}
										}
									}
								}
							}
							else
							{
								foreach (List<Regex> value6 in debugExactBadwordsRegex.Values)
								{
									foreach (Regex item2 in value6)
									{
										MatchCollection matchCollection = item2.Matches(text2);
										foreach (Match item3 in matchCollection)
										{
											foreach (Capture capture5 in item3.Captures)
											{
												Debug.Log(string.Concat("Test string contains a bad word: '", capture5.Value, "' detected by regex '", item2, "'"));
												if (!list.Contains(capture5.Value))
												{
													list.Add(capture5.Value);
												}
											}
										}
									}
								}
							}
						}
						else
						{
							foreach (string text3 in sourceNames)
							{
								List<Regex> value2;
								if (SimpleCheck)
								{
									List<string> value;
									if (simpleBadwords.TryGetValue(text3, out value))
									{
										foreach (string item4 in value)
										{
											if (text2.CTContains(item4))
											{
												Debug.Log("Test string contains a bad word detected by word '" + item4 + "'' from source '" + text3 + "'");
												if (!list.Contains(item4))
												{
													list.Add(item4);
												}
											}
										}
									}
									else
									{
										logResourceNotFound(text3);
									}
								}
								else if (debugExactBadwordsRegex.TryGetValue(text3, out value2))
								{
									foreach (Regex item5 in value2)
									{
										MatchCollection matchCollection2 = item5.Matches(text2);
										foreach (Match item6 in matchCollection2)
										{
											foreach (Capture capture6 in item6.Captures)
											{
												Debug.Log(string.Concat("Test string contains a bad word: '", capture6.Value, "' detected by regex '", item5, "'' from source '", text3, "'"));
												if (!list.Contains(capture6.Value))
												{
													list.Add(capture6.Value);
												}
											}
										}
									}
								}
								else
								{
									logResourceNotFound(text3);
								}
							}
						}
					}
					else if (sourceNames == null || sourceNames.Length == 0)
					{
						if (SimpleCheck)
						{
							foreach (List<string> value7 in simpleBadwords.Values)
							{
								foreach (string item7 in value7)
								{
									if (text2.CTContains(item7) && !list.Contains(item7))
									{
										list.Add(item7);
									}
								}
							}
						}
						else
						{
							foreach (Regex value8 in exactBadwordsRegex.Values)
							{
								MatchCollection matchCollection3 = value8.Matches(text2);
								foreach (Match item8 in matchCollection3)
								{
									foreach (Capture capture7 in item8.Captures)
									{
										if (!list.Contains(capture7.Value))
										{
											list.Add(capture7.Value);
										}
									}
								}
							}
						}
					}
					else
					{
						foreach (string text4 in sourceNames)
						{
							Regex value4;
							if (SimpleCheck)
							{
								List<string> value3;
								if (simpleBadwords.TryGetValue(text4, out value3))
								{
									foreach (string item9 in value3)
									{
										if (text2.CTContains(item9) && !list.Contains(item9))
										{
											list.Add(item9);
										}
									}
								}
								else
								{
									logResourceNotFound(text4);
								}
							}
							else if (exactBadwordsRegex.TryGetValue(text4, out value4))
							{
								MatchCollection matchCollection4 = value4.Matches(text2);
								foreach (Match item10 in matchCollection4)
								{
									foreach (Capture capture8 in item10.Captures)
									{
										if (!list.Contains(capture8.Value))
										{
											list.Add(capture8.Value);
										}
									}
								}
							}
							else
							{
								logResourceNotFound(text4);
							}
						}
					}
				}
			}
			else
			{
				logFilterNotReady();
			}
			return (from x in list.Distinct()
				orderby x
				select x).ToList();
		}

		public override string ReplaceAll(string text, bool markOnly, string prefix = "", string postfix = "", string[] sourceNames = null)
		{
			string text2 = string.Empty;
			bool flag = false;
			if (isReady)
			{
				if (string.IsNullOrEmpty(text))
				{
					logReplaceAll();
				}
				else
				{
					string text3 = (text2 = replaceLeetToText(text));
					if (SimpleCheck)
					{
						foreach (string item in GetAll(text3, sourceNames))
						{
							text3 = Regex.Replace(text3, item, BaseHelper.CreateString(ReplaceCharacters, item.Length), RegexOptions.IgnoreCase);
							flag = true;
						}
						text2 = text3;
					}
					else if (Config.DEBUG_BADWORDS)
					{
						if (sourceNames == null || sourceNames.Length == 0)
						{
							foreach (List<Regex> value3 in debugExactBadwordsRegex.Values)
							{
								foreach (Regex item2 in value3)
								{
									MatchCollection matchCollection = item2.Matches(text3);
									foreach (Match item3 in matchCollection)
									{
										foreach (Capture capture5 in item3.Captures)
										{
											Debug.Log(string.Concat("Test string contains a bad word: '", capture5.Value, "' detected by regex '", item2, "'"));
											text2 = replaceCapture(text2, capture5, markOnly, prefix, postfix, text2.Length - text3.Length);
											flag = true;
										}
									}
								}
							}
						}
						else
						{
							foreach (string text4 in sourceNames)
							{
								List<Regex> value;
								if (debugExactBadwordsRegex.TryGetValue(text4, out value))
								{
									foreach (Regex item4 in value)
									{
										MatchCollection matchCollection2 = item4.Matches(text3);
										foreach (Match item5 in matchCollection2)
										{
											foreach (Capture capture6 in item5.Captures)
											{
												Debug.Log(string.Concat("Test string contains a bad word: '", capture6.Value, "' detected by regex '", item4, "'' from source '", text4, "'"));
												text2 = replaceCapture(text2, capture6, markOnly, prefix, postfix, text2.Length - text3.Length);
												flag = true;
											}
										}
									}
								}
								else
								{
									logResourceNotFound(text4);
								}
							}
						}
					}
					else if (sourceNames == null || sourceNames.Length == 0)
					{
						foreach (Regex value4 in exactBadwordsRegex.Values)
						{
							MatchCollection matchCollection3 = value4.Matches(text3);
							foreach (Match item6 in matchCollection3)
							{
								foreach (Capture capture7 in item6.Captures)
								{
									text2 = replaceCapture(text2, capture7, markOnly, prefix, postfix, text2.Length - text3.Length);
									flag = true;
								}
							}
						}
					}
					else
					{
						foreach (string text5 in sourceNames)
						{
							Regex value2;
							if (exactBadwordsRegex.TryGetValue(text5, out value2))
							{
								MatchCollection matchCollection4 = value2.Matches(text3);
								foreach (Match item7 in matchCollection4)
								{
									foreach (Capture capture8 in item7.Captures)
									{
										text2 = replaceCapture(text2, capture8, markOnly, prefix, postfix, text2.Length - text3.Length);
										flag = true;
									}
								}
							}
							else
							{
								logResourceNotFound(text5);
							}
						}
					}
				}
			}
			else
			{
				logFilterNotReady();
			}
			if (flag)
			{
				return text2;
			}
			return text;
		}

		private string replaceCapture(string text, Capture capture, bool markOnly, string prefix, string postfix, int offset)
		{
			StringBuilder stringBuilder = new StringBuilder(text);
			string value = ((!markOnly) ? (prefix + BaseHelper.CreateString(ReplaceCharacters, capture.Value.Length) + postfix) : (prefix + capture.Value + postfix));
			stringBuilder.Remove(capture.Index + offset, capture.Value.Length);
			stringBuilder.Insert(capture.Index + offset, value);
			return stringBuilder.ToString();
		}

		protected string replaceLeetToText(string input)
		{
			string text = input;
			if (ReplaceLeetSpeak && !string.IsNullOrEmpty(input))
			{
				text = text.Replace("@", "a");
				text = text.Replace("4", "a");
				text = text.Replace("^", "a");
				text = text.Replace("8", "b");
				text = text.Replace("©", "c");
				text = text.Replace('¢', 'c');
				text = text.Replace("€", "e");
				text = text.Replace("3", "e");
				text = text.Replace("£", "e");
				text = text.Replace("ƒ", "f");
				text = text.Replace("6", "g");
				text = text.Replace("9", "g");
				text = text.Replace("#", "h");
				text = text.Replace("1", "i");
				text = text.Replace("|", "i");
				text = text.Replace("0", "o");
				text = text.Replace("2", "r");
				text = text.Replace("®", "r");
				text = text.Replace("$", "s");
				text = text.Replace("5", "s");
				text = text.Replace("§", "s");
				text = text.Replace("7", "t");
				text = text.Replace("+", "t");
				text = text.Replace("†", "t");
				text = text.Replace("¥", "y");
			}
			return text;
		}

		protected string replaceTextToLeet(string input, bool obvious = true)
		{
			string text = input;
			if (ReplaceLeetSpeak && !string.IsNullOrEmpty(input))
			{
				if (obvious)
				{
					text = text.Replace("s", "$");
				}
				else
				{
					text = text.Replace("a", "@");
					text = text.Replace("b", "8");
					text = text.Replace("e", "3");
					text = text.Replace("g", "9");
					text = text.Replace("i", "1");
					text = text.Replace("o", "0");
					text = text.Replace("r", "2");
					text = text.Replace("s", "$");
					text = text.Replace("t", "7");
				}
			}
			return text;
		}
	}
}
