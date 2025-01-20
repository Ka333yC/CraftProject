namespace _Scripts.Core.InventoryCore.ItemLogic
{
	public interface IItemContainer : IItemData
	{
		public void Initialize(int id);
		public Item Create();
	}
}
