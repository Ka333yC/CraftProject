using _Scripts.Core.BlocksCore;

namespace _Scripts.Core.InventoryCore.ItemLogic
{
	public interface IBlockItemData : IItemData
	{
		public IBlockContainer BlockContainer { get; }
	}
}
