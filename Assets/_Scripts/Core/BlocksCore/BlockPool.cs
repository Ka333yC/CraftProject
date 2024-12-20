using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._Scripts.Core.BlocksCore
{
	public class BlockPool
	{
		private readonly object _lock = new object();
		private readonly Queue<Block> _freeBlocks = new Queue<Block>();

		public static readonly BlockPool Shared = new BlockPool();

		public Block Rent()
		{
			lock(_lock)
			{
				return _freeBlocks.Any() ? _freeBlocks.Dequeue() : Create();
			}
		}

		public void Return(Block block)
		{
			lock(_lock)
			{
				_freeBlocks.Enqueue(block);
			}
		}

		private Block Create()
		{
			return new Block(false);
		}
	}
}
