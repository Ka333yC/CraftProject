using System;
using Assets._Scripts.Core.BlocksCore;
using Assets._Scripts.Implementation.BlocksImplementation;
using Assets.Scripts.Core.InventoryCore.ItemLogic.BlockItem;
using Assets.Scripts.InventoryCore;
using Assets.Scripts.Undone.Realization.Blocks.InventoryBlockPresentation;
using ChunkCore.BlockData;
using UnityEngine;

namespace Assets.Scripts.Realization.Blocks.InventoryBlockPresentation
{
	[CreateAssetMenu(fileName = "BlockInventoryItemContainer", menuName = "Blocks/Items/Block item container")]
	public class BlockInventoryItemContainer : BaseInventoryItemContainer, IBlockItemData
	{
		[SerializeField] 
		private BlockContainer _blockContainer;

		public IBlockContainer BlockContainer 
		{
			get 
			{
				return _blockContainer;
			}
		}

		public override Item Create()
		{
			return new BlockInventoryItem(this);
		}

		public override void Initialize(int id)
		{
			base.Initialize(id);
			_blockContainer.Id = id;
		}
	}
}
