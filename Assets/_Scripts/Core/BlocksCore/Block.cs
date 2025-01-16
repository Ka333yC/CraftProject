﻿using System.Collections.Generic;
using System.Linq;

namespace _Scripts.Core.BlocksCore
{
	public sealed class Block
	{
		public readonly bool IsShared;

		private readonly List<IBlockComponent> _components = new List<IBlockComponent>(1);

		public int Id => Container.Id;
		public IBlockContainer Container { get; set; }

		private Block(bool isShared)
		{
			IsShared = isShared;
		}

		public void AddComponent(IBlockComponent component)
		{
			_components.Add(component);
		}

		public void Release()
		{
			Container = null;
			_components.Clear();
		}

		public bool TryGetComponent<T>(out T result) where T : IBlockComponent
		{
			foreach(var component in _components)
			{
				if(component is T)
				{
					result = (T)component;
					return true;
				}
			}

			result = default;
			return false;
		}

		public class BlockPool
		{
			private readonly object _lock = new object();
			private readonly Queue<Block> _freeBlocks = new Queue<Block>();

			public static readonly BlockPool Shared = new BlockPool();

			public Block Rent(bool isShared)
			{
				if(isShared)
				{
					return new Block(true);
				}

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
}
