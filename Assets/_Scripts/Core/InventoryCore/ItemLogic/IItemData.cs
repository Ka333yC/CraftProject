using UnityEngine;

namespace _Scripts.Core.InventoryCore.ItemLogic
{
	public interface IItemData
	{
		public int Id { get; }
		public short StackSize { get; }
		public Sprite Icon { get; }

		public void Initialize(int id);
	}
}
