using _Scripts.Core.InventoryCore.SlotLogic;

namespace _Scripts.Core.InventoryCore
{
	public class StandardInventory : Inventory
	{
		public StandardInventory(int slotsCount)
		{
			for(int i = 0; i < slotsCount; i++)
			{
				_slots.Add(new InventorySlot());
			}
		}
	}
}