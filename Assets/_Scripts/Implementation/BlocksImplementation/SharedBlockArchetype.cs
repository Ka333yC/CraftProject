using _Scripts.Core.BlocksCore;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;
using static _Scripts.Core.BlocksCore.Block;

namespace _Scripts.Implementation.BlocksImplementation
{
	[CreateAssetMenu(fileName = "BlockArchetype", menuName = "Blocks/Archetypes/Shared block archetype")]
	public class SharedBlockArchetype : BlockArchetype
	{
		[SerializeReference, SubclassSelector]
		private IBlockComponent[] _blockComponents;
		[SerializeReference, SubclassSelector]
		private IBlockPlaceableChecker[] _blockPlaceableCheckers;

		private int _id;
		private Block _sharedBlock;

		public override int Id => _id;

		public override void Initialize(int id)
		{
			_id = id;
			_sharedBlock = BlockPool.Shared.Rent(true);
			_sharedBlock.Archetype = this;
			foreach(var component in _blockComponents)
			{
				var newComponent = component.Clone();
				newComponent.InitializeBlock(_sharedBlock);
			}
		}

		public override Block CreateBlock()
		{
			return _sharedBlock;
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
