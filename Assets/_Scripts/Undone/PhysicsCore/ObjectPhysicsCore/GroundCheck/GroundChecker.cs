using _Scripts.Apart.Extensions;
using _Scripts.Core.PhysicsCore.Presets;
using UnityEngine;

namespace _Scripts.Undone.PhysicsCore.ObjectPhysicsCore.GroundCheck
{
	public class GroundChecker
	{
		private static readonly Collider[] _overlappedColliders = new Collider[128];

		private readonly Rigidbody _rigidbody;
		private readonly Collider _collider;
		private readonly PhysicsPresets _physicsPresets;
		private readonly Vector3 _boxCastHalfSize; // Считаем без значения y

		public GroundChecker(Rigidbody rigidbody, Collider collider, PhysicsPresets physicsPresets)
		{
			_rigidbody = rigidbody;
			_collider = collider;
			_physicsPresets = physicsPresets;
			var boundsSize = collider.bounds.size;
			// Домножаю на 2 на всякий случай, т.к. OverlapBox может захватить площадь сбоку, а не снизу
			Vector3 boxCastSize = new Vector3()
			{
				x = boundsSize.x - Physics.defaultContactOffset * 2,
				y = _physicsPresets.GroundCheckDistance,
				z = boundsSize.z - Physics.defaultContactOffset * 2,
			};

			_boxCastHalfSize = boxCastSize / 2;
		}

		public bool IsGrounded() 
		{
			if(Mathf.Abs(_rigidbody.velocity.y) > _physicsPresets.GroundCheckAllowedVelocity)
			{
				return false;
			}

			var bounds = _collider.bounds;
			var boxCastCenter = bounds.center;
			boxCastCenter.y -= bounds.size.y / 2;
			var overlappedCollidersCount = Physics.OverlapBoxNonAlloc(boxCastCenter,
				_boxCastHalfSize, _overlappedColliders);
			var groundLayer = _physicsPresets.GroundLayer;
			for(int i = 0; i < overlappedCollidersCount; i++)
			{
				if(groundLayer.Has(_overlappedColliders[i].gameObject.layer))
				{
					return true;
				}
			}

			return false;
		}
	}
}
