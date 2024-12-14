using System;
using Assets.Scripts.Core.ChunkCore.LifeTimeControl.Components;
using ChunkCore.ChunksContainerScripts;
using ChunkCore.ChunksContainerScripts.Components;
using ChunkCore.LifeTimeControl.Components.Fixed;
using ChunkCore.LifeTimeControl.Components.Standart;
using ChunkCore.Loading.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Assets.Scripts.Core.ChunkCore.LifeTimeControl.Systems
{
	public class ChunkDestroyer : IEcsPreInitSystem, IEcsInitSystem, IEcsRunSystem
	{
		private ChunksContainer _chunksContainer;

		private EcsPool<ChunkComponent> _chunkPool;
		private EcsPool<FixedChunkDestroyedComponent> _fixedChunkDestroyedPool;
		private EcsPool<StandartChunkDestroyedComponent> _standartChunkDestroyedPool;
		private EcsPool<ChunkInitializingTag> _chunkGeneratingPool;
		private EcsPool<ChunkSavingTag> _chunkSavingPool;

		public void PreInit(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();
			_chunkPool = world.GetPool<ChunkComponent>();
			_fixedChunkDestroyedPool = world.GetPool<FixedChunkDestroyedComponent>();
			_standartChunkDestroyedPool = world.GetPool<StandartChunkDestroyedComponent>();
			_chunkGeneratingPool = world.GetPool<ChunkInitializingTag>();
			_chunkSavingPool = world.GetPool<ChunkSavingTag>();
		}

		public void Init(IEcsSystems systems)
		{
			_chunksContainer = GetChunksContainer(systems.GetWorld());
		}

		// TODO: исправить неправильно работающий TryGetChunkEntityWithoutChunkUsers
		public void Run(IEcsSystems systems)
		{
			if(_chunksContainer.TryGetChunkEntityWithoutChunkUsers(out var chunkEntity) &&
				!_chunkGeneratingPool.Has(chunkEntity) &&
				!_chunkSavingPool.Has(chunkEntity))
			{
				DestroyChunk(chunkEntity);
			}
		}

		private void DestroyChunk(int chunkEntity)
		{
			var chunk = _chunkPool.Get(chunkEntity);
			var gridPosition = chunk.GridPosition;
			chunk.Blocks.Dispose();
			chunk.CancellationTokenSource.Cancel();
			chunk.CancellationTokenSource.Dispose();
			NotifyFixedChunkDestroyed(chunkEntity, gridPosition);
			NotifyStandartChunkDestroyed(chunkEntity, gridPosition);
			_chunkPool.Del(chunkEntity);
			_chunksContainer.RemoveChunkEntity(gridPosition);
		}

		private void NotifyFixedChunkDestroyed(int chunkEntity, Vector3Int gridPosition)
		{
			ref var chunkDestroyed = ref _fixedChunkDestroyedPool.Add(chunkEntity);
			chunkDestroyed.GridPosition = gridPosition;
		}

		private void NotifyStandartChunkDestroyed(int chunkEntity, Vector3Int gridPosition)
		{
			ref var chunkDestroyed = ref _standartChunkDestroyedPool.Add(chunkEntity);
			chunkDestroyed.GridPosition = gridPosition;
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
	}
}
