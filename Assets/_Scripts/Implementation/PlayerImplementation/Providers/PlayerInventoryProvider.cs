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
			ref var inventoryComponent = ref pool.Add(entity);
			var inventory = new Inventory();
			inventoryComponent.Inventory = inventory;
			for(int i = 0; i < _slotsCount; i++)
			{
				inventory.AddSlot(new InventorySlot());
			}

			foreach(var t in _itemsOnStart)
			{
				inventory.TryAddItem(t.Create(), 1);
			}
		}
	}
}