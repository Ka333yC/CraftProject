using _Scripts.Core.BlocksCore;
using UnityEngine;
using Zenject;
using static _Scripts.Core.BlocksCore.Block;

namespace _Scripts.Implementation.BlocksImplementation
{
	[CreateAssetMenu(fileName = "SharedBlockContainer", menuName = "Blocks/Shared block container")]
	public class SharedBlockContainer : BlockContainer
	{
		[SerializeReference, SubclassSelector]
		private IBlockComponentContainer[] _blockComponentContainers;
		[SerializeReference, SubclassSelector]
		private IBlockPlaceableChecker[] _blockPlaceableCheckers;

		private int _id;
		private Block _sharedBlock;

		public override int Id => _id;

		public override void Initialize(int id)
		{
			_id = id;
			_sharedBlock = BlockPool.Shared.Rent(true);
			_sharedBlock.Container = this;
			foreach(var componentContainer in _blockComponentContainers)
			{
				componentContainer.InitializeBlock(_sharedBlock);
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

		[Inject]
		private void InjectDataContainers(DiContainer container)
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
	}
}
