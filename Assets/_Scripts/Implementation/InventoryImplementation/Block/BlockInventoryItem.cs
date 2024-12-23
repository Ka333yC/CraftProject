using System;
using Assets._Scripts.Core.BlocksCore;
using Assets.Scripts.Realization.Blocks.InventoryBlockPresentation;
using Assets.Scripts.Undone.Realization.Blocks.InventoryBlockPresentation;
using ChunkCore.LifeTimeControl;
using ChunkCore.OnBlockChanged.Components;
using TempScripts;
using UnityEngine;

namespace Assets.Scripts.InventoryCore
{
	public class BlockInventoryItem : Item
	{
		private readonly BlockInventoryItemContainer _itemContainer;

		public BlockInventoryItem(BlockInventoryItemContainer itemContainer)
		{
			_itemContainer = itemContainer;
		}

		public override IItemData ItemData
		{
			get
			{
				return _itemContainer;
			}
		}

		public override Item Duplicate()
		{
			return new BlockInventoryItem(_itemContainer);
		}

		public bool Use(Vector3Int worldPosition)
		{
			if(!_itemContainer.BlockContainer.IsPlaceable(worldPosition))
			{
				return false;
			}

			var block = _itemContainer.BlockContainer.CreateBlock();
			SetBlock(worldPosition, block);
			return true;
		}

		public bool UseAsModifiable(Vector3Int worldPosition)
		{
			var isUsed = Use(worldPosition);
			if(isUsed)
			{
				Count--;
			}

			return isUsed;
		}

		private static void SetBlock(Vector3Int worldPosition, Block block)
		{
			var world = Singleton.Instance.EcsWorld;
			var setBlockPool = world.GetPool<SetBlockComponent>();
			var entity = world.NewEntity();
			ref var setBlock = ref setBlockPool.Add(entity);
			setBlock.WorldPosition = worldPosition;
			setBlock.Block = block;
		}
	}
}
