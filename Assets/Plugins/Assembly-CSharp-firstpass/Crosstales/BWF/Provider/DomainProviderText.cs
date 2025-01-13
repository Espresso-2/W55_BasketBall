using System;
using System.Collections;
using System.Collections.Generic;
using Crosstales.BWF.Data;
using Crosstales.BWF.Model;
using Crosstales.Common.Util;
using UnityEngine;
using UnityEngine.Networking;

namespace Crosstales.BWF.Provider
{
	[HelpURL("https://www.crosstales.com/media/data/assets/badwordfilter/api/class_crosstales_1_1_b_w_f_1_1_provider_1_1_domain_provider_text.html")]
	public class DomainProviderText : DomainProvider
	{
		public override void Load()
		{
			base.Load();
			if (Sources == null)
			{
				return;
			}
			loading = true;
			if (BaseHelper.isEditorMode)
			{
				return;
			}
			Crosstales.BWF.Data.Source[] sources = Sources;
			foreach (Crosstales.BWF.Data.Source source in sources)
			{
				if (source != null)
				{
					if (source.Resource != null)
					{
						StartCoroutine(loadResource(source));
					}
					if (!string.IsNullOrEmpty(source.URL))
					{
						StartCoroutine(loadWeb(source));
					}
				}
			}
		}

		public override void Save()
		{
			Debug.LogWarning("Save not implemented!");
		}

		private IEnumerator loadWeb(Crosstales.BWF.Data.Source src)
		{
			string uid = Guid.NewGuid().ToString();
			coRoutines.Add(uid);
			if (!string.IsNullOrEmpty(src.URL))
			{
				using (UnityWebRequest www = UnityWebRequest.Get(src.URL.Trim()))
				{
					www.timeout = 5;
					www.downloadHandler = new DownloadHandlerBuffer();
					yield return www.SendWebRequest();
					if (!www.isHttpError && !www.isNetworkError)
					{
						List<string> list = BaseHelper.SplitStringToLines(www.downloadHandler.text);
						yield return null;
						if (list.Count > 0)
						{
							domains.Add(new Domains(src, list));
						}
						else
						{
							Debug.LogWarning("Source: '" + src.URL + "' does not contain any active bad words!");
						}
					}
					else
					{
						Debug.LogWarning("Could not load source: '" + src.URL + "'" + Environment.NewLine + www.error + Environment.NewLine + "Did you set the correct 'URL'?");
					}
				}
			}
			else
			{
				Debug.LogWarning("'URL' is null or empty!" + Environment.NewLine + "Please add a valid URL.");
			}
			coRoutines.Remove(uid);
			if (loading && coRoutines.Count == 0)
			{
				loading = false;
				init();
			}
		}

		private IEnumerator loadResource(Crosstales.BWF.Data.Source src)
		{
			string uid = Guid.NewGuid().ToString();
			coRoutines.Add(uid);
			if (src.Resource != null)
			{
				List<string> list = BaseHelper.SplitStringToLines(src.Resource.text);
				yield return null;
				if (list.Count > 0)
				{
					domains.Add(new Domains(src, list));
				}
				else
				{
					Debug.LogWarning(string.Concat("Resource: '", src.Resource, "' does not contain any active bad words!"));
				}
			}
			else
			{
				Debug.LogWarning("Resource field 'Source' is null or empty!" + Environment.NewLine + "Please add a valid resource.");
			}
			coRoutines.Remove(uid);
			if (loading && coRoutines.Count == 0)
			{
				loading = false;
				init();
			}
		}
	}
}
