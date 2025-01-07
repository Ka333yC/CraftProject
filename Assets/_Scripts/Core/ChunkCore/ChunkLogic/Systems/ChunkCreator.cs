using System;
using System.Threading;
using _Scripts.Core.ChunkCore.ChunkLogic.Components;
using _Scripts.Core.ChunkCore.ChunkLogic.Components.Elements;
using _Scripts.Core.ChunkCore.ChunkLogic.Components.Fixed;
using _Scripts.Core.ChunkCore.ChunkLogic.Components.Standart;
using _Scripts.Core.ChunkCore.ChunkLogic.Pools;
using _Scripts.Core.ChunkCore.ChunkLogic.Pools.Components;
using _Scripts.Core.ChunkCore.ChunksContainerLogic.Components;
using _Scripts.Core.ChunkCore.ChunksContainerLogic.Components.Elements;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Scripts.Core.ChunkCore.ChunkLogic.Systems
{
	public class ChunkCreator : IEcsPreInitSystem, IEcsInitSystem, IEcsRunSystem
	{
		private ChunksContainer _chunksContainer;
		private ChunkGameObjectPool _chunkGameObjectPool;

		private EcsPool<ChunkComponent> _chunkPool;
		private EcsPool<FixedChunkСreatedTag> _fixedChunkСreatedPool;
		private EcsPool<StandartChunkСreatedTag> _standartChunkСreatedPool;

		public void PreInit(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();
			_chunkPool = world.GetPool<ChunkComponent>();
			_fixedChunkСreatedPool = world.GetPool<FixedChunkСreatedTag>();
			_standartChunkСreatedPool = world.GetPool<StandartChunkСreatedTag>();
		}

		public void Init(IEcsSystems systems)
		{
			var world = systems.GetWorld();
			_chunksContainer = GetChunksContainer(world);
			_chunkGameObjectPool = GetChunkGameObjectPool(world);
		}

		public void Run(IEcsSystems systems)
		{
			if(_chunksContainer.TryGetPositionWithoutChunkEntity(out var gridPosition))
			{
				CreateChunk(gridPosition, systems);
			}
		}

		private int CreateChunk(Vector3Int gridPosition, IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();
			var chunkEntity = world.NewEntity();
			ref var chunk = ref _chunkPool.Add(chunkEntity);
			chunk.GridPosition = gridPosition;
			chunk.GameObject = _chunkGameObjectPool.Get();
			chunk.GameObject.GridPosition = gridPosition;
			chunk.Blocks = new ChunkSizeBlocks();
			chunk.CancellationTokenSource = new CancellationTokenSource();
			_fixedChunkСreatedPool.Add(chunkEntity);
			_standartChunkСreatedPool.Add(chunkEntity);
			_chunksContainer.SetChunkEntity(gridPosition, chunkEntity);
			return chunkEntity;
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
