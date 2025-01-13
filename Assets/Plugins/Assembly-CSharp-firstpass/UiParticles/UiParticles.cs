using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UiParticles
{
	[RequireComponent(typeof(ParticleSystem))]
	public class UiParticles : MaskableGraphic
	{
		[SerializeField]
		[FormerlySerializedAs("m_ParticleSystem")]
		private ParticleSystem m_ParticleSystem;

		[FormerlySerializedAs("m_RenderMode")]
		[SerializeField]
		[Tooltip("Render mode of particles")]
		private UiParticleRenderMode m_RenderMode;

		[FormerlySerializedAs("m_StretchedSpeedScale")]
		[SerializeField]
		[Tooltip("Speed Scale for streched billboards")]
		private float m_StretchedSpeedScale = 1f;

		[FormerlySerializedAs("m_StretchedLenghScale")]
		[SerializeField]
		[Tooltip("Speed Scale for streched billboards")]
		private float m_StretchedLenghScale = 1f;

		[FormerlySerializedAs("m_IgnoreTimescale")]
		[SerializeField]
		[Tooltip("If true, particles ignore timescale")]
		private bool m_IgnoreTimescale;

		private ParticleSystemRenderer m_ParticleSystemRenderer;

		private ParticleSystem.Particle[] m_Particles;

		public ParticleSystem ParticleSystem
		{
			get
			{
				return m_ParticleSystem;
			}
			set
			{
				if (SetPropertyUtility.SetClass(ref m_ParticleSystem, value))
				{
					SetAllDirty();
				}
			}
		}

		public override Texture mainTexture
		{
			get
			{
				if (material != null && material.mainTexture != null)
				{
					return material.mainTexture;
				}
				return Graphic.s_WhiteTexture;
			}
		}

		public UiParticleRenderMode RenderMode
		{
			get
			{
				return m_RenderMode;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_RenderMode, value))
				{
					SetAllDirty();
				}
			}
		}

		protected override void Awake()
		{
			ParticleSystem component = GetComponent<ParticleSystem>();
			ParticleSystemRenderer component2 = GetComponent<ParticleSystemRenderer>();
			if (m_Material == null)
			{
				m_Material = component2.sharedMaterial;
			}
			if (component2.renderMode == ParticleSystemRenderMode.Stretch)
			{
				RenderMode = UiParticleRenderMode.StreachedBillboard;
			}
			base.Awake();
			ParticleSystem = component;
			m_ParticleSystemRenderer = component2;
		}

		public override void SetMaterialDirty()
		{
			base.SetMaterialDirty();
			if (m_ParticleSystemRenderer != null)
			{
				m_ParticleSystemRenderer.sharedMaterial = m_Material;
			}
		}

		protected override void OnPopulateMesh(VertexHelper toFill)
		{
			if (ParticleSystem == null)
			{
				base.OnPopulateMesh(toFill);
			}
			else
			{
				GenerateParticlesBillboards(toFill);
			}
		}

		protected virtual void Update()
		{
			if (!m_IgnoreTimescale)
			{
				if (ParticleSystem != null && ParticleSystem.isPlaying)
				{
					SetVerticesDirty();
				}
			}
			else if (ParticleSystem != null)
			{
				ParticleSystem.Simulate(Time.unscaledDeltaTime, true, false);
				SetVerticesDirty();
			}
			if (m_ParticleSystemRenderer != null && m_ParticleSystemRenderer.enabled)
			{
				m_ParticleSystemRenderer.enabled = false;
			}
		}

		private void InitParticlesBuffer()
		{
			if (m_Particles == null || m_Particles.Length < ParticleSystem.main.maxParticles)
			{
				m_Particles = new ParticleSystem.Particle[ParticleSystem.main.maxParticles];
			}
		}

		private void GenerateParticlesBillboards(VertexHelper vh)
		{
			InitParticlesBuffer();
			int particles = ParticleSystem.GetParticles(m_Particles);
			vh.Clear();
			for (int i = 0; i < particles; i++)
			{
				DrawParticleBillboard(m_Particles[i], vh);
			}
		}

		private void DrawParticleBillboard(ParticleSystem.Particle particle, VertexHelper vh)
		{
			Vector3 vector = particle.position;
			Quaternion rotation = Quaternion.Euler(particle.rotation3D);
			if (ParticleSystem.main.simulationSpace == ParticleSystemSimulationSpace.World)
			{
				vector = base.rectTransform.InverseTransformPoint(vector);
			}
			float num = particle.startLifetime - particle.remainingLifetime;
			float timeAlive = num / particle.startLifetime;
			Vector3 size3D = particle.GetCurrentSize3D(ParticleSystem);
			if (m_RenderMode == UiParticleRenderMode.StreachedBillboard)
			{
				GetStrechedBillboardsSizeAndRotation(particle, timeAlive, ref size3D, out rotation);
			}
			Vector3 vector2 = new Vector3((0f - size3D.x) * 0.5f, size3D.y * 0.5f);
			Vector3 vector3 = new Vector3(size3D.x * 0.5f, size3D.y * 0.5f);
			Vector3 vector4 = new Vector3(size3D.x * 0.5f, (0f - size3D.y) * 0.5f);
			Vector3 vector5 = new Vector3((0f - size3D.x) * 0.5f, (0f - size3D.y) * 0.5f);
			vector2 = rotation * vector2 + vector;
			vector3 = rotation * vector3 + vector;
			vector4 = rotation * vector4 + vector;
			vector5 = rotation * vector5 + vector;
			Color32 currentColor = particle.GetCurrentColor(ParticleSystem);
			int currentVertCount = vh.currentVertCount;
			Vector2[] array = new Vector2[4];
			if (!ParticleSystem.textureSheetAnimation.enabled)
			{
				EvaluateQuadUVs(array);
			}
			else
			{
				EvaluateTexturesheetUVs(particle, num, array);
			}
			vh.AddVert(vector5, currentColor, array[0]);
			vh.AddVert(vector2, currentColor, array[1]);
			vh.AddVert(vector3, currentColor, array[2]);
			vh.AddVert(vector4, currentColor, array[3]);
			vh.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
			vh.AddTriangle(currentVertCount + 2, currentVertCount + 3, currentVertCount);
		}

		private void EvaluateQuadUVs(Vector2[] uvs)
		{
			uvs[0] = new Vector2(0f, 0f);
			uvs[1] = new Vector2(0f, 1f);
			uvs[2] = new Vector2(1f, 1f);
			uvs[3] = new Vector2(1f, 0f);
		}

		private void EvaluateTexturesheetUVs(ParticleSystem.Particle particle, float timeAlive, Vector2[] uvs)
		{
			ParticleSystem.TextureSheetAnimationModule textureSheetAnimation = ParticleSystem.textureSheetAnimation;
			float num = particle.startLifetime / (float)textureSheetAnimation.cycleCount;
			float num2 = timeAlive % num;
			float time = num2 / num;
			int num3 = textureSheetAnimation.numTilesY * textureSheetAnimation.numTilesX;
			float num4 = textureSheetAnimation.frameOverTime.Evaluate(time);
			float num5 = 0f;
			switch (textureSheetAnimation.animation)
			{
			case ParticleSystemAnimationType.WholeSheet:
				num5 = Mathf.Clamp(Mathf.Floor(num4 * (float)num3), 0f, num3 - 1);
				break;
			case ParticleSystemAnimationType.SingleRow:
			{
				num5 = Mathf.Clamp(Mathf.Floor(num4 * (float)textureSheetAnimation.numTilesX), 0f, textureSheetAnimation.numTilesX - 1);
				int num6 = textureSheetAnimation.rowIndex;
				if (textureSheetAnimation.useRandomRow)
				{
					Random.InitState((int)particle.randomSeed);
					num6 = Random.Range(0, textureSheetAnimation.numTilesY);
				}
				num5 += (float)(num6 * textureSheetAnimation.numTilesX);
				break;
			}
			}
			int num7 = (int)num5 % textureSheetAnimation.numTilesX;
			int num8 = (int)num5 / textureSheetAnimation.numTilesX;
			float num9 = 1f / (float)textureSheetAnimation.numTilesX;
			float num10 = 1f / (float)textureSheetAnimation.numTilesY;
			num8 = textureSheetAnimation.numTilesY - 1 - num8;
			float num11 = (float)num7 * num9;
			float num12 = (float)num8 * num10;
			float x = num11 + num9;
			float y = num12 + num10;
			uvs[0] = new Vector2(num11, num12);
			uvs[1] = new Vector2(num11, y);
			uvs[2] = new Vector2(x, y);
			uvs[3] = new Vector2(x, num12);
		}

		private void GetStrechedBillboardsSizeAndRotation(ParticleSystem.Particle particle, float timeAlive01, ref Vector3 size3D, out Quaternion rotation)
		{
			Vector3 zero = Vector3.zero;
			if (ParticleSystem.velocityOverLifetime.enabled)
			{
				zero.x = ParticleSystem.velocityOverLifetime.x.Evaluate(timeAlive01);
				zero.y = ParticleSystem.velocityOverLifetime.y.Evaluate(timeAlive01);
				zero.z = ParticleSystem.velocityOverLifetime.z.Evaluate(timeAlive01);
			}
			Vector3 from = particle.velocity + zero;
			float num = Vector3.Angle(from, Vector3.up);
			int num2 = ((from.x < 0f) ? 1 : (-1));
			rotation = Quaternion.Euler(new Vector3(0f, 0f, num * (float)num2));
			size3D.y *= m_StretchedLenghScale;
			size3D += new Vector3(0f, m_StretchedSpeedScale * from.magnitude);
		}
	}
}
