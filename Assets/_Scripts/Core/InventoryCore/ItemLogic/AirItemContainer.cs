using _Scripts.Core.BlocksCore;
using UnityEngine;

namespace _Scripts.Core.InventoryCore.ItemLogic
{
	public class AirItemContainer : IItemContainer, IBlockItemData
	{
		private AirBlockContainer _blockContainer = new AirBlockContainer();

		public int Id { get; private set; }

		public short StackSize
		{
			get
			{
				return 0;
			}
		}

		public Sprite Icon
		{
			get 
			{
				return null;
			}
		}

		public IBlockContainer BlockContainer 
		{
			get
			{
				return _blockContainer;
			}
		} 

		public Item Create()
		{
			return null;
		}

		public void Initialize(int id)
		{
			Id = id;
			_blockContainer.Id = id;
		}
	}
}
