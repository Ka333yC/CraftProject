using Assets.Scripts.Core.ChunkGraphicsCore.ChunkGraphicsScripts.Components.Elements.BlockGraphicsGetters;
using ChunkCore;
using GraphicsCore.ChunkGraphicsCore.BlockGraphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

namespace Assets.Scripts.Core.ChunkGraphicsCore.ChunkGraphicsScripts.Components.Elements
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
