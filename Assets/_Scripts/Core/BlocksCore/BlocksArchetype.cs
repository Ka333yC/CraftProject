using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Core.InventoryCore.ItemLogic;

namespace _Scripts.Core.BlocksCore
{
	public class BlocksArchetype : IEnumerable<IBlockArchetype>
	{
		private readonly List<IBlockArchetype> _archetypes = new List<IBlockArchetype>();
		private readonly ItemsContainers _itemsContainers;

		public Block Air { get; private set; }

		public IBlockArchetype this[int itemId]
		{
			get
			{
				return _archetypes[itemId];
			}
		}

		public BlocksArchetype(ItemsContainers itemsContainers)
		{
			_itemsContainers = itemsContainers;
		}

		public void Initialize()
		{
			CacheBlockArchetypes();
			SetAirBlock();
		}

		public IEnumerator<IBlockArchetype> GetEnumerator()
		{
			return _archetypes.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		private void CacheBlockArchetypes()
		{
			foreach(var itemContainer in _itemsContainers)
			{
				IBlockArchetype blockArchetype = null;
				if(itemContainer is IBlockItemData blockItemData)
				{
					blockArchetype = blockItemData.BlockArchetype;
				}

				_archetypes.Add(blockArchetype);
			}
		}

		private void SetAirBlock()
		{
			foreach(var archetype in _archetypes)
			{
				if(archetype is AirBlockArchetype airBlockArchetype)
				{
					Air = airBlockArchetype.CreateBlock();
					return;
				}
			}

			throw new ArgumentException($"BlocksContainers has no {nameof(AirBlockArchetype)}");
		}
	}
}
