using _Scripts.Core.ChunkCore.ChunkLogic.Components;
using _Scripts.Core.PhysicsCore.ChunkPhysicsCore.ChunkPhysicsLogic.Components;
using _Scripts.Core.PhysicsCore.ChunkPhysicsCore.ChunkPhysicsLogic.MeshGeneration.Components;
using _Scripts.Core.PhysicsCore.ChunkPhysicsCore.ChunkPhysicsLogic.MeshPartsContainerLogic.Components;
using _Scripts.Core.PhysicsCore.ObjectPhysicsCore.InSimulatedChunkCheck.Components;
using Leopotam.EcsLite;

namespace _Scripts.Core.PhysicsCore.ObjectPhysicsCore.InSimulatedChunkCheck.Systems
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
