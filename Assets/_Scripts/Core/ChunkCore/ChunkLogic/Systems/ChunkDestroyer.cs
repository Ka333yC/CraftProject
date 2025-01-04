using System;
using Assets.Scripts.Core.ChunkCore.LifeTimeControl.Components;
using ChunkCore.ChunksContainerScripts;
using ChunkCore.ChunksContainerScripts.Components;
using ChunkCore.LifeTimeControl.Components.Fixed;
using ChunkCore.LifeTimeControl.Components.Standart;
using ChunkCore.Loading.Components;
using Cysharp.Threading.Tasks;
using Leopotam.EcsLite;
using PhysicsCore.ChunkPhysicsCore.Cache.ChunkPhysicsMeshColliderPoolScripts;
using PhysicsCore.ChunkPhysicsCore.Cache.ChunkPhysicsMeshColliderPoolScripts.Components;
using UnityEngine;

namespace Assets.Scripts.Core.ChunkCore.LifeTimeControl.Systems
{
	public class ChunkDestroyer : IEcsPreInitSystem, IEcsInitSystem, IEcsRunSystem
	{
		private ChunksContainer _chunksContainer;
		private ChunkGameObjectPool _chunkGameObjectPool;
		private bool _isChunkDestroying = false;

		private EcsPool<ChunkComponent> _chunkPool;
		private EcsPool<FixedChunkDestroyedComponent> _fixedChunkDestroyedPool;
		private EcsPool<StandartChunkDestroyedComponent> _standartChunkDestroyedPool;
		private EcsPool<ChunkInitializingTag> _chunkInitializingPool;
		private EcsPool<ChunkSavingTag> _chunkSavingPool;

		public void PreInit(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();
			_chunkPool = world.GetPool<ChunkComponent>();
			_fixedChunkDestroyedPool = world.GetPool<FixedChunkDestroyedComponent>();
			_standartChunkDestroyedPool = world.GetPool<StandartChunkDestroyedComponent>();
			_chunkInitializingPool = world.GetPool<ChunkInitializingTag>();
			_chunkSavingPool = world.GetPool<ChunkSavingTag>();
		}

		public void Init(IEcsSystems systems)
		{
			var world = systems.GetWorld();
			_chunksContainer = GetChunksContainer(world);
			_chunkGameObjectPool = GetChunkGameObjectPool(world);
		}

		public void Run(IEcsSystems systems)
		{
			if(_chunksContainer.TryGetChunkEntityWithoutChunkUsers(out var chunkEntity) &&
				!_chunkInitializingPool.Has(chunkEntity) &&
				!_chunkSavingPool.Has(chunkEntity))
			{
				DestroyChunk(chunkEntity);
			}
		}

		private void DestroyChunk(int chunkEntity)
		{
			var chunk = _chunkPool.Get(chunkEntity);
			var gridPosition = chunk.GridPosition;
			NotifyFixedChunkDestroyed(chunkEntity, gridPosition);
			NotifyStandartChunkDestroyed(chunkEntity, gridPosition);
			_chunkPool.Del(chunkEntity);
			_chunksContainer.RemoveChunkEntity(gridPosition);
			DisposeChunk(chunk).Forget();
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

		private async UniTaskVoid DisposeChunk(ChunkComponent chunk)
		{
			_isChunkDestroying = true;
			try
			{
				chunk.CancellationTokenSource.Cancel();
				chunk.CancellationTokenSource.Dispose();
				_chunkGameObjectPool.Return(chunk.GameObject);
				await UniTask.RunOnThreadPool(() => chunk.Blocks.Dispose());
			}
			finally
			{
				_isChunkDestroying = false;
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

			throw new Exception($"{typeof(ChunksContainerComponent).Name} not found");
		}

		private ChunkGameObjectPool GetChunkGameObjectPool(EcsWorld world)
		{
			var componentPool = world.GetPool<ChunkGameObjectPoolComponent>();
			var filter = world
				.Filter<ChunkGameObjectPoolComponent>()
				.End();
			foreach(var entity in filter)
			{
				return componentPool.Get(entity).Pool;
			}

			throw new Exception($"{typeof(ChunkGameObjectPoolComponent).Name} not found");
		}
	}
}
