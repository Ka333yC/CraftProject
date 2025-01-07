using _Scripts.Core.BlocksCore;
using _Scripts.Core.InventoryCore.ItemLogic;
using _Scripts.Implementation.BlocksImplementation;
using UnityEngine;

namespace _Scripts.Implementation.InventoryImplementation.Block
{
	[CreateAssetMenu(fileName = "BlockInventoryItemContainer", menuName = "Blocks/Items/Block item container")]
	public class BlockInventoryItemContainer : ItemContainer, IBlockItemData
	{
		[SerializeField]
		private short _stackSize = 64;
		[SerializeField]
		private Sprite _icon;
		[SerializeField] 
		private BlockContainer _blockContainer;

		private int _id;

		public IBlockContainer BlockContainer => _blockContainer;
		public override int Id => _id;
		public override short StackSize => _stackSize;
		public override Sprite Icon => _icon;

		public override Item Create()
		{
			return new BlockInventoryItem(this);
		}

		public override void Initialize(int id)
		{
			_id = id;
			_blockContainer.Id = id;
		}
	}
}
