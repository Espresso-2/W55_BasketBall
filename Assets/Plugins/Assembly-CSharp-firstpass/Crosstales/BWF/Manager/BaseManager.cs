using UnityEngine;

namespace Crosstales.BWF.Manager
{
	[ExecuteInEditMode]
	public abstract class BaseManager : MonoBehaviour
	{
		[Header("Behaviour Settings")]
		[Tooltip("Don't destroy gameobject during scene switches (default: true).")]
		public bool DontDestroy = true;
	}
}
