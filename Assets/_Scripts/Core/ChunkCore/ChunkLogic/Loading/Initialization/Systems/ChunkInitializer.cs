using Cysharp.Threading.Tasks;
using Leopotam.EcsLite;
using ChunkCore.LifeTimeControl;
using ChunkCore.Loading.Components;
using System;
using System.Linq;
using System.Threading;
using UnityEngine;
using Assets.Scripts.Core.ChunkCore.LifeTimeControl.Components;
using ChunkCore.ChunksContainerScripts.Components;
using ChunkCore.ChunksContainerScripts;
using Assets.Scripts.Apart.Extensions.Ecs;
using Assets.Scripts.Core.ChunkCore.Saving.Components;
using Assets.Scripts.Core.ChunkCore.ChunkLogic.Components.Elements;
using Assets.Scripts.Core.ChunkCore.ChunkLogic.Loading.Saving.Components;
using TempScripts.TerrainGeneration;
using Assets.Scripts.Core.ChunkCore.ChunkLogic.Loading.TerrainGeneration.Components;
using Assets._Scripts.Core.BlocksCore;

namespace ChunkCore.Loading.Systems
{
	public class ChunkInitializer : IEcsPreInitSystem, IEcsInitSystem, IEcsRunSystem
	{
		private ChunksContainer _chunksContainer;
		private ChunkSerializer _chunkSerializer;
		private ChunkGenerator _chunkGenerator;

		private EcsWorld _world;
		private EcsPool<ChunkComponent> _chunkPool;
		private EcsPool<ChunkInitializingTag> _chunkInitializingPool;
		private EcsPool<ChunkInitializedTag> _chunkInitializedPool;
		private EcsPool<FixedChunkInitializedNotificationTag> _fixedChunkInitializedNotificationPool;
		private EcsPool<StandartChunkInitializedNotificationTag> _standartChunkInitializedNotificationPool;
		private EcsPool<NeedSaveChunkTag> _needSaveChunkPool;
		private EcsFilter _uninitializedChunksFilter;
		private EcsFilter _chunkInitializingFilter;

		public void PreInit(IEcsSystems systems)
		{
			_world = systems.GetWorld();
			_chunkPool = _world.GetPool<ChunkComponent>();
			_chunkInitializingPool = _world.GetPool<ChunkInitializingTag>();
			_chunkInitializedPool = _world.GetPool<ChunkInitializedTag>();
			_fixedChunkInitializedNotificationPool = _world.GetPool<FixedChunkInitializedNotificationTag>();
			_standartChunkInitializedNotificationPool = _world.GetPool<StandartChunkInitializedNotificationTag>();
			_needSaveChunkPool = _world.GetPool<NeedSaveChunkTag>();
			_uninitializedChunksFilter = _world
				.Filter<ChunkComponent>()
				.Exc<ChunkInitializedTag>()
				.End();
			_chunkInitializingFilter = _world
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
			if(_chunkInitializingFilter.Any() || !_uninitializedChunksFilter.Any())
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
					var gridPosition = chunk.GridPosition;
					var blocks = chunk.Blocks;
					await UniTask.RunOnThreadPool(() => GenerateBlocks(blocks, gridPosition, token),
						cancellationToken: token);
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

		private void GenerateBlocks(ChunkSizeBlocks blocks, Vector3Int gridPosition, CancellationToken token)
		{
			Vector3Int worldChunksPosition = ChunkConstantData.GridToWorldPosition(gridPosition);
			for(int y = 0; y < ChunkConstantData.ChunkScale.y; y++)
			{
				token.ThrowIfCancellationRequested();
				for(int x = 0; x < ChunkConstantData.ChunkScale.x; x++)
				{
					for(int z = 0; z < ChunkConstantData.ChunkScale.z; z++)
					{
						Vector3Int blockPosition = new Vector3Int(x, y, z);
						Vector3Int worldBlockPosition = blockPosition + worldChunksPosition;
						Block newBlock = _chunkGenerator.GetRandomBlock(ref worldBlockPosition);
						blocks[x, y, z] = newBlock;
					}
				}
			}
		}

		private void NotifyChunkInitialized(int chunkEntity)
		{
			_chunkInitializedPool.Add(chunkEntity);
			_fixedChunkInitializedNotificationPool.Add(chunkEntity);
			_standartChunkInitializedNotificationPool.Add(chunkEntity);
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
