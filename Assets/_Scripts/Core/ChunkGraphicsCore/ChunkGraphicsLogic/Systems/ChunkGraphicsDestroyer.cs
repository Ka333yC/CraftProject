using _Scripts.Core.ChunkCore.ChunkLogic.Components;
using _Scripts.Core.ChunkGraphicsCore.ChunkGraphicsLogic.Components;
using Leopotam.EcsLite;

namespace _Scripts.Core.ChunkGraphicsCore.ChunkGraphicsLogic.Systems
{
	public class ChunkGraphicsDestroyer : IEcsPreInitSystem, IEcsRunSystem
	{
		private EcsPool<ChunkGraphicsComponent> _chunkGraphicsPool;
		private EcsFilter _chunkGraphicsToDestroyFilter;

		public void PreInit(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();
			_chunkGraphicsPool = world.GetPool<ChunkGraphicsComponent>();
			_chunkGraphicsToDestroyFilter = world
				.Filter<ChunkGraphicsComponent>()
				.Exc<ChunkComponent>()
				.End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach(int chunkGraphicsEntity in _chunkGraphicsToDestroyFilter)
			{
				DestroyChunkGraphics(chunkGraphicsEntity);
			}
		}

		private void DestroyChunkGraphics(int chunkGraphicsEntity) 
		{
			ref var chunkGraphics = ref _chunkGraphicsPool.Get(chunkGraphicsEntity);
			chunkGraphics.MeshPartsContainer.Dispose();
			_chunkGraphicsPool.Del(chunkGraphicsEntity);
		}
	}
}
