using UnityEngine;

namespace _Scripts.Core.PhysicsCore.Presets
{
	[CreateAssetMenu(fileName = "PhysicsPresets", menuName = "Global settings/Physics presets")]
	public class PhysicsPresets : ScriptableObject
	{
		[field: SerializeField, Tooltip("Показатель незначительного движения, " +
			"при котором движением объекта можно пренебречь.")]
		public float SlightlyVelocityMagnitude { get; private set; } = 0.003f;

		[field: SerializeField]
		public float GroundCheckDistance { get; private set; } = 0.05f;

		[field: SerializeField]
		public LayerMask IgnoreAllLayer { get; private set; }

		[field: SerializeField]
		public LayerMask GroundLayer { get; private set; }

		[field: SerializeField]
		public MeshColliderCookingOptions CookingOptions { get; private set; }
	}
}
