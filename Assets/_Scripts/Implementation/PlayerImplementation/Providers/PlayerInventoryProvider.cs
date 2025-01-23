using System;
using System.Collections.Generic;
using _Scripts.Core.InventoryCore;
using _Scripts.Core.InventoryCore.Components;
using _Scripts.Core.InventoryCore.SlotLogic;
using _Scripts.Implementation.InventoryImplementation.Block;
using _Scripts.Implementation.PlayerImplementation.Inventory;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.Serialization;
using Voody.UniLeo.Lite;

namespace _Scripts.Core.PlayerCore
{
	public class PlayerInventoryProvider: BaseMonoProvider, IConvertToEntity
	{
		[SerializeField]
		private BlockInventoryItemContainer[] _itemsOnStart = Array.Empty<BlockInventoryItemContainer>();

		private void OnValidate()
		{
			if(_itemsOnStart.Length >= PlayerConstantData.ToolbarSize)
			{
				Array.Resize(ref _itemsOnStart, PlayerConstantData.ToolbarSize);
			}
		}

		public void Convert(int entity, EcsWorld world)
		{
			var toolbar = AddToolbar(entity, world);
			AddItemsIntoToolbar(toolbar);
		}

		private Toolbar AddToolbar(int entity, EcsWorld world)
		{
			var playerInventoryPool = world.GetPool<PlayerInventoryComponent>();
			ref var playerInventoryComponent = ref playerInventoryPool.Add(entity);
			var toolbar = new Toolbar();
			playerInventoryComponent.Toolbar = toolbar;
			return toolbar;
		}

		private void AddItemsIntoToolbar(Toolbar toolbar)
		{
			for(int i = 0; i < _itemsOnStart.Length; i++)
			{
				var item = _itemsOnStart[i].Create();
				item.Count = 1;
				toolbar[i].TrySetItem(item);
			}
		}
	}
}