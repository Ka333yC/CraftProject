using System.Collections;
using System.Collections.Generic;
using _Scripts.Core.InventoryCore.ItemLogic;
using _Scripts.Core.InventoryCore.SlotLogic;

namespace _Scripts.Core.InventoryCore
{
	public class Inventory : IEnumerable<InventorySlot>
	{
		protected readonly List<InventorySlot> _slots = new List<InventorySlot>(1);

		public int SlotsCount => _slots.Count;
		
		public InventorySlot this[int index] => _slots[index];

		public List<InventorySlot>.Enumerator GetEnumerator()
		{
			return _slots.GetEnumerator();
		}

		IEnumerator<InventorySlot> IEnumerable<InventorySlot>.GetEnumerator()
		{
			return _slots.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _slots.GetEnumerator();
		}
	}
}
