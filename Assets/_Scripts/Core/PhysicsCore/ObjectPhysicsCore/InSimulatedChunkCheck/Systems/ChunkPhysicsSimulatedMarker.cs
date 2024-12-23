using System;
using Assets.Scripts.Core.ChunkCore.LifeTimeControl.Components;
using Assets.Scripts.Core.ObjectPhysicsCore.InSimulatedChunkCheck.Components;
using Assets.Scripts.Core.PhysicsCore.ChunkPhysicsCore.MeshPartsContainerUpdating.Components;
using Assets.Scripts.Core.PhysicsCore.ChunkPhysicsCore.MeshUpdating.Components;
using Assets.Scripts.Core.PhysicsCore.ObjectPhysics.PhysicsDeactivating.Components;
using ChunkCore.ChunksContainerScripts.Components;
using Leopotam.EcsLite;
using PhysicsCore.ChunkPhysicsCore.LifeTimeControl.Components;
using PhysicsCore.ObjectPhysics.Components;
using PhysicsCore.ObjectPhysics.PositionUpdater.Components;

namespace Assets.Scripts.Core.PhysicsCore.ObjectPhysics.PhysicsDeactivating.Systems
{
	public class ChunkPhysicsSimulatedMarker : IEcsPreInitSystem, IEcsRunSystem
	{
		private EcsPool<ChunkPhysicsSimulatedTag> _chunkPhysicsSimulatedPool;
		private EcsPool<ChunkPhysicsBecomeSimulatedTag> _chunkPhysicsBecomeSimulatedPool;
		private EcsFilter _generatedMeshChunkPhysicsFilter;

		public void PreInit(IEcsSystems systems)
		{
			var world = systems.GetWorld();
			_chunkPhysicsSimulatedPool = world.GetPool<ChunkPhysicsSimulatedTag>();
			_chunkPhysicsBecomeSimulatedPool = world.GetPool<ChunkPhysicsBecomeSimulatedTag>();
			_generatedMeshChunkPhysicsFilter = world
				.Filter<ChunkPhysicsMeshGeneratedNotificationTag>()
				.Inc<ChunkComponent>()
				.Inc<ChunkPhysicsComponent>()
				.Inc<MeshPartsContainerInitializedTag>()
				.Exc<ChunkPhysicsSimulatedTag>()
				.End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach(var entity in _generatedMeshChunkPhysicsFilter)
			{
				_chunkPhysicsSimulatedPool.Add(entity);
				_chunkPhysicsBecomeSimulatedPool.Add(entity);
			}
		}
	}
}
