using System;
using Assets._Scripts.Core.BlocksCore;
using Assets._Scripts.Implementation.BlocksImplementation;
using Assets._Scripts.Implementation.InventoryImplementation;
using Assets.Scripts.Core.InventoryCore.ItemLogic.BlockItem;
using Assets.Scripts.InventoryCore;
using ChunkCore.BlockData;
using UnityEngine;

namespace Assets.Scripts.Realization.Blocks.InventoryBlockPresentation
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
