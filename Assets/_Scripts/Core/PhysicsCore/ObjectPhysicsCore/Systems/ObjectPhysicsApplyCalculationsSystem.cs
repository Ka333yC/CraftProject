using _Scripts.Core.PhysicsCore.ObjectPhysicsCore.Components;
using _Scripts.Core.PhysicsCore.Presets;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace _Scripts.Core.PhysicsCore.ObjectPhysicsCore.Systems
{
	public class ObjectPhysicsApplyCalculationsSystem : IEcsPreInitSystem, IEcsRunSystem
	{
		[Inject]
		private PhysicsPresets _physicsPresets;

		private EcsPool<ObjectPhysicsComponent> _objectPhysicsPool;
		private EcsFilter _objectPhysicsFilter;

		public void PreInit(IEcsSystems systems)
		{
			var world = systems.GetWorld();
			_objectPhysicsPool = world.GetPool<ObjectPhysicsComponent>();
			_objectPhysicsFilter = world
				.Filter<ObjectPhysicsComponent>()
				.End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach(var objectPhysicsEntity in _objectPhysicsFilter)
			{
				ref var objectPhysics = ref _objectPhysicsPool.Get(objectPhysicsEntity);
				var rigidbody = objectPhysics.Rigidbody;
				if(rigidbody.IsSleeping() ||
					rigidbody.velocity.magnitude < _physicsPresets.SlightlyVelocityMagnitude) 
				{
					continue;
				}

				ApplyDrag(ref objectPhysics);
			}
		}

		private void ApplyDrag(ref ObjectPhysicsComponent objectPhysics)
		{
			var rigidbody = objectPhysics.Rigidbody;
			var velocity = rigidbody.velocity;
			var xDrag = velocity.x * objectPhysics.HorizontalDrag * Time.fixedDeltaTime;
			var yDrag = velocity.y * objectPhysics.VerticalDrag * Time.fixedDeltaTime;
			var zDrag = velocity.z * objectPhysics.HorizontalDrag * Time.fixedDeltaTime;
			Vector3 velocityDrag = new Vector3(xDrag, yDrag, zDrag);
			rigidbody.velocity -= velocityDrag;
		}
	}
}
