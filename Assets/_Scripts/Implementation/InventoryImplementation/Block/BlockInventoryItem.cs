using _Scripts.Core.ChunkCore.BlockChanging.Components;
using _Scripts.Core.InventoryCore.ItemLogic;
using _Scripts.TempScripts;
using UnityEngine;

namespace _Scripts.Implementation.InventoryImplementation.Block
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

		private static void SetBlock(Vector3Int worldPosition, Core.BlocksCore.Block block)
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
