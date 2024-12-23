using Leopotam.EcsLite;
using PhysicsCore.ObjectPhysics.Components;
using PhysicsCore.ObjectPhysics.GroundChecking.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Implementation.ObjectPhysics.GroundCheck.Systems
{
	public class GroundCheckerInitializeSystem : IEcsPreInitSystem, IEcsRunSystem
	{
		private EcsPool<GroundCheckerInitializedTag> _groundCheckerInitializedPool;
		private EcsPool<ObjectPhysicsComponent> _objectPhysicsPool;
		private EcsFilter _objectPhysicsToInitializeGroundCheckerFilter;

		public void PreInit(IEcsSystems systems)
		{
			var world = systems.GetWorld();
			_groundCheckerInitializedPool = world.GetPool<GroundCheckerInitializedTag>();
			_objectPhysicsPool = world.GetPool<ObjectPhysicsComponent>();
			_objectPhysicsToInitializeGroundCheckerFilter = world
				.Filter<ObjectPhysicsComponent>()
				.Exc<GroundCheckerInitializedTag>()
				.End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach(var objectPhysicsEntity in _objectPhysicsToInitializeGroundCheckerFilter)
			{
				ref var objectPhysics = ref _objectPhysicsPool.Get(objectPhysicsEntity);
				objectPhysics.GroundChecker = new GroundChecker(objectPhysics.Rigidbody,
					objectPhysics.FullSizeCollider);
				_groundCheckerInitializedPool.Add(objectPhysicsEntity);
			}
		}
	}
}
