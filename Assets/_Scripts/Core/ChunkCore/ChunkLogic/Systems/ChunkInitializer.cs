using System;
using _Scripts.Apart.Extensions.Ecs;
using _Scripts.Core.ChunkCore.ChunkLogic.ChunkGeneration;
using _Scripts.Core.ChunkCore.ChunkLogic.ChunkGeneration.Elements;
using _Scripts.Core.ChunkCore.ChunkLogic.ChunkSerialization;
using _Scripts.Core.ChunkCore.ChunkLogic.ChunkSerialization.Elements;
using _Scripts.Core.ChunkCore.ChunkLogic.Components;
using _Scripts.Core.ChunkCore.ChunkLogic.Components.Fixed;
using _Scripts.Core.ChunkCore.ChunkLogic.Components.Standard;
using _Scripts.Core.ChunkCore.ChunkLogic.Saving.Components;
using _Scripts.Core.ChunkCore.ChunksContainerLogic.Components;
using _Scripts.Core.ChunkCore.ChunksContainerLogic.Components.Elements;
using Cysharp.Threading.Tasks;
using Leopotam.EcsLite;

namespace _Scripts.Core.ChunkCore.ChunkLogic.Systems
{
	public class ChunkInitializer : IEcsPreInitSystem, IEcsInitSystem, IEcsRunSystem
	{
		private ChunksContainer _chunksContainer;
		private ChunkSerializer _chunkSerializer;
		private ChunkGenerator _chunkGenerator;

		private EcsPool<ChunkComponent> _chunkPool;
		private EcsPool<ChunkInitializingTag> _chunkInitializingPool;
		private EcsPool<ChunkInitializedTag> _chunkInitializedPool;
		private EcsPool<FixedChunkInitializedNotificationTag> _fixedChunkInitializedNotificationPool;
		private EcsPool<StandardChunkInitializedNotificationTag> _standardChunkInitializedNotificationPool;
		private EcsPool<NeedSaveChunkTag> _needSaveChunkPool;
		private EcsFilter _uninitializedChunksFilter;
		private EcsFilter _chunksInitializingFilter;

		public void PreInit(IEcsSystems systems)
		{
			var world = systems.GetWorld();
			_chunkPool = world.GetPool<ChunkComponent>();
			_chunkInitializingPool = world.GetPool<ChunkInitializingTag>();
			_chunkInitializedPool = world.GetPool<ChunkInitializedTag>();
			_fixedChunkInitializedNotificationPool = world.GetPool<FixedChunkInitializedNotificationTag>();
			_standardChunkInitializedNotificationPool = world.GetPool<StandardChunkInitializedNotificationTag>();
			_needSaveChunkPool = world.GetPool<NeedSaveChunkTag>();
			_uninitializedChunksFilter = world
				.Filter<ChunkComponent>()
				.Exc<ChunkInitializedTag>()
				.End();
			_chunksInitializingFilter = world
				.Filter<ChunkInitializingTag>()
				.End();
		}

		public void Init(IEcsSystems systems)
		{
			var world = systems.GetWorld();
			_chunksContainer = GetChunksContainer(world);
			_chunkSerializer = GetChunksSerializer(world);
			_chunkGenerator = GetChunkGenerator(world);
		}

		public void Run(IEcsSystems systems)
		{
			if(_chunksInitializingFilter.Any() || !_uninitializedChunksFilter.Any())
			{
				return;
			}

			var chunkToGenerate = _chunksContainer.GetChunkWithLowestPriority(_uninitializedChunksFilter);
			InitializeChunk(chunkToGenerate).Forget();
		}

		private async UniTask InitializeChunk(int chunkEntity)
		{
			var chunk = _chunkPool.Get(chunkEntity);
			var token = chunk.CancellationTokenSource.Token;
			try
			{
				_chunkInitializingPool.Add(chunkEntity);
				var isLoaded = await _chunkSerializer.Populate(chunkEntity, token);
				if(!isLoaded)
				{
					await _chunkGenerator.GenerateBlocks(chunkEntity);
					// Если чанк ранее не существовал, надо его сохранить, чтобы потом ещё раз не генерировать
					_needSaveChunkPool.Add(chunkEntity);
				}

				NotifyChunkInitialized(chunkEntity);
			}
			catch(OperationCanceledException)
			{
			}
			finally
			{
				_chunkInitializingPool.Del(chunkEntity);
			}
		}

		private void NotifyChunkInitialized(int chunkEntity)
		{
			_chunkInitializedPool.Add(chunkEntity);
			_fixedChunkInitializedNotificationPool.Add(chunkEntity);
			_standardChunkInitializedNotificationPool.Add(chunkEntity);
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

			throw new Exception($"{typeof(ChunksContainerComponent).Name} not found");
		}

		private ChunkSerializer GetChunksSerializer(EcsWorld world)
		{
			var pool = world.GetPool<ChunkSerializerComponent>();
			var filter = world
				.Filter<ChunkSerializerComponent>()
				.End();
			foreach(var entity in filter)
			{
				return pool.Get(entity).ChunkSerializer;
			}

			throw new Exception($"{typeof(ChunkSerializerComponent).Name} not found");
		}

		private ChunkGenerator GetChunkGenerator(EcsWorld world)
		{
			var pool = world.GetPool<ChunkGeneratorComponent>();
			var filter = world
				.Filter<ChunkGeneratorComponent>()
				.End();
			foreach(var entity in filter)
			{
				return pool.Get(entity).ChunkGenerator;
			}

			throw new Exception($"{typeof(ChunkGeneratorComponent).Name} not found");
		}
	}
}
