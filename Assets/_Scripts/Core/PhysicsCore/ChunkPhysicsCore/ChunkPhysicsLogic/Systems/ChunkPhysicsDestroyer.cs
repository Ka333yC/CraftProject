using _Scripts.Core.ChunkCore.ChunkLogic.Components;
using _Scripts.Core.PhysicsCore.ChunkPhysicsCore.ChunkPhysicsLogic.Components;
using Leopotam.EcsLite;

namespace _Scripts.Core.PhysicsCore.ChunkPhysicsCore.ChunkPhysicsLogic.Systems
{
	public class ChunkPhysicsDestroyer : IEcsPreInitSystem, IEcsRunSystem
	{
		private EcsPool<ChunkPhysicsComponent> _chunkPhysicsPool;
		private EcsFilter _chunksPhysicsToDestroyFilter;

		public void PreInit(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();
			_chunkPhysicsPool = world.GetPool<ChunkPhysicsComponent>();
			_chunksPhysicsToDestroyFilter = world
				.Filter<ChunkPhysicsComponent>()
				.Exc<ChunkComponent>()
				.End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach(int chunkPhysicsEntity in _chunksPhysicsToDestroyFilter)
			{
				ref var chunkPhysics = ref _chunkPhysicsPool.Get(chunkPhysicsEntity);
				chunkPhysics.BlocksPhysicsGetter.Dispose();
				chunkPhysics.MeshPartsContainer.Dispose();
				_chunkPhysicsPool.Del(chunkPhysicsEntity);
			}
		}
	}
}
