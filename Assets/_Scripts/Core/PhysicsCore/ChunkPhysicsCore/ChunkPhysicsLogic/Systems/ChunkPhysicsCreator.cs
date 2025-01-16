using _Scripts.Core.ChunkCore.ChunkLogic.Components;
using _Scripts.Core.PhysicsCore.ChunkPhysicsCore.ChunkPhysicsLogic.Components;
using _Scripts.Core.PhysicsCore.ChunkPhysicsCore.ChunkPhysicsLogic.Components.Elements;
using _Scripts.Core.PhysicsCore.ChunkPhysicsCore.ChunkPhysicsLogic.Components.Elements.BlockPhysicsGetters;
using Leopotam.EcsLite;

namespace _Scripts.Core.PhysicsCore.ChunkPhysicsCore.ChunkPhysicsLogic.Systems
{
	public class ChunkPhysicsCreator : IEcsPreInitSystem, IEcsRunSystem
	{
		private EcsPool<ChunkPhysicsComponent> _chunkPhysicsPool;
		private EcsFilter _chunksWithoutChunkPhysicsFilter;

		public void PreInit(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();
			_chunkPhysicsPool = world.GetPool<ChunkPhysicsComponent>();
			_chunksWithoutChunkPhysicsFilter = world
				.Filter<ChunkComponent>()
				.Inc<ChunkInitializedTag>()
				.Exc<ChunkPhysicsComponent>()
				.End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach(var chunkWithoutChunkPhysicsEntity in _chunksWithoutChunkPhysicsFilter)
			{
				CreateChunkPhysics(chunkWithoutChunkPhysicsEntity, systems.GetWorld());
			}
		}

		private void CreateChunkPhysics(int chunkEntity, EcsWorld world)
		{
			ref var chunkPhysics = ref _chunkPhysicsPool.Add(chunkEntity);
			chunkPhysics.BlocksPhysicsGetter = new BlocksPhysicsGetter(chunkEntity, world);
			chunkPhysics.MeshPartsContainer = new ColliderMeshPartsContainer(chunkPhysics.BlocksPhysicsGetter);
		}
	}
}
