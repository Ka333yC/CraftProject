using System;
using _Scripts.Core.BlocksCore;
using _Scripts.Core.ChunkCore.ChunkLogic.Components;
using _Scripts.Core.ChunkCore.ChunkLogic.Components.Elements;
using _Scripts.Core.ChunkCore.ChunksContainerLogic.Components;
using _Scripts.Core.ChunkCore.ChunksContainerLogic.Components.Elements;
using _Scripts.Core.PhysicsCore.ChunkPhysicsCore.BlockPhysics;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Scripts.Core.PhysicsCore.ChunkPhysicsCore.ChunkPhysicsLogic.Components.Elements.BlockPhysicsGetters
{
	public class BlocksPhysicsGetter : IBlocksPhysicsGetter, IDisposable
	{
		private readonly EcsPool<ChunkPhysicsComponent> _chunkPhysicsPool;
		private readonly EcsPool<ChunksContainerComponent> _chunksContainerPool;
		private readonly EcsFilter _chunksContainerFilter;

		private readonly ChunkSizeBlocks _blocks;

		public BlocksPhysicsGetter ForwardBlocksPhysicsGetter { get; private set; }
		public BlocksPhysicsGetter BackBlocksPhysicsGetter { get; private set; }
		public BlocksPhysicsGetter RightBlocksPhysicsGetter { get; private set; }
		public BlocksPhysicsGetter LeftBlocksPhysicsGetter { get; private set; }

		public BlocksPhysicsGetter(int chunkEntity, EcsWorld world)
		{
			_chunkPhysicsPool = world.GetPool<ChunkPhysicsComponent>();
			_chunksContainerPool = world.GetPool<ChunksContainerComponent>();
			var chunkPool = world.GetPool<ChunkComponent>();
			_chunksContainerFilter = world
				.Filter<ChunksContainerComponent>()
				.End();
			
			var chunk = chunkPool.Get(chunkEntity);
			_blocks = chunk.Blocks;
			var gridPosition = chunk.GridPosition;
			CacheBlocksPhysicsContainers(gridPosition);
		}

		public void Dispose()
		{
			UncacheBlocksPhysicsContainers();
		}

		public IPhysicsBlockComponent GetBlockPhysics(Vector3Int blockPosition)
		{
			return GetBlockPhysics(blockPosition.x, blockPosition.y, blockPosition.z);
		}

		public IPhysicsBlockComponent GetBlockPhysics(int x, int y, int z)
		{
			Block block = _blocks[x, y, z];
			if(block.TryGetComponent(out IPhysicsBlockComponent component))
			{
				return component;
			}

			return null;
		}

		public BlocksPhysicsGetter GetBorderingGetter(Face face)
		{
			return face switch
			{
				Face.Forward => ForwardBlocksPhysicsGetter,
				Face.Back => BackBlocksPhysicsGetter,
				Face.Right => RightBlocksPhysicsGetter,
				Face.Left => LeftBlocksPhysicsGetter,
				_ => throw new ArgumentException($"Unknown face {face}")
			};
		}

		private void CacheBlocksPhysicsContainers(Vector3Int gridPosition)
		{
			ChunkPhysicsComponent? borderChunkPhysics;
			borderChunkPhysics = GetChunkPhysics(gridPosition + Vector3Int.forward);
			if(borderChunkPhysics.HasValue)
			{
				var borderBlocksPhysicsGetter = borderChunkPhysics.Value.BlocksPhysicsGetter;
				ForwardBlocksPhysicsGetter = borderBlocksPhysicsGetter;
				borderBlocksPhysicsGetter.BackBlocksPhysicsGetter = this;
			}

			borderChunkPhysics = GetChunkPhysics(gridPosition + Vector3Int.back);
			if(borderChunkPhysics.HasValue)
			{
				var borderBlocksPhysicsGetter = borderChunkPhysics.Value.BlocksPhysicsGetter;
				BackBlocksPhysicsGetter = borderBlocksPhysicsGetter;
				borderBlocksPhysicsGetter.ForwardBlocksPhysicsGetter = this;
			}

			borderChunkPhysics = GetChunkPhysics(gridPosition + Vector3Int.right);
			if(borderChunkPhysics.HasValue)
			{
				var borderBlocksPhysicsGetter = borderChunkPhysics.Value.BlocksPhysicsGetter;
				RightBlocksPhysicsGetter = borderBlocksPhysicsGetter;
				borderBlocksPhysicsGetter.LeftBlocksPhysicsGetter = this;
			}

			borderChunkPhysics = GetChunkPhysics(gridPosition + Vector3Int.left);
			if(borderChunkPhysics.HasValue)
			{
				var borderBlocksPhysicsGetter = borderChunkPhysics.Value.BlocksPhysicsGetter;
				LeftBlocksPhysicsGetter = borderBlocksPhysicsGetter;
				borderBlocksPhysicsGetter.RightBlocksPhysicsGetter = this;
			}
		}

		private void UncacheBlocksPhysicsContainers()
		{
			if(ForwardBlocksPhysicsGetter != null)
			{
				ForwardBlocksPhysicsGetter.BackBlocksPhysicsGetter = null;
				ForwardBlocksPhysicsGetter = null;
			}

			if(BackBlocksPhysicsGetter != null)
			{
				BackBlocksPhysicsGetter.ForwardBlocksPhysicsGetter = null;
				BackBlocksPhysicsGetter = null;
			}

			if(RightBlocksPhysicsGetter != null)
			{
				RightBlocksPhysicsGetter.LeftBlocksPhysicsGetter = null;
				RightBlocksPhysicsGetter = null;
			}

			if(LeftBlocksPhysicsGetter != null)
			{
				LeftBlocksPhysicsGetter.RightBlocksPhysicsGetter = null;
				LeftBlocksPhysicsGetter = null;
			}
		}

		private ChunkPhysicsComponent? GetChunkPhysics(Vector3Int gridPosition)
		{
			var chunksContainer = GetChunksContainer();
			if(!chunksContainer.TryGetChunk(gridPosition, out int chunkEntity) ||
				!_chunkPhysicsPool.Has(chunkEntity))
			{
				return null;
			}

			return _chunkPhysicsPool.Get(chunkEntity);
		}

		private ref ChunksContainer GetChunksContainer()
		{
			foreach(var entity in _chunksContainerFilter)
			{
				return ref _chunksContainerPool.Get(entity).ChunksContainer;
			}

			throw new Exception($"{typeof(ChunksContainerComponent).Name} not found");
		}
	}
}
