using Leopotam.EcsLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._Scripts.Core.BlocksCore
{
	public class Block
	{
		public class BlockPool
		{
			private readonly object _lock = new object();
			private readonly Queue<Block> _freeBlocks = new Queue<Block>();

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
				return new Block();
			}
		}

		public static readonly BlockPool Pool = new BlockPool();
		public static readonly Block Air = new Block();

		private readonly List<IBlockComponent> _staticBlockComponents = new List<IBlockComponent>();
		private readonly List<IBlockComponent> _stateBlockComponents = new List<IBlockComponent>();

		public int Id => Container.Id;
		public bool HasState => _stateBlockComponents.Count > 0;
		public IBlockContainer Container { get; private set; }

		private Block()
		{

		}

		public void Initialize(IBlockContainer container) 
		{
			Container = container;
		}

		public void AddStaticBlockComponent(IBlockComponent blockComponent)
		{
			_staticBlockComponents.Add(blockComponent);
		}

		public void AddStateBlockComponent(IBlockComponent blockComponent)
		{
			_stateBlockComponents.Add(blockComponent);
		}

		public bool TryGetComponent<T>(out T result) where T : IBlockComponent
		{
			foreach(var component in _staticBlockComponents)
			{
				if(component is T)
				{
					result = (T)component;
					return true;
				}
			}

			foreach(var component in _stateBlockComponents)
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

		public bool TryGetStateComponent<T>(out T result) where T : IBlockComponent
		{
			foreach(var component in _stateBlockComponents)
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

		public void PoolIfHasState()
		{
			if(HasState)
			{
				Reset();
				Pool.Return(this);
			}
		}

		private void Reset()
		{
			_staticBlockComponents.Clear();
			_stateBlockComponents.Clear();
		}

		public string Serialize()
		{
			throw new NotImplementedException();
		}

		public void Populate(string serializedData)
		{
			throw new NotImplementedException();
		}
	}
}
