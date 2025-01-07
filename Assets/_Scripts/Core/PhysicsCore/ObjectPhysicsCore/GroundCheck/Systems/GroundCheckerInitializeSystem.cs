using _Scripts.Core.PhysicsCore.ObjectPhysicsCore.Components;
using _Scripts.Core.PhysicsCore.ObjectPhysicsCore.GroundCheck.Components;
using Leopotam.EcsLite;

namespace _Scripts.Core.PhysicsCore.ObjectPhysicsCore.GroundCheck.Systems
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
