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

		private readonly ChunksContainer _chunksContainer;
		private readonly ChunkSizeBlocks _blocks;

		public BlocksPhysicsGetter ForwardBlocksPhysicsGetter { get; private set; }
		public BlocksPhysicsGetter BackBlocksPhysicsGetter { get; private set; }
		public BlocksPhysicsGetter RightBlocksPhysicsGetter { get; private set; }
		public BlocksPhysicsGetter LeftBlocksPhysicsGetter { get; private set; }

		public BlocksPhysicsGetter(int chunkEntity, EcsWorld world)
		{
			_chunkPhysicsPool = world.GetPool<ChunkPhysicsComponent>();

			_chunksContainer = GetChunksContainer(world);
			var chunkPool = world.GetPool<ChunkComponent>();
			var chunk = chunkPool.Get(chunkEntity);
			_blocks = chunk.Blocks;
			CacheBlocksPhysicsContainers(chunk.GridPosition);
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
			Vector3Int borderPosition = gridPosition + Vector3Int.forward;
			if(TryGetChunkPhysics(borderPosition, out var borderChunkPhysics))
			{
				var borderBlocksPhysicsGetter = borderChunkPhysics.BlocksPhysicsGetter;
				ForwardBlocksPhysicsGetter = borderBlocksPhysicsGetter;
				borderBlocksPhysicsGetter.BackBlocksPhysicsGetter = this;
			}

			borderPosition = gridPosition + Vector3Int.back;
			if(TryGetChunkPhysics(borderPosition, out borderChunkPhysics))
			{
				var borderBlocksPhysicsGetter = borderChunkPhysics.BlocksPhysicsGetter;
				BackBlocksPhysicsGetter = borderBlocksPhysicsGetter;
				borderBlocksPhysicsGetter.ForwardBlocksPhysicsGetter = this;
			}

			borderPosition = gridPosition + Vector3Int.right;
			if(TryGetChunkPhysics(borderPosition, out borderChunkPhysics))
			{
				var borderBlocksPhysicsGetter = borderChunkPhysics.BlocksPhysicsGetter;
				RightBlocksPhysicsGetter = borderBlocksPhysicsGetter;
				borderBlocksPhysicsGetter.LeftBlocksPhysicsGetter = this;
			}

			borderPosition = gridPosition + Vector3Int.left;
			if(TryGetChunkPhysics(borderPosition, out borderChunkPhysics))
			{
				var borderBlocksPhysicsGetter = borderChunkPhysics.BlocksPhysicsGetter;
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

		private bool TryGetChunkPhysics(Vector3Int gridPosition, out ChunkPhysicsComponent result)
		{
			if(!_chunksContainer.TryGetChunk(gridPosition, out int chunkEntity) ||
			   !_chunkPhysicsPool.Has(chunkEntity))
			{
				result = default;	
				return false;
			}

			result = _chunkPhysicsPool.Get(chunkEntity);
			return true;
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
