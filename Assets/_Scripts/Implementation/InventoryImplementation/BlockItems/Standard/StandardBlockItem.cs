﻿using _Scripts.Core.ChunkCore.BlockChanging.Components;
using _Scripts.Core.InventoryCore.ItemLogic;
using _Scripts.Implementation.InventoryImplementation.BlockItems.Standard;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Scripts.Implementation.InventoryImplementation.Block
{
	public class StandardBlockItem : Item
	{
		private readonly StandardBlockItemArchetype _itemArchetype;
		private readonly EcsWorld _world;

		public StandardBlockItem(StandardBlockItemArchetype itemArchetype, EcsWorld world)
		{
			_itemArchetype = itemArchetype;
			_world = world;
		}

		public override IItemData ItemData => _itemArchetype;

		public override Item Clone()
		{
			var result = new StandardBlockItem(_itemArchetype, _world);
			result.Count = Count;
			return result;
		}

		public bool Use(Vector3Int worldPosition)
		{
			if(!_itemArchetype.BlockArchetype.IsPlaceable(worldPosition))
			{
				return false;
			}

			var block = _itemArchetype.BlockArchetype.CreateBlock();
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
