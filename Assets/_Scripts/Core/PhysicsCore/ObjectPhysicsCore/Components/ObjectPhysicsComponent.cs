using System;
using _Scripts.Core.PhysicsCore.ObjectPhysicsCore.GroundCheck;
using UnityEngine;

namespace _Scripts.Core.PhysicsCore.ObjectPhysicsCore.Components
{
	[Serializable]
	public struct ObjectPhysicsComponent
	{
		public Rigidbody Rigidbody;
		public Collider FullSizeCollider;
		public float HorizontalDrag;
		public float VerticalDrag;
		public GroundChecker GroundChecker;
	}
}
