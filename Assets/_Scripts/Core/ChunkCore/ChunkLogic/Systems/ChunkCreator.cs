using System;
using Assets.Scripts.Core.ChunkCore.LifeTimeControl.Components;
using ChunkCore.ChunksContainerScripts.Components;
using ChunkCore.LifeTimeControl.Components.Fixed;
using ChunkCore.LifeTimeControl.Components.Standart;
using Leopotam.EcsLite;
using System.Threading;
using UnityEngine;
using ChunkCore.ChunksContainerScripts;
using Assets.Scripts.Core.ChunkCore.ChunkLogic.Components.Elements;
using Zenject;
using Assets._Scripts.Core.BlocksCore;
using PhysicsCore.ChunkPhysicsCore.Cache.ChunkPhysicsMeshColliderPoolScripts;
using PhysicsCore.ChunkPhysicsCore.Cache.ChunkPhysicsMeshColliderPoolScripts.Components;
using ChunkCore;

namespace Assets.Scripts.Core.ChunkCore.LifeTimeControl.Systems
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
			chunk.GameObject.transform.position = ChunkConstantData.GridToWorldPosition(gridPosition);
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
