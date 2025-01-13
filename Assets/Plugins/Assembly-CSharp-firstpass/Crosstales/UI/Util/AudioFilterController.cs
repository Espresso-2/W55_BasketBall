using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Crosstales.UI.Util
{
	public class AudioFilterController : MonoBehaviour
	{
		[Header("Audio Filters")]
		[Tooltip("Searches for all audio filters in the whole scene (default: true).")]
		public bool FindAllAudioFiltersOnStart = true;

		public AudioReverbFilter[] ReverbFilters;

		public AudioChorusFilter[] ChorusFilters;

		public AudioEchoFilter[] EchoFilters;

		public AudioDistortionFilter[] DistortionFilters;

		public AudioLowPassFilter[] LowPassFilters;

		public AudioHighPassFilter[] HighPassFilters;

		[Header("Settings")]
		[Tooltip("Resets all active audio filters (default: on).")]
		public bool ResetAudioFiltersOnStart = true;

		public bool ChorusFilter;

		public bool EchoFilter;

		public bool DistortionFilter;

		public float DistortionFilterValue = 0.5f;

		public bool LowpassFilter;

		public float LowpassFilterValue = 5000f;

		public bool HighpassFilter;

		public float HighpassFilterValue = 5000f;

		[Header("UI Objects")]
		public Dropdown ReverbFilterDropdown;

		public Text DistortionText;

		public Text LowpassText;

		public Text HighpassText;

		private List<AudioReverbPreset> reverbPresets = new List<AudioReverbPreset>();

		private bool initalized;

		public void Start()
		{
			List<Dropdown.OptionData> list = new List<Dropdown.OptionData>();
			foreach (AudioReverbPreset value in Enum.GetValues(typeof(AudioReverbPreset)))
			{
				list.Add(new Dropdown.OptionData(value.ToString()));
				reverbPresets.Add(value);
			}
			if (ReverbFilterDropdown != null)
			{
				ReverbFilterDropdown.ClearOptions();
				ReverbFilterDropdown.AddOptions(list);
			}
		}

		public void Update()
		{
			if (!initalized && Time.frameCount % 30 == 0)
			{
				initalized = true;
				if (FindAllAudioFiltersOnStart)
				{
					FindAllAudioFilters();
				}
				if (ResetAudioFiltersOnStart)
				{
					ResetAudioFilters();
				}
			}
		}

		public void FindAllAudioFilters()
		{
			ReverbFilters = UnityEngine.Object.FindObjectsOfType(typeof(AudioReverbFilter)) as AudioReverbFilter[];
			ChorusFilters = UnityEngine.Object.FindObjectsOfType(typeof(AudioChorusFilter)) as AudioChorusFilter[];
			EchoFilters = UnityEngine.Object.FindObjectsOfType(typeof(AudioEchoFilter)) as AudioEchoFilter[];
			DistortionFilters = UnityEngine.Object.FindObjectsOfType(typeof(AudioDistortionFilter)) as AudioDistortionFilter[];
			LowPassFilters = UnityEngine.Object.FindObjectsOfType(typeof(AudioLowPassFilter)) as AudioLowPassFilter[];
			HighPassFilters = UnityEngine.Object.FindObjectsOfType(typeof(AudioHighPassFilter)) as AudioHighPassFilter[];
		}

		public void ResetAudioFilters()
		{
			ReverbFilterDropdownChanged(0);
			ChorusFilterEnabled(ChorusFilter);
			EchoFilterEnabled(EchoFilter);
			DistortionFilterEnabled(DistortionFilter);
			DistortionFilterChanged(DistortionFilterValue);
			LowPassFilterEnabled(LowpassFilter);
			LowPassFilterChanged(LowpassFilterValue);
			HighPassFilterEnabled(HighpassFilter);
			HighPassFilterChanged(HighpassFilterValue);
		}

		public void ReverbFilterDropdownChanged(int index)
		{
			AudioReverbFilter[] reverbFilters = ReverbFilters;
			foreach (AudioReverbFilter audioReverbFilter in reverbFilters)
			{
				audioReverbFilter.reverbPreset = reverbPresets[index];
			}
		}

		public void ChorusFilterEnabled(bool enabled)
		{
			AudioChorusFilter[] chorusFilters = ChorusFilters;
			foreach (AudioChorusFilter audioChorusFilter in chorusFilters)
			{
				audioChorusFilter.enabled = enabled;
			}
		}

		public void EchoFilterEnabled(bool enabled)
		{
			AudioEchoFilter[] echoFilters = EchoFilters;
			foreach (AudioEchoFilter audioEchoFilter in echoFilters)
			{
				audioEchoFilter.enabled = enabled;
			}
		}

		public void DistortionFilterEnabled(bool enabled)
		{
			AudioDistortionFilter[] distortionFilters = DistortionFilters;
			foreach (AudioDistortionFilter audioDistortionFilter in distortionFilters)
			{
				audioDistortionFilter.enabled = enabled;
			}
		}

		public void DistortionFilterChanged(float value)
		{
			AudioDistortionFilter[] distortionFilters = DistortionFilters;
			foreach (AudioDistortionFilter audioDistortionFilter in distortionFilters)
			{
				audioDistortionFilter.distortionLevel = value;
			}
			if (DistortionText != null)
			{
				DistortionText.text = value.ToString("0.00");
			}
		}

		public void LowPassFilterEnabled(bool enabled)
		{
			AudioLowPassFilter[] lowPassFilters = LowPassFilters;
			foreach (AudioLowPassFilter audioLowPassFilter in lowPassFilters)
			{
				audioLowPassFilter.enabled = enabled;
			}
		}

		public void LowPassFilterChanged(float value)
		{
			AudioLowPassFilter[] lowPassFilters = LowPassFilters;
			foreach (AudioLowPassFilter audioLowPassFilter in lowPassFilters)
			{
				audioLowPassFilter.cutoffFrequency = value;
			}
			if (LowpassText != null)
			{
				LowpassText.text = value.ToString("0");
			}
		}

		public void HighPassFilterEnabled(bool enabled)
		{
			AudioHighPassFilter[] highPassFilters = HighPassFilters;
			foreach (AudioHighPassFilter audioHighPassFilter in highPassFilters)
			{
				audioHighPassFilter.enabled = enabled;
			}
		}

		public void HighPassFilterChanged(float value)
		{
			AudioHighPassFilter[] highPassFilters = HighPassFilters;
			foreach (AudioHighPassFilter audioHighPassFilter in highPassFilters)
			{
				audioHighPassFilter.cutoffFrequency = value;
			}
			if (HighpassText != null)
			{
				HighpassText.text = value.ToString("0");
			}
		}
	}
}
