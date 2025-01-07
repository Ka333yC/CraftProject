using System;
using _Scripts.Core.ChunkGraphicsCore.BlockGraphics;
using UnityEngine;

namespace _Scripts.Core.ChunkGraphicsCore.ChunkGraphicsLogic.Components.Elements.BlockGraphicsGetters
{
	public class CachedBlocksGraphicsGetter : IBlocksGraphicsGetter, IDisposable
	{
		private readonly IGraphicsBlockComponent[,,] _cachedBlockGraphics;
		private readonly BlocksGraphicsGetter _blocksGraphicsGetter;

		public CachedBlocksGraphicsGetter(BlocksGraphicsGetter blocksGraphicsGetter)
		{
			_cachedBlockGraphics = ChunkSizeArrayPool<IGraphicsBlockComponent>.Shared.Rent();
			_blocksGraphicsGetter = blocksGraphicsGetter;
		}

		public void Dispose()
		{
			ChunkSizeArrayPool<IGraphicsBlockComponent>.Shared.Return(_cachedBlockGraphics);
		}

		public IGraphicsBlockComponent GetBlockGraphics(Vector3Int blockPosition)
		{
			return _cachedBlockGraphics[blockPosition.x, blockPosition.y, blockPosition.z];
		}

		public void CacheBlocksGraphics(Vector3Int from, Vector3Int to) 
		{
			for(int y = from.y; y < to.y; y++)
			{
				for(int x = from.x; x < to.x; x++)
				{
					for(int z = from.z; z < to.z; z++)
					{
						_cachedBlockGraphics[x, y, z] = _blocksGraphicsGetter.GetBlockGraphics(x, y, z);
					}
				}
			}
		}
	}
}
