using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Core.InventoryCore.ItemLogic;
using Zenject;

namespace _Scripts.Core.BlocksCore
{
	public class BlocksContainers : IEnumerable<IBlockContainer>
	{
		private readonly List<IBlockContainer> _containers = new List<IBlockContainer>();
		private readonly DiContainer _diContainer;
		private readonly ItemsContainers _itemsContainers;

		public Block Air { get; private set; }

		public IBlockContainer this[int itemId]
		{
			get
			{
				return _containers[itemId];
			}
		}

		public BlocksContainers(DiContainer diContainer, ItemsContainers itemsContainers)
		{
			_diContainer = diContainer;
			_itemsContainers = itemsContainers;
		}

		public void Initialize()
		{
			CacheBlockContainers();
			foreach(var blockContainer in _containers)
			{
				if(blockContainer != null)
				{
					_diContainer.Inject(blockContainer);
					blockContainer.Initialize();
				}
			}

			SetAirBlock();
		}

		public IEnumerator<IBlockContainer> GetEnumerator()
		{
			return _containers.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		private void CacheBlockContainers()
		{
			foreach(var itemContainer in _itemsContainers)
			{
				IBlockContainer blockContainer = null;
				if(itemContainer is IBlockItemData blockItemData)
				{
					blockContainer = blockItemData.BlockContainer;
				}

				_containers.Add(blockContainer);
			}
		}

		private void SetAirBlock()
		{
			foreach(var container in _containers)
			{
				if(container is AirBlockContainer airBlockContainer)
				{
					Air = airBlockContainer.CreateBlock();
					return;
				}
			}

			throw new ArgumentException($"BlocksContainers has no {nameof(AirBlockContainer)}");
		}
	}
}
