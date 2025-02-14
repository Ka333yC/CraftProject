using _Scripts.Core.ChunkCore.BlockChanging.Components;
using _Scripts.Core.InventoryCore.ItemLogic;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Scripts.Implementation.InventoryImplementation.Block
{
	public class BlockInventoryItem : Item
	{
		private readonly BlockInventoryItemContainer _itemContainer;
		private readonly EcsWorld _world;

		public BlockInventoryItem(BlockInventoryItemContainer itemContainer, EcsWorld world)
		{
			_itemContainer = itemContainer;
			_world = world;
		}

		public override IItemData ItemData
		{
			get
			{
				return _itemContainer;
			}
		}

		public override Item Clone()
		{
			var result = new BlockInventoryItem(_itemContainer, _world);
			result.Count = Count;
			return result;
		}

		public bool Use(Vector3Int worldPosition)
		{
			if(!_itemContainer.BlockArchetype.IsPlaceable(worldPosition))
			{
				return false;
			}

			var block = _itemContainer.BlockArchetype.CreateBlock();
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

		private void SetBlock(Vector3Int worldPosition, Core.BlocksCore.Block block)
		{
			var setBlockPool = _world.GetPool<SetBlockComponent>();
			var entity = _world.NewEntity();
			ref var setBlock = ref setBlockPool.Add(entity);
			setBlock.WorldPosition = worldPosition;
			setBlock.Block = block;
		}
	}
}
