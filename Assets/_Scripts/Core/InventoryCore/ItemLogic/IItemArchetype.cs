namespace _Scripts.Core.InventoryCore.ItemLogic
{
	public interface IItemArchetype : IItemData
	{
		public void Initialize(int id);
		public Item Create();
	}
}
