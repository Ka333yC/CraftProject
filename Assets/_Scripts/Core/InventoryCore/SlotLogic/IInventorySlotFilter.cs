using _Scripts.Core.InventoryCore.ItemLogic;

namespace _Scripts.Core.InventoryCore.SlotLogic
{
	public interface IInventorySlotFilter
	{
		public bool IsPass(Item item);
	}
}
