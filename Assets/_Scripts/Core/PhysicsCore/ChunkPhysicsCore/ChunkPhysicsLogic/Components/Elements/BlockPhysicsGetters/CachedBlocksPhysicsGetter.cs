using System;
using _Scripts.Core.PhysicsCore.ChunkPhysicsCore.BlockPhysics;
using UnityEngine;

namespace _Scripts.Core.PhysicsCore.ChunkPhysicsCore.ChunkPhysicsLogic.Components.Elements.BlockPhysicsGetters
{
	public class CachedBlocksPhysicsGetter : IBlocksPhysicsGetter, IDisposable
	{
		private readonly IPhysicsBlockComponent[,,] _cachedBlockPhysics;
		private readonly BlocksPhysicsGetter _blocksPhysicsGetter;

		public CachedBlocksPhysicsGetter(BlocksPhysicsGetter blocksPhysicsGetter)
		{
			_cachedBlockPhysics = ChunkSizeArrayPool<IPhysicsBlockComponent>.Shared.Rent();
			_blocksPhysicsGetter = blocksPhysicsGetter;
		}

		public void Dispose()
		{
			ChunkSizeArrayPool<IPhysicsBlockComponent>.Shared.Return(_cachedBlockPhysics);
		}

		public IPhysicsBlockComponent GetBlockPhysics(Vector3Int blockPosition)
		{
			return _cachedBlockPhysics[blockPosition.x, blockPosition.y, blockPosition.z];
		}

		public void CacheBlocksPhysics(Vector3Int from, Vector3Int to)
		{
			for(int y = from.y; y < to.y; y++)
			{
				for(int x = from.x; x < to.x; x++)
				{
					for(int z = from.z; z < to.z; z++)
					{
						_cachedBlockPhysics[x, y, z] = _blocksPhysicsGetter.GetBlockPhysics(x, y, z);
					}
				}
			}
		}
	}
}
