using _Scripts.Core.InventoryCore.ItemLogic;
using _Scripts.Core.InventoryCore.SlotLogic;

namespace _Scripts.Core.InventoryCore
{
	public static class InventoryExtensions
	{
		// public static int AddItem(this Inventory inventory, Item sourceItem, int count) 
		// {
		// 	int sourceItemInitialCount = sourceItem.Count;
		// 	// Пытаемся добавить в такой же предмет, куда можно "стакнуть"
		// 	if(inventory.TryAddItem(sourceItem, count, false)) 
		// 	{
		// 		int addedCount = sourceItemInitialCount - sourceItem.Count;
		// 		count -= addedCount;
		// 	}
		//
		// 	// Если ещё остались предметы для добавления, пытаемся добавить в пустые слоты
		// 	if(count != 0)
		// 	{
		// 		inventory.TryAddItem(sourceItem, count, true);
		// 	}
		//
		// 	return sourceItemInitialCount - sourceItem.Count;
		// }
		//
		// private static bool TryAddItem(this Inventory inventory, Item sourceItem, int count, bool isSlotEmpty) 
		// {
		// 	int sourceItemInitialCount = sourceItem.Count;
		// 	foreach(var slot in inventory)
		// 	{
		// 		if(slot.HasItem != isSlotEmpty && slot.TryAddItem(sourceItem, count))
		// 		{
		// 			int addedCount = sourceItemInitialCount - sourceItem.Count;
		// 			count -= addedCount;
		// 			if(count == 0) 
		// 			{
		// 				break;
		// 			}
		// 		}
		// 	}
		//
		// 	return sourceItemInitialCount != sourceItem.Count;
		// }
	}
}