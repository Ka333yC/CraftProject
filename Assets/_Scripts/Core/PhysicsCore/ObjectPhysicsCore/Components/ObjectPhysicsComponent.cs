using Assets.Scripts.Implementation.ObjectPhysics.GroundCheck;
using System;
using UnityEngine;

namespace PhysicsCore.ObjectPhysics.Components
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
