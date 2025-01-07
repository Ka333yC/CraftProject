using System;
using System.Collections.Generic;
using _Scripts.Core.ChunkCore.BlockChanging.Components;
using _Scripts.Core.ChunkCore.ChunkLogic.Components;
using _Scripts.Core.ChunkCore.ChunksContainerLogic.Components;
using _Scripts.Core.ChunkCore.ChunksContainerLogic.Components.Elements;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Scripts.Core.ChunkCore.BlockChanging.Systems
{
	public class BlockChangeSystem : IEcsPreInitSystem, IEcsInitSystem, IEcsRunSystem
	{
		private ChunksContainer _chunksContainer;

		private EcsPool<SetBlockComponent> _setBlockPool;
		private EcsPool<ChunkComponent> _chunkPool;
		private EcsPool<BlocksChangedComponent> _blocksChangedPool;
		private EcsFilter _setBlocksFilter;

		public void PreInit(IEcsSystems systems)
		{
			var world = systems.GetWorld();
			_setBlockPool = world.GetPool<SetBlockComponent>();
			_chunkPool = world.GetPool<ChunkComponent>();
			_blocksChangedPool = world.GetPool<BlocksChangedComponent>();
			_setBlocksFilter = world
				.Filter<SetBlockComponent>()
				.End();
		}

		public void Init(IEcsSystems systems)
		{
			_chunksContainer = GetChunksContainer(systems.GetWorld());
		}

		public void Run(IEcsSystems systems)
		{
			foreach(var setBlockEntity in _setBlocksFilter)
			{
				ref var setBlock = ref _setBlockPool.Get(setBlockEntity);
				SetBlock(ref setBlock);
				_setBlockPool.Del(setBlockEntity);
			}
		}

		private void SetBlock(ref SetBlockComponent setBlock)
		{
			var gridPosition = ChunkConstantData.WorldToGridPosition(setBlock.WorldPosition);
			if(_chunksContainer.TryGetChunk(gridPosition, out int chunkEntity))
			{
				ref var chunk = ref _chunkPool.Get(chunkEntity);
				var blockPositionInChunk =
					ChunkConstantData.WorldToBlockPositionInChunk(setBlock.WorldPosition);
				chunk.Blocks[blockPositionInChunk] = setBlock.Block;
				NotifyBlockChanged(chunkEntity, blockPositionInChunk);
			}
		}

		private void NotifyBlockChanged(int chunkEntity, Vector3Int blockPosition)
		{
			LinkedList<Vector3Int> changedBlockPositions;
			if(_blocksChangedPool.Has(chunkEntity))
			{
				changedBlockPositions = _blocksChangedPool.Get(chunkEntity).ChangedBlocksPositions;
			}
			else
			{
				changedBlockPositions = new LinkedList<Vector3Int>();
				ref var blockChanged = ref _blocksChangedPool.Add(chunkEntity);
				blockChanged.ChangedBlocksPositions = changedBlockPositions;
			}

			changedBlockPositions.AddLast(blockPosition);
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
