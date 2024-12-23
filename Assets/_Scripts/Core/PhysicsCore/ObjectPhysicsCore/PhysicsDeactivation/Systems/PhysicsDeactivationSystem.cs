using Assets.Scripts.Core.ChunkCore.LifeTimeControl.Components;
using Assets.Scripts.Core.ObjectPhysicsCore.InBlockCheck.Components;
using Assets.Scripts.Core.ObjectPhysicsCore.InSimulatedChunkCheck.Components;
using Assets.Scripts.Core.PhysicsCore.ChunkPhysicsCore.MeshUpdating.Components;
using Assets.Scripts.Core.PhysicsCore.ObjectPhysics.PhysicsDeactivating;
using Assets.Scripts.Core.PhysicsCore.ObjectPhysics.PhysicsDeactivating.Components;
using Leopotam.EcsLite;
using PhysicsCore.ChunkPhysicsCore.LifeTimeControl.Components;
using PhysicsCore.ObjectPhysics.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Core.ObjectPhysicsCore.PhysicsDeactivation.Systems
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
