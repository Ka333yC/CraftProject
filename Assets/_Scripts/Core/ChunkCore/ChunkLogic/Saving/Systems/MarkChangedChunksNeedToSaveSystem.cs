using Assets.Scripts.Core.ChunkCore.LifeTimeControl.Components;
using Leopotam.EcsLite;
using Assets.Scripts.Core.ChunkCore.Saving.Components;
using ChunkCore.OnBlockChanged.FixedNotification.Components;

namespace Assets.Scripts.Core.ChunkCore.Saving.Systems
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
