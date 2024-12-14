using Assets._Scripts.Core.BlocksCore;
using ChunkCore;
using ChunkCore.LifeTimeControl;
using System;
using UnityEngine;

namespace Assets.Scripts.Core.ChunkCore.ChunkLogic.Components.Elements
{
	public class ChunkSizeBlocks : IDisposable
	{
		private readonly Block[,,] _blocks;

		public Block this[int x, int y, int z]
		{
			get => _blocks[x, y, z];
			set => _blocks[x, y, z] = value;
		}

		public Block this[Vector3Int position]
		{
			get => _blocks[position.x, position.y, position.z];
			set => _blocks[position.x, position.y, position.z] = value;
		} 

		public ChunkSizeBlocks()
		{
			_blocks = ChunkSizeArrayPool<Block>.Shared.Rent();
		}

		public void Dispose()
		{
			ChunkSizeArrayPool<Block>.Shared.Return(_blocks);
		}
	}
}
