using System.Collections.Generic;
using _Scripts.Core.ChunkCore.BlockChanging.Components;
using _Scripts.Core.ChunkCore.BlockChanging.FixedNotification.Components;
using _Scripts.Core.ChunkCore.ChunkLogic.Components;
using Leopotam.EcsLite;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts.Core.ChunkCore.BlockChanging.FixedNotification.Systems
{
	public class FixedBlocksChangedNotificationSystem : IEcsPreInitSystem, IEcsRunSystem
	{
		private EcsPool<BlocksChangedComponent> _blocksChangedPool;
		private EcsPool<FixedBlocksChangedComponent> _fixedBlocksChangedPool;
		private EcsFilter _chunksWithChangedBlocksFilter;

		public void PreInit(IEcsSystems systems)
		{
			var world = systems.GetWorld();
			_blocksChangedPool = world.GetPool<BlocksChangedComponent>();
			_fixedBlocksChangedPool = world.GetPool<FixedBlocksChangedComponent>();
			_chunksWithChangedBlocksFilter = world
				.Filter<ChunkComponent>()
				.Inc<BlocksChangedComponent>()
				.End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach(var chunkWithChangedBlocksEntity in _chunksWithChangedBlocksFilter)
			{
				ref var blocksChangedComponent =
					ref _blocksChangedPool.Get(chunkWithChangedBlocksEntity);
				NotifyFixedBlockChanged(chunkWithChangedBlocksEntity,
					blocksChangedComponent.ChangedBlocksPositions);
			}
		}

		private void NotifyFixedBlockChanged(int chunkEntity, IEnumerable<Vector3Int> blockPositions)
		{
			LinkedList<Vector3Int> changedBlockPositions;
			if(_fixedBlocksChangedPool.Has(chunkEntity))
			{
				changedBlockPositions = _fixedBlocksChangedPool.Get(chunkEntity).ChangedBlocksPositions;
			}
			else
			{
				changedBlockPositions = new LinkedList<Vector3Int>();
				ref var blockChanged = ref _fixedBlocksChangedPool.Add(chunkEntity);
				blockChanged.ChangedBlocksPositions = changedBlockPositions;
			}

			changedBlockPositions.AddRange(blockPositions);
		}
	}
}
