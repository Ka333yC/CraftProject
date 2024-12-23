﻿using System;
using System.Collections.Generic;
using Assets.Scripts.Core.ChunkCore.LifeTimeControl.Components;
using ChunkCore.ChunksContainerScripts;
using ChunkCore;
using ChunkCore.ChunksContainerScripts.Components;
using ChunkCore.OnBlockChanged.Components;
using GraphicsCore.ChunkGraphicsCore.LifeTimeControl.Components;
using Leopotam.EcsLite;
using UnityEngine;
using Assets.Scripts.Core.GraphicsCore.ChunkGraphicsCore.MeshUpdating.Components;
using Assets.Scripts.Core.GraphicsCore.ChunkGraphicsCore.MeshPartsContainerUpdating.Components;
using Unity.VisualScripting;
using Extensions.Ecs;

namespace Assets.Scripts.Core.GraphicsCore.ChunkGraphicsCore.MeshPartsContainerUpdating.BlocksUpdating.Systems
{
	public class GraphicsUpdaterOnBlockChanged : IEcsPreInitSystem, IEcsInitSystem, IEcsRunSystem
	{
		private ChunksContainer _chunksContainer;

		private EcsPool<BlocksChangedComponent> _blockChangedPool;
		private EcsPool<ChunkComponent> _chunkPool;
		private EcsPool<ChunkGraphicsComponent> _chunkGraphicsPool;
		private EcsPool<ChunkGraphicsDirtyMeshComponent> _dirtyMeshPool;
		private EcsFilter _chunksGraphicsWithChangedBlocksFilter;

		public void PreInit(IEcsSystems systems)
		{
			var world = systems.GetWorld();
			_blockChangedPool = world.GetPool<BlocksChangedComponent>();
			_chunkGraphicsPool = world.GetPool<ChunkGraphicsComponent>();
			_dirtyMeshPool = world.GetPool<ChunkGraphicsDirtyMeshComponent>();
			_chunkPool = world.GetPool<ChunkComponent>();
			_chunksGraphicsWithChangedBlocksFilter = world
				.Filter<BlocksChangedComponent>()
				.Inc<ChunkComponent>()
				.Inc<ChunkGraphicsComponent>()
				.End();
		}

		public void Init(IEcsSystems systems)
		{
			_chunksContainer = GetChunksContainer(systems.GetWorld());
		}

		public void Run(IEcsSystems systems)
		{
			foreach(var chunkEntity in _chunksGraphicsWithChangedBlocksFilter)
			{
				var changedBlockPositions =
					_blockChangedPool.Get(chunkEntity).ChangedBlocksPositions;
				UpdateGraphics(chunkEntity, changedBlockPositions);
			}
		}

		private void UpdateGraphics(int chunkEntity, IEnumerable<Vector3Int> blockPositionsToUpdate) 
		{
			ref var chunkGraphics = ref _chunkGraphicsPool.Get(chunkEntity);
			foreach(var blockPosition in blockPositionsToUpdate)
			{
				chunkGraphics.MeshPartsContainer.UpdateMesh(blockPosition);
				_dirtyMeshPool.AddOrGet(chunkEntity);
				UpdateMeshPartsAroundPosition(chunkEntity, blockPosition);
			}
		}

		private void UpdateMeshPartsAroundPosition(int chunkEntity, Vector3Int blockPosition)
		{
			Vector3Int positionToCheck = blockPosition + Vector3Int.up;
			if(ChunkConstantData.IsPositionInChunkByY(positionToCheck))
			{
				var chunkGraphics = _chunkGraphicsPool.Get(chunkEntity);
				chunkGraphics.MeshPartsContainer.UpdateMesh(positionToCheck);
				_dirtyMeshPool.AddOrGet(chunkEntity);
			}

			positionToCheck = blockPosition + Vector3Int.down;
			if(ChunkConstantData.IsPositionInChunkByY(positionToCheck))
			{
				var chunkGraphics = _chunkGraphicsPool.Get(chunkEntity);
				chunkGraphics.MeshPartsContainer.UpdateMesh(positionToCheck);
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
				var chunkGraphics = _chunkGraphicsPool.Get(chunkEntity);
				chunkGraphics.MeshPartsContainer.UpdateMesh(positionToCheck);
				_dirtyMeshPool.AddOrGet(chunkEntity);
			}
			else
			{
				var borderChunkGridPosition = _chunkPool.Get(chunkEntity).GridPosition + side;
				if(_chunksContainer.TryGetChunk(borderChunkGridPosition, out int borderChunk))
				{
					positionToCheck = ChunkConstantData.WorldToBlockPositionInChunk(positionToCheck);
					var chunkGraphics = _chunkGraphicsPool.Get(borderChunk);
					chunkGraphics.MeshPartsContainer.UpdateMesh(positionToCheck);
					MarkMeshDirty(chunkEntity, borderChunk);
				}
			}
		}

		private void MarkMeshDirty(int chunkEntity, int associatedChunkEntity)
		{
			ref var dirtyMesh = ref _dirtyMeshPool.AddOrGet(chunkEntity);
			if(dirtyMesh.AssociatedDirtyMeshChunkEntities == null)
			{
				dirtyMesh.AssociatedDirtyMeshChunkEntities = new LinkedList<int>();
			}

			dirtyMesh.AssociatedDirtyMeshChunkEntities.AddLast(associatedChunkEntity);
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
