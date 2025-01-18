using System;
using System.Collections.Generic;
using _Scripts.Core.InventoryCore;
using _Scripts.Core.InventoryCore.Components;
using _Scripts.Core.InventoryCore.SlotLogic;
using _Scripts.Implementation.InventoryImplementation.Block;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.Serialization;
using Voody.UniLeo.Lite;

namespace _Scripts.Core.PlayerCore
{
	public class PlayerInventoryProvider: BaseMonoProvider, IConvertToEntity
	{
		[SerializeField]
		private int _slotsCount = 9;
		[FormerlySerializedAs("_items")]
		[SerializeField]
		private BlockInventoryItemContainer[] _itemsOnStart;

		private void OnValidate()
		{
			if(_itemsOnStart != null && _itemsOnStart.Length >= _slotsCount)
			{
				Array.Resize(ref _itemsOnStart, _slotsCount);
			}
		}

		public void Convert(int entity, EcsWorld world)
		{
			var pool = world.GetPool<InventoryComponent>();
			ref var inventory = ref pool.Add(entity);
			var inventoryContainer = new InventorySlotsContainer();
			inventory.SlotsContainer = inventoryContainer;
			for(int i = 0; i < _slotsCount; i++)
			{
				inventoryContainer.AddSlot(new InventorySlot());
			}

			foreach(var t in _itemsOnStart)
			{
				inventoryContainer.TryAddItem(t.Create(), 1);
			}
		}
	}
}