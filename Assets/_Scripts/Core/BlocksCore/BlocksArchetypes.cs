using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Core.InventoryCore.ItemLogic;

namespace _Scripts.Core.BlocksCore
{
	public class BlocksArchetypes : IEnumerable<IBlockArchetype>
	{
		private readonly List<IBlockArchetype> _archetypes = new List<IBlockArchetype>();
		private readonly ItemsArchetypes _itemsArchetypes;

		public Block Air { get; private set; }

		public IBlockArchetype this[int itemId]
		{
			get
			{
				return _archetypes[itemId];
			}
		}

		public BlocksArchetypes(ItemsArchetypes itemsArchetypes)
		{
			_itemsArchetypes = itemsArchetypes;
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
			foreach(var itemArchetype in _itemsArchetypes)
			{
				IBlockArchetype blockArchetype = null;
				if(itemArchetype is IBlockItemData blockItemData)
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

			throw new ArgumentException($"BlocksArchetypes has no {nameof(AirBlockArchetype)}");
		}
	}
}
