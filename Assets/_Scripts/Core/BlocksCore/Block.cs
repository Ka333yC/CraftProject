using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

namespace Assets._Scripts.Core.BlocksCore
{
	public sealed class Block
	{
		public readonly bool IsShared;

		private readonly List<IBlockComponent> _сomponents = new List<IBlockComponent>(1);

		public int Id => Container.Id;
		public IBlockContainer Container { get; private set; }

		public Block(bool isShared)
		{
			IsShared = isShared;
		}

		public void Initialize(IBlockContainer container) 
		{
			Container = container;
		}

		public void AddComponent(IBlockComponent component)
		{
			_сomponents.Add(component);
		}

		public void Destroy()
		{
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
