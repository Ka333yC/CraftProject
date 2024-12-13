using Assets.Scripts.InventoryCore;

namespace Assets.Scripts.Core.InventoryCore.ItemLogic
{
	public interface IItemContainer : IItemData
	{
		public Item Create();
	}
}
