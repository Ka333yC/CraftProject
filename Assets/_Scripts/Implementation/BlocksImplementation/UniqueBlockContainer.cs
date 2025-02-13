using System.Collections.Generic;
using _Scripts.Core.BlocksCore;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;
using static _Scripts.Core.BlocksCore.Block;

namespace _Scripts.Implementation.BlocksImplementation
{
	[CreateAssetMenu(fileName = "UniqueBlockContainer", menuName = "Blocks/Unique block container")]
	public class UniqueBlockContainer : BlockContainer, ISerializableBlockContainer
	{
		[SerializeReference, SubclassSelector]
		private IBlockComponentContainer[] _blockComponentContainers;
		[SerializeReference, SubclassSelector]
		private IBlockPlaceableChecker[] _blockPlaceableCheckers;

		private int _id;
		private ISerializableBlockComponentContainer[] _serializableBlockComponentContainers;

		public override int Id => _id;

		public override void Initialize(int id)
		{
			_id = id;
			_serializableBlockComponentContainers = GetSerializableBlockComponentContainers().ToArray();
		}

		public override Block CreateBlock()
		{
			var result = BlockPool.Shared.Rent(false);
			result.Container = this;
			foreach(var blockComponentContainer in _blockComponentContainers)
			{
				blockComponentContainer.InitializeBlock(result);
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

		public override bool TryGetComponentContainer<T>(out T result)
		{
			foreach(var componentContainer in _blockComponentContainers)
			{
				if(componentContainer is T resultContainer)
				{
					result = resultContainer;
					return true;
				}
			}

			result = default;
			return false;
		}

		public string Serialize(Block block)
		{
			List<string> serializedData = UnityEngine.Pool.ListPool<string>.Get();
			foreach(var componentContainer in _serializableBlockComponentContainers)
			{
				serializedData.Add(componentContainer.Serialize(block));
			}

			string result = JsonConvert.SerializeObject(serializedData);
			UnityEngine.Pool.ListPool<string>.Release(serializedData);
			return result;
		}

		public Block Deserialize(string serializedBlock)
		{
			var result = BlockPool.Shared.Rent(false);
			result.Container = this;
			List<string> serializedData = JsonConvert.DeserializeObject<List<string>>(serializedBlock);
			int serializedDataIndex = 0;
			foreach(var componentContainer in _blockComponentContainers)
			{
				if(componentContainer is ISerializableBlockComponentContainer serializableComponent)
				{
					serializableComponent.InitializeBlock(result, serializedData[serializedDataIndex++]);
				}
				else
				{
					componentContainer.InitializeBlock(result);
				}
			}

			return result;
		}

		[Inject]
		private void Inject(DiContainer container)
		{
			foreach(var componentContainer in _blockComponentContainers)
			{
				container.Inject(componentContainer);
			}

			foreach(var placeableChecker in _blockPlaceableCheckers)
			{
				container.Inject(placeableChecker);
			}
		}

		private List<ISerializableBlockComponentContainer> GetSerializableBlockComponentContainers() 
		{
			var result = new List<ISerializableBlockComponentContainer>();
			foreach(var component in _blockComponentContainers)
			{
				if(component is ISerializableBlockComponentContainer serializableComponentContainer)
				{
					result.Add(serializableComponentContainer);
				}
			}

			return result;
		}
	}
}
