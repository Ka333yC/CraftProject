using System;
using System.Collections.Generic;
using _Scripts.Core.BlocksCore;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;
using static _Scripts.Core.BlocksCore.Block;

namespace _Scripts.Implementation.BlocksImplementation
{
	public abstract class UniqueBlockArchetype : BlockArchetype, ISerializableBlockArchetype
	{
		[SerializeReference, SubclassSelector]
		protected IBlockComponent[] _blockComponents = Array.Empty<IBlockComponent>();
		[SerializeReference, SubclassSelector]
		protected IBlockPlaceableChecker[] _blockPlaceableCheckers = Array.Empty<IBlockPlaceableChecker>();

		protected int _id;
		protected ISerializableBlockComponent[] _serializableBlockComponents;

		public override int Id => _id;

		public override void Initialize(int id)
		{
			_id = id;
			_serializableBlockComponents = GetSerializableBlockComponents().ToArray();
		}

		public override Block CreateBlock()
		{
			var result = BlockPool.Shared.Rent(false);
			result.Archetype = this;
			foreach(var component in _blockComponents)
			{
				var newComponent = component.Clone();
				newComponent.InitializeBlock(result);
			}

			return result;
		}

		public override bool IsPlaceable(Vector3Int worldPosition)
		{
			foreach(var placeableChecker in _blockPlaceableCheckers)
			{
				if(!placeableChecker.IsPlaceable(worldPosition))
				{
					return false;
				}
			}

			return true;
		}

		public override bool TryGetComponent<T>(out T result)
		{
			foreach(var component in _blockComponents)
			{
				if(component is T resultComponent)
				{
					result = resultComponent;
					return true;
				}
			}

			result = default;
			return false;
		}

		public string Serialize(Block block)
		{
			List<string> serializedData = UnityEngine.Pool.ListPool<string>.Get();
			foreach(var componentContainer in _serializableBlockComponents)
			{
				serializedData.Add(componentContainer.Serialize(block));
			}

			string result = JsonConvert.SerializeObject(serializedData);
			UnityEngine.Pool.ListPool<string>.Release(serializedData);
			return result;
		}

		public Block CreateBlock(string serializedBlock)
		{
			var result = BlockPool.Shared.Rent(false);
			result.Archetype = this;
			List<string> serializedData = JsonConvert.DeserializeObject<List<string>>(serializedBlock);
			int serializedDataIndex = 0;
			foreach(var componentContainer in _blockComponents)
			{
				if(componentContainer is ISerializableBlockComponent serializableComponent)
				{
					var newComponent = (ISerializableBlockComponent)serializableComponent.Clone();
					newComponent.InitializeBlock(result, serializedData[serializedDataIndex++]);
				}
				else
				{
					var newComponent = componentContainer.Clone();
					newComponent.InitializeBlock(result);
				}
			}

			return result;
		}

		protected List<ISerializableBlockComponent> GetSerializableBlockComponents() 
		{
			var result = new List<ISerializableBlockComponent>();
			foreach(var component in _blockComponents)
			{
				if(component is ISerializableBlockComponent serializableComponent)
				{
					result.Add(serializableComponent);
				}
			}

			return result;
		}

		[Inject]
		private void Inject(DiContainer container)
		{
			foreach(var component in _blockComponents)
			{
				container.Inject(component);
			}

			foreach(var placeableChecker in _blockPlaceableCheckers)
			{
				container.Inject(placeableChecker);
			}
		}
	}
}
