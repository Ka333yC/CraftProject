using Assets._Scripts.Core.BlocksCore;
using ChunkCore;
using ChunkCore.LifeTimeControl;
using System;
using System.Threading;
using UnityEngine;
using static Assets._Scripts.Core.BlocksCore.Block;

namespace Assets.Scripts.Core.ChunkCore.ChunkLogic.Components.Elements
{
	public class ChunkSizeBlocks : IDisposable
	{
		private readonly Block[,,] _blocks;

		public Block this[int x, int y, int z]
		{
			get
			{
				return _blocks[x, y, z];
			}

			set
			{
				var block = _blocks[x, y, z];
				if(block != null && !block.IsShared)
				{
					ReleaseBlock(block);
				}

				_blocks[x, y, z] = value;
			}
		}

		public Block this[Vector3Int position]
		{
			get => this[position.x, position.y, position.z];
			set => this[position.x, position.y, position.z] = value;
		} 

		public ChunkSizeBlocks()
		{
			_blocks = ChunkSizeArrayPool<Block>.Shared.Rent();
		}

		public void Dispose()
		{
			foreach(var block in _blocks)
			{
				// Проверка на null - это костыль. Если чанк ещё не инициализирован, то у него весь массив будет
				// состоять из null объектов
				if(block != null && !block.IsShared)
				{
					ReleaseBlock(block);
				}
			}

			ChunkSizeArrayPool<Block>.Shared.Return(_blocks, true);
		}

		private void ReleaseBlock(Block block)
		{
			block.Release();
			BlockPool.Shared.Return(block);
		}
	}
}
