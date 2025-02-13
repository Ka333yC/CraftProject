using _Scripts.Core.BlocksCore;
using UnityEngine;

namespace _Scripts.Core.InventoryCore.ItemLogic
{
	public class AirItemContainer : IItemContainer, IBlockItemData
	{
		private readonly AirBlockContainer _blockContainer = new AirBlockContainer();

		public int Id { get; private set; }
		public short StackSize => 0;
		public Sprite Icon => null;
		public IBlockContainer BlockContainer => _blockContainer;

		public Item Create()
		{
			return null;
		}

		public void Initialize(int id)
		{
			Id = id;
			_blockContainer.Initialize(id);
		}
	}
}
