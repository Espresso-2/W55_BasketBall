using UnityEngine;
using UnityEngine.UI;

namespace Crosstales.UI.Util
{
	public class AudioSourceController : MonoBehaviour
	{
		[Header("Audio Sources")]
		[Tooltip("Searches for all AudioSource in the whole scene (default: true).")]
		public bool FindAllAudioSourcesOnStart = true;

		[Tooltip("Active controlled AudioSources.")]
		public AudioSource[] AudioSources;

		[Header("Settings")]
		[Tooltip("Resets all active AudioSources (default: true).")]
		public bool ResetAudioSourcesOnStart = true;

		[Tooltip("Mute on/off (default: false).")]
		public bool Mute;

		[Tooltip("Loop on/off (default: false).")]
		public bool Loop;

		[Tooltip("Volume of the audio (default: 1)")]
		public float Volume = 1f;

		[Tooltip("Pitch of the audio (default: 1).")]
		public float Pitch = 1f;

		[Tooltip("Stereo pan of the audio (default: 0).")]
		public float StereoPan;

		[Header("UI Objects")]
		public Text VolumeText;

		public Text PitchText;

		public Text StereoPanText;

		private bool initalized;

		public void Update()
		{
			if (!initalized && Time.frameCount % 30 == 0)
			{
				initalized = true;
				if (FindAllAudioSourcesOnStart)
				{
					FindAllAudioSources();
				}
				if (ResetAudioSourcesOnStart)
				{
					ResetAllAudioSources();
				}
			}
		}

		public void FindAllAudioSources()
		{
			AudioSources = Object.FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
		}

		public void ResetAllAudioSources()
		{
			MuteEnabled(Mute);
			LoopEnabled(Loop);
			VolumeChanged(Volume);
			PitchChanged(Pitch);
			StereoPanChanged(0f);
		}

		public void MuteEnabled(bool enabled)
		{
			AudioSource[] audioSources = AudioSources;
			foreach (AudioSource audioSource in audioSources)
			{
				audioSource.mute = enabled;
			}
		}

		public void LoopEnabled(bool enabled)
		{
			AudioSource[] audioSources = AudioSources;
			foreach (AudioSource audioSource in audioSources)
			{
				audioSource.mute = enabled;
			}
		}

		public void VolumeChanged(float value)
		{
			AudioSource[] audioSources = AudioSources;
			foreach (AudioSource audioSource in audioSources)
			{
				audioSource.volume = value;
			}
			if (VolumeText != null)
			{
				VolumeText.text = value.ToString("0.00");
			}
		}

		public void PitchChanged(float value)
		{
			AudioSource[] audioSources = AudioSources;
			foreach (AudioSource audioSource in audioSources)
			{
				audioSource.pitch = value;
			}
			if (PitchText != null)
			{
				PitchText.text = value.ToString("0.00");
			}
		}

		public void StereoPanChanged(float value)
		{
			AudioSource[] audioSources = AudioSources;
			foreach (AudioSource audioSource in audioSources)
			{
				audioSource.panStereo = value;
			}
			if (StereoPanText != null)
			{
				StereoPanText.text = value.ToString("0.00");
			}
		}
	}
}
