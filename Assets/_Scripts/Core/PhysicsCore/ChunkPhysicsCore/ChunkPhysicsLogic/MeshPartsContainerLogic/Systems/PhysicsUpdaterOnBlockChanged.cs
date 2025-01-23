using System;
using System.Collections.Generic;
using _Scripts.Apart.Extensions.Ecs;
using _Scripts.Core.ChunkCore.BlockChanging.FixedNotification.Components;
using _Scripts.Core.ChunkCore.ChunkLogic.Components;
using _Scripts.Core.ChunkCore.ChunksContainerLogic.Components;
using _Scripts.Core.ChunkCore.ChunksContainerLogic.Components.Elements;
using _Scripts.Core.PhysicsCore.ChunkPhysicsCore.ChunkPhysicsLogic.Components;
using _Scripts.Core.PhysicsCore.ChunkPhysicsCore.ChunkPhysicsLogic.MeshGeneration.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Scripts.Core.PhysicsCore.ChunkPhysicsCore.ChunkPhysicsLogic.MeshPartsContainerLogic.Systems
{
	public class PhysicsUpdaterOnBlockChanged : IEcsPreInitSystem, IEcsInitSystem, IEcsRunSystem
	{
		private ChunksContainer _chunksContainer;

		private EcsPool<FixedBlocksChangedComponent> _blockChangedPool;
		private EcsPool<ChunkComponent> _chunkPool;
		private EcsPool<ChunkPhysicsComponent> _chunkPhysicsPool;
		private EcsPool<ChunkPhysicsDirtyMeshTag> _dirtyMeshPool;
		private EcsFilter _chunksPhysicsWithChangedBlocksFilter;

		public void PreInit(IEcsSystems systems)
		{
			var world = systems.GetWorld();
			_blockChangedPool = world.GetPool<FixedBlocksChangedComponent>();
			_chunkPhysicsPool = world.GetPool<ChunkPhysicsComponent>();
			_dirtyMeshPool = world.GetPool<ChunkPhysicsDirtyMeshTag>();
			_chunkPool = world.GetPool<ChunkComponent>();
			_chunksPhysicsWithChangedBlocksFilter = world
				.Filter<FixedBlocksChangedComponent>()
				.Inc<ChunkComponent>()
				.Inc<ChunkPhysicsComponent>()
				.End();
		}

		public void Init(IEcsSystems systems)
		{
			_chunksContainer = GetChunksContainer(systems.GetWorld());
		}

		public void Run(IEcsSystems systems)
		{
			foreach(var chunkEntity in _chunksPhysicsWithChangedBlocksFilter)
			{
				var changedBlockPositions =
					_blockChangedPool.Get(chunkEntity).ChangedBlocksPositions;
				UpdatePhysics(chunkEntity, changedBlockPositions);
			}
		}

		private void UpdatePhysics(int chunkEntity, IEnumerable<Vector3Int> blockPositionsToUpdate) 
		{
			ref var chunkPhysics = ref _chunkPhysicsPool.Get(chunkEntity);
			foreach(var blockPosition in blockPositionsToUpdate)
			{
				chunkPhysics.MeshPartsContainer.UpdateMesh(blockPosition);
				_dirtyMeshPool.AddOrGet(chunkEntity);
				UpdateMeshPartsAroundPosition(chunkEntity, blockPosition);
			}
		}

		private void UpdateMeshPartsAroundPosition(int chunkEntity, Vector3Int blockPosition)
		{
			Vector3Int positionToCheck = blockPosition + Vector3Int.up;
			if(ChunkConstantData.IsPositionInChunkByY(positionToCheck))
			{
				var chunkPhysics = _chunkPhysicsPool.Get(chunkEntity);
				chunkPhysics.MeshPartsContainer.UpdateMesh(positionToCheck);
				_dirtyMeshPool.AddOrGet(chunkEntity);
			}

			positionToCheck = blockPosition + Vector3Int.down;
			if(ChunkConstantData.IsPositionInChunkByY(positionToCheck))
			{
				var chunkPhysics = _chunkPhysicsPool.Get(chunkEntity);
				chunkPhysics.MeshPartsContainer.UpdateMesh(positionToCheck);
				_dirtyMeshPool.AddOrGet(chunkEntity);
			}

			AddMeshPartToUpdateWithBorderCheck(chunkEntity, 
				 blockPosition, Vector3Int.forward);
			AddMeshPartToUpdateWithBorderCheck(chunkEntity,
				 blockPosition, Vector3Int.back);
			AddMeshPartToUpdateWithBorderCheck(chunkEntity,
				 blockPosition, Vector3Int.right);
			AddMeshPartToUpdateWithBorderCheck(chunkEntity,
				 blockPosition, Vector3Int.left);
		}

		private void AddMeshPartToUpdateWithBorderCheck(int chunkEntity, Vector3Int blockPosition, Vector3Int side)
		{
			var positionToCheck = blockPosition + side;
			if(ChunkConstantData.IsPositionInChunk(positionToCheck))
			{
				var chunkPhysics = _chunkPhysicsPool.Get(chunkEntity);
				chunkPhysics.MeshPartsContainer.UpdateMesh(positionToCheck);
				_dirtyMeshPool.AddOrGet(chunkEntity);
			}
			else
			{
				var borderChunkGridPosition = _chunkPool.Get(chunkEntity).GridPosition + side;
				if(_chunksContainer.TryGetChunk(borderChunkGridPosition, out int borderChunk))
				{
					positionToCheck = ChunkConstantData.WorldToBlockPositionInChunk(positionToCheck);
					var chunkPhysics = _chunkPhysicsPool.Get(borderChunk);
					chunkPhysics.MeshPartsContainer.UpdateMesh(positionToCheck);
					_dirtyMeshPool.AddOrGet(borderChunk);
				}
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
	}
}
