using System.Collections.Generic;
using Leopotam.EcsLite;
using ChunkCore.OnBlockChanged.Components;
using ChunkCore.OnBlockChanged.FixedNotification.Components;
using Unity.VisualScripting;
using UnityEngine;
using Assets.Scripts.Core.ChunkCore.LifeTimeControl.Components;

namespace ChunkCore.OnBlockChanged.FixedNotification.Systems
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
