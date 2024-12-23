using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

namespace Assets._Scripts.Core.BlocksCore
{
	public sealed class Block
	{
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
				if(block.IsShared)
				{
					return;
				}

				block.Destroy();
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

		public readonly bool IsShared;

		private readonly List<IBlockComponent> _сomponents = new List<IBlockComponent>(1);

		public int Id => Container.Id;
		public IBlockContainer Container { get; private set; }

		private Block(bool isShared)
		{
			IsShared = isShared;
		}

		public void Initialize(IBlockContainer container) 
		{
			Container = container;
			foreach(var componentContainer in container.BlockComponentContainers)
			{
				componentContainer.InitializeBlock(this);
			}
		}

		public void AddComponent(IBlockComponent component)
		{
			_сomponents.Add(component);
		}

		public void Destroy()
		{
			Container = null;
			_сomponents.Clear();
		}

		public string Serialize()
		{
			List<string> serializedData = ListPool<string>.Get();
			foreach(var component in _сomponents)
			{
				if(component is ISerializableBlockComponent serializableComponent)
				{
					serializedData.Add(serializableComponent.Serialize());
				}
			}

			string result = JsonConvert.SerializeObject(serializedData);
			ListPool<string>.Release(serializedData);
			return result;
		}

		public void Populate(string rawSerializedData)
		{
			List<string> serializedData = JsonConvert.DeserializeObject<List<string>>(rawSerializedData);
			int serializedDataIndex = 0;
			foreach(var component in _сomponents)
			{
				if(component is ISerializableBlockComponent serializableComponent)
				{
					serializableComponent.Populate(serializedData[serializedDataIndex++]);
				}
			}
		}

		public bool TryGetComponent<T>(out T result) where T : IBlockComponent
		{
			foreach(var component in _сomponents)
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
	}
}
