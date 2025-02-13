using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Core.InventoryCore.ItemLogic;

namespace _Scripts.Core.BlocksCore
{
	public class BlocksContainers : IEnumerable<IBlockContainer>
	{
		private readonly List<IBlockContainer> _containers = new List<IBlockContainer>();
		private readonly ItemsContainers _itemsContainers;

		public Block Air { get; private set; }

		public IBlockContainer this[int itemId]
		{
			get
			{
				return _containers[itemId];
			}
		}

		public BlocksContainers(ItemsContainers itemsContainers)
		{
			_itemsContainers = itemsContainers;
		}

		public void Initialize()
		{
			CacheBlockContainers();
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
