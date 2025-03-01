﻿using System;
using _Scripts.Core.BlocksCore;
using _Scripts.Core.ChunkCore.ChunkLogic.Components;
using _Scripts.Core.ChunkCore.ChunkLogic.Components.Elements;
using _Scripts.Core.ChunkCore.ChunksContainerLogic.Components;
using _Scripts.Core.ChunkCore.ChunksContainerLogic.Components.Elements;
using _Scripts.Core.ChunkGraphicsCore.BlockGraphics;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Scripts.Core.ChunkGraphicsCore.ChunkGraphicsLogic.Components.Elements.BlockGraphicsGetters
{
	public class BlocksGraphicsGetter : IBlocksGraphicsGetter, IDisposable
	{
		private readonly EcsPool<ChunkGraphicsComponent> _chunkGraphicsPool;

		private readonly ChunkSizeBlocks _blocks;
		private readonly ChunksContainer _chunksContainer;

		public BlocksGraphicsGetter ForwardBlocksGraphicsGetter { get; private set; }
		public BlocksGraphicsGetter BackBlocksGraphicsGetter { get; private set; }
		public BlocksGraphicsGetter RightBlocksGraphicsGetter { get; private set; }
		public BlocksGraphicsGetter LeftBlocksGraphicsGetter { get; private set; }

		public BlocksGraphicsGetter(int chunkEntity, EcsWorld world)
		{
			_chunkGraphicsPool = world.GetPool<ChunkGraphicsComponent>();
			
			_chunksContainer = GetChunksContainer(world);
			var chunkPool = world.GetPool<ChunkComponent>();
			var chunk = chunkPool.Get(chunkEntity);
			_blocks = chunk.Blocks;
			CacheBlocksGraphicsContainers(chunk.GridPosition);
		}

		public void Dispose()
		{
			UncacheBlocksGraphicsContainers();
		}

		public IGraphicsBlockComponent GetBlockGraphics(int x, int y, int z)
		{
			Block block = _blocks[x, y, z];
			if(block.TryGetComponent(out IGraphicsBlockComponent component))
			{
				return component;
			}

			return null;
		}

		public IGraphicsBlockComponent GetBlockGraphics(Vector3Int blockPosition)
		{
			return GetBlockGraphics(blockPosition.x, blockPosition.y, blockPosition.z);
		}

		public BlocksGraphicsGetter GetBorderingGetter(Face face) 
		{
			return face switch
			{
				Face.Forward => ForwardBlocksGraphicsGetter,
				Face.Back => BackBlocksGraphicsGetter,
				Face.Right => RightBlocksGraphicsGetter,
				Face.Left => LeftBlocksGraphicsGetter,
				_ => throw new ArgumentException($"Unknown face {face}")
			};
		}

		private void CacheBlocksGraphicsContainers(Vector3Int gridPosition)
		{
			Vector3Int borderPosition = gridPosition + Vector3Int.forward;
			if(TryGetChunkGraphics(borderPosition, out var borderChunkGraphics))
			{
				var borderBlocksGraphicsGetter = borderChunkGraphics.BlocksGraphicsGetter;
				ForwardBlocksGraphicsGetter = borderBlocksGraphicsGetter;
				borderBlocksGraphicsGetter.BackBlocksGraphicsGetter = this;
			}

			borderPosition = gridPosition + Vector3Int.back;
			if(TryGetChunkGraphics(borderPosition, out borderChunkGraphics))
			{
				var borderBlocksGraphicsGetter = borderChunkGraphics.BlocksGraphicsGetter;
				BackBlocksGraphicsGetter = borderBlocksGraphicsGetter;
				borderBlocksGraphicsGetter.ForwardBlocksGraphicsGetter = this;
			}

			borderPosition = gridPosition + Vector3Int.right;
			if(TryGetChunkGraphics(borderPosition, out borderChunkGraphics))
			{
				var borderBlocksGraphicsGetter = borderChunkGraphics.BlocksGraphicsGetter;
				RightBlocksGraphicsGetter = borderBlocksGraphicsGetter;
				borderBlocksGraphicsGetter.LeftBlocksGraphicsGetter = this;
			}

			borderPosition = gridPosition + Vector3Int.left;
			if(TryGetChunkGraphics(borderPosition, out borderChunkGraphics))
			{
				var borderBlocksGraphicsGetter = borderChunkGraphics.BlocksGraphicsGetter;
				LeftBlocksGraphicsGetter = borderBlocksGraphicsGetter;
				borderBlocksGraphicsGetter.RightBlocksGraphicsGetter = this;
			}
		}

		private void UncacheBlocksGraphicsContainers()
		{
			if(ForwardBlocksGraphicsGetter != null)
			{
				ForwardBlocksGraphicsGetter.BackBlocksGraphicsGetter = null;
				ForwardBlocksGraphicsGetter = null;
			}

			if(BackBlocksGraphicsGetter != null)
			{
				BackBlocksGraphicsGetter.ForwardBlocksGraphicsGetter = null;
				BackBlocksGraphicsGetter = null;
			}

			if(RightBlocksGraphicsGetter != null)
			{
				RightBlocksGraphicsGetter.LeftBlocksGraphicsGetter = null;
				RightBlocksGraphicsGetter = null;
			}

			if(LeftBlocksGraphicsGetter != null)
			{
				LeftBlocksGraphicsGetter.RightBlocksGraphicsGetter = null;
				LeftBlocksGraphicsGetter = null;
			}
		}

		private bool TryGetChunkGraphics(Vector3Int gridPosition, out ChunkGraphicsComponent result)
		{
			if(!_chunksContainer.TryGetChunk(gridPosition, out int chunkEntity) ||
			   !_chunkGraphicsPool.Has(chunkEntity))
			{
				result = default;
				return false;
			}

			result = _chunkGraphicsPool.Get(chunkEntity);
			return true;
		}

		private ref ChunksContainer GetChunksContainer(EcsWorld world)
		{
			var chunksContainerPool = world.GetPool<ChunksContainerComponent>();
			var chunksContainerFilter = world.Filter<ChunksContainerComponent>().End();
			foreach(var entity in chunksContainerFilter)
			{
				return ref chunksContainerPool.Get(entity).ChunksContainer;
			}

			throw new Exception($"{nameof(ChunksContainerComponent)} not found");
		}
	}
}
