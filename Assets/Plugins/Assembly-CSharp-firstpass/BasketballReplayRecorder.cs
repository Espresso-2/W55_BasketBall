using System;
using System.Collections.Generic;
using Moments;
using UnityEngine;

[RequireComponent(typeof(Recorder))]
[AddComponentMenu("")]
public class BasketballReplayRecorder : MonoBehaviour
{
	private Recorder m_Recorder;

	public float m_Progress;

	public string m_LastFile = string.Empty;

	private bool m_IsSaving;

	public bool savingCompleted;

	private void Start()
	{
		m_Recorder = GetComponent<Recorder>();
		m_Recorder.Record();
		m_Recorder.OnPreProcessingDone = OnProcessingDone;
		m_Recorder.OnFileSaveProgress = OnFileSaveProgress;
		m_Recorder.OnFileSaved = OnFileSaved;
	}

	private void OnProcessingDone()
	{
		m_IsSaving = true;
	}

	private void OnFileSaveProgress(int id, float percent)
	{
		m_Progress = percent * 100f;
	}

	private void OnFileSaved(int id, string filepath)
	{
		m_LastFile = filepath;
		m_IsSaving = false;
		savingCompleted = true;
	}

	private void OnDestroy()
	{
	}

	private void Update()
	{
	}

	public void SaveGif()
	{
		int num = m_Recorder.framesSinceGoodLookingEvent;
		if (num > m_Recorder.RenderTextureFrames.Count)
		{
			num = 10;
		}
		try
		{
			SetToThisNumFramesFromEnd(num);
		}
		catch (Exception ex)
		{
			Debug.Log("Error trying to set to specific frame, continue trying to save gif anyway error: " + ex.ToString());
		}
		m_Recorder.Save();
		m_Progress = 0f;
	}

	private void SetToThisNumFramesFromEnd(int framesFromEnd)
	{
		Queue<RenderTexture> renderTextureFrames = m_Recorder.RenderTextureFrames;
		int num = renderTextureFrames.Count - framesFromEnd;
		if (renderTextureFrames.Count > num + 1)
		{
			for (int i = 0; i < num; i++)
			{
				renderTextureFrames.Enqueue(renderTextureFrames.Dequeue());
			}
		}
	}
}
