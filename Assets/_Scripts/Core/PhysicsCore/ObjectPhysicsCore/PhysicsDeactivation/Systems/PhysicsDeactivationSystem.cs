using _Scripts.Core.PhysicsCore.ObjectPhysicsCore.Components;
using _Scripts.Core.PhysicsCore.ObjectPhysicsCore.InBlockCheck.Components;
using _Scripts.Core.PhysicsCore.ObjectPhysicsCore.InSimulatedChunkCheck.Components;
using Leopotam.EcsLite;

namespace _Scripts.Core.PhysicsCore.ObjectPhysicsCore.PhysicsDeactivation.Systems
{
	public class PhysicsDeactivationSystem : IEcsPreInitSystem, IEcsRunSystem
	{
		private EcsPool<ObjectPhysicsComponent> _objectPhysicsPool;
		private EcsPool<ObjectPhysicsInBlockTag> _chunkPhysicsBecomeSimulatedPool;
		private EcsFilter _objectPhysicsSimulatedFilter;
		private EcsFilter _objectPhysicsInBlockFilter;
		private EcsFilter _objectPhysicsInNotSimulatedChunkFilter;

		public void PreInit(IEcsSystems systems)
		{
			var world = systems.GetWorld();
			_objectPhysicsPool = world.GetPool<ObjectPhysicsComponent>();
			_objectPhysicsSimulatedFilter = world
				.Filter<ObjectPhysicsComponent>()
				.Inc<InSimulatedChunkPhysicsTag>()
				.Exc<ObjectPhysicsInBlockTag>()
				.End();
			_objectPhysicsInBlockFilter = world
				.Filter<ObjectPhysicsComponent>()
				.Inc<ObjectPhysicsInBlockTag>()
				.End();
			_objectPhysicsInNotSimulatedChunkFilter = world
				.Filter<ObjectPhysicsComponent>()
				.Exc<InSimulatedChunkPhysicsTag>()
				.End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach(var objectPhysicsEntity in _objectPhysicsSimulatedFilter)
			{
				_objectPhysicsPool.Get(objectPhysicsEntity).Rigidbody.isKinematic = false;
			}

			foreach(var objectPhysicsEntity in _objectPhysicsInBlockFilter)
			{
				_objectPhysicsPool.Get(objectPhysicsEntity).Rigidbody.isKinematic = true;
			}

			foreach(var objectPhysicsEntity in _objectPhysicsInNotSimulatedChunkFilter)
			{
				_objectPhysicsPool.Get(objectPhysicsEntity).Rigidbody.isKinematic = true;
			}
		}
	}
}
