using _Scripts.Core.ChunkCore.BlockChanging.FixedNotification.Components;
using _Scripts.Core.ChunkCore.ChunkLogic.Components;
using _Scripts.Core.ChunkCore.ChunkLogic.Saving.Components;
using Leopotam.EcsLite;

namespace _Scripts.Core.ChunkCore.ChunkLogic.Saving.Systems
{
	public class MarkChangedChunksNeedToSaveSystem : IEcsPreInitSystem, IEcsRunSystem
	{
		private EcsPool<NeedSaveChunkTag> _needSaveChunkPool;
		private EcsFilter _chunksChangedFilter;

		public void PreInit(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();
			_needSaveChunkPool = world.GetPool<NeedSaveChunkTag>();
			_chunksChangedFilter = world
				.Filter<ChunkComponent>()
				.Inc<FixedBlocksChangedComponent>()
				.Exc<NeedSaveChunkTag>()
				.End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach(var chunkEntity in _chunksChangedFilter)
			{
				_needSaveChunkPool.Add(chunkEntity);
			}
		}
	}
}
