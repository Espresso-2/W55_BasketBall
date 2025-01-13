using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Crosstales.BWF.Data;
using Crosstales.BWF.Provider;
using Crosstales.BWF.Util;
using Crosstales.Common.Util;
using UnityEngine;

namespace Crosstales.BWF.Filter
{
	public class DomainFilter : BaseFilter
	{
		public string ReplaceCharacters;

		private List<DomainProvider> domainProvider = new List<DomainProvider>();

		private readonly List<DomainProvider> tempDomainProvider;

		private readonly Dictionary<string, Regex> domainsRegex = new Dictionary<string, Regex>();

		private readonly Dictionary<string, List<Regex>> debugDomainsRegex = new Dictionary<string, List<Regex>>();

		private bool ready;

		private bool readyFirstime;

		public List<DomainProvider> DomainProvider
		{
			get
			{
				return domainProvider;
			}
			set
			{
				domainProvider = value;
				if (domainProvider != null && domainProvider.Count > 0)
				{
					foreach (DomainProvider item in domainProvider)
					{
						if (item != null)
						{
							if (Config.DEBUG_DOMAINS)
							{
								debugDomainsRegex.CTAddRange(item.DebugDomainsRegex);
							}
							else
							{
								domainsRegex.CTAddRange(item.DomainsRegex);
							}
						}
						else if (!BaseHelper.isEditorMode)
						{
							Debug.LogError("DomainProvider is null!");
						}
					}
					return;
				}
				domainProvider = new List<DomainProvider>();
				if (!BaseHelper.isEditorMode)
				{
					Debug.LogWarning("No 'DomainProvider' added!" + Environment.NewLine + "If you want to use this functionality, please add your desired 'DomainProvider' in the editor or script.");
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
					if (tempDomainProvider != null)
					{
						foreach (DomainProvider item in tempDomainProvider)
						{
							if (item != null && !item.isReady)
							{
								flag = false;
								break;
							}
						}
					}
					if (!readyFirstime && flag)
					{
						DomainProvider = tempDomainProvider;
						if (DomainProvider != null)
						{
							foreach (DomainProvider item2 in DomainProvider)
							{
								if (!(item2 != null))
								{
									continue;
								}
								Source[] array = item2.Sources;
								foreach (Source source in array)
								{
									if (source != null && !sources.ContainsKey(source.Name))
									{
										sources.Add(source.Name, source);
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

		public DomainFilter(List<DomainProvider> domainProvider, string replaceCharacters)
		{
			tempDomainProvider = domainProvider;
			ReplaceCharacters = replaceCharacters;
		}

		public override bool Contains(string text, string[] sourceNames)
		{
			bool result = false;
			if (isReady)
			{
				if (string.IsNullOrEmpty(text))
				{
					logContains();
				}
				else if (Config.DEBUG_DOMAINS)
				{
					if (sourceNames == null || sourceNames.Length == 0)
					{
						foreach (List<Regex> value3 in debugDomainsRegex.Values)
						{
							foreach (Regex item in value3)
							{
								Match match = item.Match(text);
								if (match.Success)
								{
									Debug.Log(string.Concat("Test string contains a domain: '", match.Value, "' detected by regex '", item, "'"));
									result = true;
									break;
								}
							}
						}
					}
					else
					{
						foreach (string text2 in sourceNames)
						{
							List<Regex> value;
							if (debugDomainsRegex.TryGetValue(text2, out value))
							{
								foreach (Regex item2 in value)
								{
									Match match2 = item2.Match(text);
									if (match2.Success)
									{
										Debug.Log(string.Concat("Test string contains a domain: '", match2.Value, "' detected by regex '", item2, "'' from source '", text2, "'"));
										result = true;
										break;
									}
								}
							}
							else
							{
								logResourceNotFound(text2);
							}
						}
					}
				}
				else if (sourceNames == null || sourceNames.Length == 0)
				{
					foreach (Regex value4 in domainsRegex.Values)
					{
						if (value4.Match(text).Success)
						{
							result = true;
							break;
						}
					}
				}
				else
				{
					foreach (string text3 in sourceNames)
					{
						Regex value2;
						if (domainsRegex.TryGetValue(text3, out value2))
						{
							Match match3 = value2.Match(text);
							if (match3.Success)
							{
								result = true;
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
			else
			{
				logFilterNotReady();
			}
			return result;
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
				else if (Config.DEBUG_DOMAINS)
				{
					if (sourceNames == null || sourceNames.Length == 0)
					{
						foreach (List<Regex> value3 in debugDomainsRegex.Values)
						{
							foreach (Regex item in value3)
							{
								MatchCollection matchCollection = item.Matches(text);
								foreach (Match item2 in matchCollection)
								{
									foreach (Capture capture5 in item2.Captures)
									{
										Debug.Log(string.Concat("Test string contains a domain: '", capture5.Value, "' detected by regex '", item, "'"));
										if (!list.Contains(capture5.Value))
										{
											list.Add(capture5.Value);
										}
									}
								}
							}
						}
					}
					else
					{
						foreach (string text2 in sourceNames)
						{
							List<Regex> value;
							if (debugDomainsRegex.TryGetValue(text2, out value))
							{
								foreach (Regex item3 in value)
								{
									MatchCollection matchCollection2 = item3.Matches(text);
									foreach (Match item4 in matchCollection2)
									{
										foreach (Capture capture6 in item4.Captures)
										{
											Debug.Log(string.Concat("Test string contains a domain: '", capture6.Value, "' detected by regex '", item3, "'' from source '", text2, "'"));
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
								logResourceNotFound(text2);
							}
						}
					}
				}
				else if (sourceNames == null || sourceNames.Length == 0)
				{
					foreach (Regex value4 in domainsRegex.Values)
					{
						MatchCollection matchCollection3 = value4.Matches(text);
						foreach (Match item5 in matchCollection3)
						{
							foreach (Capture capture7 in item5.Captures)
							{
								if (!list.Contains(capture7.Value))
								{
									list.Add(capture7.Value);
								}
							}
						}
					}
				}
				else
				{
					foreach (string text3 in sourceNames)
					{
						Regex value2;
						if (domainsRegex.TryGetValue(text3, out value2))
						{
							MatchCollection matchCollection4 = value2.Matches(text);
							foreach (Match item6 in matchCollection4)
							{
								foreach (Capture capture8 in item6.Captures)
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
							logResourceNotFound(text3);
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
			string text2 = text;
			if (isReady)
			{
				if (string.IsNullOrEmpty(text))
				{
					logReplaceAll();
					text2 = string.Empty;
				}
				else if (Config.DEBUG_DOMAINS)
				{
					if (sourceNames == null || sourceNames.Length == 0)
					{
						foreach (List<Regex> value3 in debugDomainsRegex.Values)
						{
							foreach (Regex item in value3)
							{
								MatchCollection matchCollection = item.Matches(text);
								foreach (Match item2 in matchCollection)
								{
									foreach (Capture capture5 in item2.Captures)
									{
										Debug.Log(string.Concat("Test string contains a domain: '", capture5.Value, "' detected by regex '", item, "'"));
										text2 = text2.Replace(capture5.Value, (!markOnly) ? (prefix + BaseHelper.CreateString(ReplaceCharacters, capture5.Value.Length) + postfix) : (prefix + capture5.Value + postfix));
									}
								}
							}
						}
					}
					else
					{
						foreach (string text3 in sourceNames)
						{
							List<Regex> value;
							if (debugDomainsRegex.TryGetValue(text3, out value))
							{
								foreach (Regex item3 in value)
								{
									MatchCollection matchCollection2 = item3.Matches(text);
									foreach (Match item4 in matchCollection2)
									{
										foreach (Capture capture6 in item4.Captures)
										{
											Debug.Log(string.Concat("Test string contains a domain: '", capture6.Value, "' detected by regex '", item3, "'' from source '", text3, "'"));
											text2 = text2.Replace(capture6.Value, (!markOnly) ? (prefix + BaseHelper.CreateString(ReplaceCharacters, capture6.Value.Length) + postfix) : (prefix + capture6.Value + postfix));
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
					foreach (Regex value4 in domainsRegex.Values)
					{
						MatchCollection matchCollection3 = value4.Matches(text);
						foreach (Match item5 in matchCollection3)
						{
							foreach (Capture capture7 in item5.Captures)
							{
								text2 = text2.Replace(capture7.Value, (!markOnly) ? (prefix + BaseHelper.CreateString(ReplaceCharacters, capture7.Value.Length) + postfix) : (prefix + capture7.Value + postfix));
							}
						}
					}
				}
				else
				{
					foreach (string text4 in sourceNames)
					{
						Regex value2;
						if (domainsRegex.TryGetValue(text4, out value2))
						{
							MatchCollection matchCollection4 = value2.Matches(text);
							foreach (Match item6 in matchCollection4)
							{
								foreach (Capture capture8 in item6.Captures)
								{
									text2 = text2.Replace(capture8.Value, (!markOnly) ? (prefix + BaseHelper.CreateString(ReplaceCharacters, capture8.Value.Length) + postfix) : (prefix + capture8.Value + postfix));
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
			else
			{
				logFilterNotReady();
			}
			return text2;
		}
	}
}
