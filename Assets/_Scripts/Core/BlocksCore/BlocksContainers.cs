using Assets.Scripts.Core.InventoryCore.ItemLogic;
using Assets.Scripts.Core.InventoryCore.ItemLogic.BlockItem;
using System.Collections;
using System.Collections.Generic;
using Zenject;

namespace Assets._Scripts.Core.BlocksCore
{
	public class BlocksContainers : IEnumerable<IBlockContainer>
	{
		private readonly List<IBlockContainer> _containers = new List<IBlockContainer>();
		private readonly ItemsContainers _itemsContainers;

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

		public void Initialize(DiContainer container)
		{
			_containers.Clear();
			CacheBlockContainers();
			foreach(var blockContainer in _containers)
			{
				if(blockContainer != null)
				{
					container.Inject(blockContainer);
					blockContainer.Initialize();
				}
			}
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
	}
}
