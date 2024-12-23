using Assets.Scripts.Core.ChunkPhysicsCore.ChunkPhysicsScripts.Components.Elements.BlockPhysicsGetters;
using Assets.Scripts.Core.ChunkPhysicsCore.ChunkPhysicsScripts.Components.Elements;
using PhysicsCore.ChunkPhysicsCore.BlockPhysics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;
using GraphicsCore.ChunkGraphicsCore.BlockGraphics;
using ChunkCore;

namespace Assets.Scripts.Core.ChunkPhysicsCore.ChunkPhysicsScripts.Components.Elements.BlockPhysicsGetters
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
