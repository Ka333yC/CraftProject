using System;
using _Scripts.Core.PlayerCore;
using _Scripts.Implementation.InventoryImplementation.Block;
using _Scripts.Implementation.PlayerImplementation.PlayerInventory;
using _Scripts.Implementation.PlayerImplementation.PlayerInventory.Components;
using Leopotam.EcsLite;
using UnityEngine;
using Voody.UniLeo.Lite;

namespace _Scripts.Implementation.PlayerImplementation.Providers
{
	public class PlayerInventoryProvider: BaseMonoProvider, IConvertToEntity
	{
		[SerializeField]
		private BlockInventoryItemArchetype[] _itemsOnStart = Array.Empty<BlockInventoryItemArchetype>();

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