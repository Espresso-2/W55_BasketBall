using UnityEngine;

namespace Crosstales.Common.Util
{
	public class SpectrumVisualizer : MonoBehaviour
	{
		[Tooltip("FFT-analyzer with the spectrum data.")]
		public FFTAnalyzer Analyzer;

		[Tooltip("Prefab for the frequency representation.")]
		public GameObject VisualPrefab;

		[Tooltip("Width per prefab.")]
		public float Width = 0.075f;

		[Tooltip("Gain-power for the frequency.")]
		public float Gain = 70f;

		[Tooltip("Frequency band from left-to-right (default: true).")]
		public bool LeftToRight = true;

		[Tooltip("Opacity of the material of the prefab (default: 1).")]
		[Range(0f, 1f)]
		public float Opacity = 1f;

		private Transform tf;

		private Transform[] visualTransforms;

		private Vector3 visualPos = Vector3.zero;

		private int samplesPerChannel;

		public void Start()
		{
			tf = base.transform;
			samplesPerChannel = Analyzer.Samples.Length / 2;
			visualTransforms = new Transform[samplesPerChannel];
			for (int i = 0; i < samplesPerChannel; i++)
			{
				GameObject gameObject = ((!LeftToRight) ? Object.Instantiate(VisualPrefab, new Vector3(tf.position.x - (float)i * Width, tf.position.y, tf.position.z), Quaternion.identity) : Object.Instantiate(VisualPrefab, new Vector3(tf.position.x + (float)i * Width, tf.position.y, tf.position.z), Quaternion.identity));
				gameObject.GetComponent<Renderer>().material.color = BaseHelper.HSVToRGB(360f / (float)samplesPerChannel * (float)i, 1f, 1f, Opacity);
				visualTransforms[i] = gameObject.GetComponent<Transform>();
				visualTransforms[i].parent = tf;
			}
		}

		public void Update()
		{
			for (int i = 0; i < visualTransforms.Length; i++)
			{
				visualPos.Set(Width, Analyzer.Samples[i] * Gain, Width);
				visualTransforms[i].localScale = visualPos;
			}
		}
	}
}
