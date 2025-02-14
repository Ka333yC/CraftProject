using System;
using _Scripts.Undone.PhysicsCore.ObjectPhysicsCore.GroundCheck;
using UnityEngine;

namespace _Scripts.Undone.PhysicsCore.ObjectPhysicsCore.Components
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
