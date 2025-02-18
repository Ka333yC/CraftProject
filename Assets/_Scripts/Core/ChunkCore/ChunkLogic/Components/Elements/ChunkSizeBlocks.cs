using System;
using _Scripts.Core.BlocksCore;
using UnityEngine;
using static _Scripts.Core.BlocksCore.Block;

namespace _Scripts.Core.ChunkCore.ChunkLogic.Components.Elements
{
	public class ChunkSizeBlocks : IDisposable
	{
		private readonly object _lockObject = new object();
		private readonly Block[,,] _blocks;
		private readonly Block _airBlock;

		public Block this[int x, int y, int z]
		{
			get
			{
				lock(_lockObject)
				{
					return IsDisposed ? _airBlock : _blocks[x, y, z];
				}
			}

			set
			{
				lock(_lockObject)
				{
					if(IsDisposed)
					{
						return;
					}
					
					var block = _blocks[x, y, z];
					if(block != null && !block.IsShared)
					{
						ReleaseBlock(block);
					}

					_blocks[x, y, z] = value;
				}
			}
		}

		public Block this[Vector3Int position]
		{
			get => this[position.x, position.y, position.z];
			set => this[position.x, position.y, position.z] = value;
		} 

		public bool IsDisposed { get; private set; }

		public ChunkSizeBlocks(Block airBlock)
		{
			_blocks = ChunkSizeArrayPool<Block>.Shared.Rent();
			_airBlock = airBlock;
		}

		public void Dispose()
		{
			lock(_lockObject)
			{
				IsDisposed = true;
			}
			
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
