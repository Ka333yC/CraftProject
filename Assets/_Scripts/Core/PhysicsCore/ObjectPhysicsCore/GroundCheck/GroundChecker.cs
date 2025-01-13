using _Scripts.Apart.Extensions;
using _Scripts.TempScripts;
using UnityEngine;

namespace _Scripts.Core.PhysicsCore.ObjectPhysicsCore.GroundCheck
{
	public class GroundChecker
	{
		private static readonly Collider[] _overlapedColliders = new Collider[128];

		private readonly Rigidbody _rigidbody;
		private readonly Collider _collider;
		private readonly Vector3 _boxCastHalfSize; // Считаем без значения y

		public GroundChecker(Rigidbody rigidbody, Collider collider)
		{
			_rigidbody = rigidbody;
			_collider = collider;
			var boundsSize = collider.bounds.size;
			// Домножаю на 2 на всякий случай, т.к. OverlapBox может захватить площадь сбоку, а не снизу
			Vector3 boxCastSize = new Vector3()
			{
				x = boundsSize.x - Physics.defaultContactOffset * 2,
				y = Singleton.Instance.PhysicsSettings.GroundCheckDistance,
				z = boundsSize.z - Physics.defaultContactOffset * 2,
			};

			_boxCastHalfSize = boxCastSize / 2;
		}

		public bool IsGrounded() 
		{
			if(Mathf.Abs(_rigidbody.velocity.y) > Singleton.Instance.PhysicsSettings.SlightlyVelocityMagnitude / 3)
			{
				return false;
			}

			var bounds = _collider.bounds;
			var boxCastCenter = bounds.center;
			boxCastCenter.y -= bounds.size.y / 2;
			var overlapedCollidersCount = Physics.OverlapBoxNonAlloc(boxCastCenter,
				_boxCastHalfSize, _overlapedColliders);
			var groundLayer = Singleton.Instance.PhysicsSettings.GroundLayer;
			for(int i = 0; i < overlapedCollidersCount; i++)
			{
				if(groundLayer.Has(_overlapedColliders[i].gameObject.layer))
				{
					return true;
				}
			}

			return false;
		}
	}
}
