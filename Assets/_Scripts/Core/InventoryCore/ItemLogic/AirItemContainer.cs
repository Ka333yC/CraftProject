using _Scripts.Core.BlocksCore;
using UnityEngine;

namespace _Scripts.Core.InventoryCore.ItemLogic
{
	public class AirItemContainer : IItemContainer, IBlockItemData
	{
		private readonly AirBlockArchetype _blockArchetype = new AirBlockArchetype();

		public int Id { get; private set; }
		public short StackSize => 0;
		public Sprite Icon => null;
		public IBlockArchetype BlockArchetype => _blockArchetype;

		public Item Create()
		{
			return null;
		}

		public void Initialize(int id)
		{
			Id = id;
			_blockArchetype.Initialize(id);
		}
	}
}
