using System;
using _Scripts.Apart.Extensions.Ecs;
using _Scripts.Core.ChunkCore.ChunkLogic.ChunkSerialization;
using _Scripts.Core.ChunkCore.ChunkLogic.ChunkSerialization.Elements;
using _Scripts.Core.ChunkCore.ChunkLogic.Components;
using _Scripts.Core.ChunkCore.ChunkLogic.Saving.Components;
using _Scripts.Core.ChunkCore.ChunksContainerLogic.Components;
using _Scripts.Core.ChunkCore.ChunksContainerLogic.Components.Elements;
using Cysharp.Threading.Tasks;
using Leopotam.EcsLite;

namespace _Scripts.Core.ChunkCore.ChunkLogic.Saving.Systems
{
	public class ChunkSaveSystem : IEcsPreInitSystem, IEcsInitSystem, IEcsRunSystem
	{
		private ChunksContainer _chunksContainer;
		private ChunkSerializer _chunkSerializer;

		private EcsPool<ChunkSavingTag> _chunkSavingPool;
		private EcsPool<NeedSaveChunkTag> _needSaveChunkPool;
		private EcsFilter _chunksToSaveFilter;
		private EcsFilter _savingChunksFilter;

		public void PreInit(IEcsSystems systems)
		{
			var world = systems.GetWorld();
			_chunkSavingPool = world.GetPool<ChunkSavingTag>();
			_needSaveChunkPool = world.GetPool<NeedSaveChunkTag>();
			_chunksToSaveFilter = world
				.Filter<ChunkComponent>()
				.Inc<ChunkInitializedTag>()
				.Inc<NeedSaveChunkTag>()
				.Exc<ChunkSavingTag>()
				.End();
			_savingChunksFilter = world
				.Filter<ChunkSavingTag>()
				.End();
		}

		public void Init(IEcsSystems systems)
		{
			var world = systems.GetWorld();
			_chunksContainer = GetChunksContainer(world);
			_chunkSerializer = GetChunkSerializer(world);
		}

		public void Run(IEcsSystems systems)
		{
			if(_savingChunksFilter.Any() || !_chunksToSaveFilter.Any())
			{
				return;
			}

			var chunkToSave = _chunksContainer.GetChunkWithLowestPriority(_chunksToSaveFilter);
			SaveChunk(chunkToSave).Forget();
		}

		private async UniTask SaveChunk(int chunkEntity)
		{
			try
			{
				_chunkSavingPool.Add(chunkEntity);
				_needSaveChunkPool.Del(chunkEntity);
				await _chunkSerializer.Save(chunkEntity);
			}
			catch(OperationCanceledException)
			{
			}
			finally
			{
				_chunkSavingPool.Del(chunkEntity);
			}
		}

		private ChunksContainer GetChunksContainer(EcsWorld world)
		{
			var pool = world.GetPool<ChunksContainerComponent>();
			var filter = world
				.Filter<ChunksContainerComponent>()
				.End();
			foreach(var entity in filter)
			{
				return pool.Get(entity).ChunksContainer;
			}

			throw new Exception($"{nameof(ChunksContainerComponent)} not found");
		}

		private ChunkSerializer GetChunkSerializer(EcsWorld world)
		{
			var pool = world.GetPool<ChunkSerializerComponent>();
			var filter = world
				.Filter<ChunkSerializerComponent>()
				.End();
			foreach(var entity in filter)
			{
				return pool.Get(entity).ChunkSerializer;
			}

			throw new Exception($"{nameof(ChunkSerializerComponent)} not found");
		}
	}
}
