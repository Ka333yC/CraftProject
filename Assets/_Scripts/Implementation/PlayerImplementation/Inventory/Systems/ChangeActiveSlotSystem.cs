using _Scripts.Core.InventoryCore.Components;
using _Scripts.Core.PlayerCore.Components;
using Leopotam.EcsLite;

namespace _Scripts.Core.InventoryCore.Systems
{
	public class ChangeActiveSlotSystem : IEcsPreInitSystem, IEcsRunSystem
	{
		private EcsPool<InventoryComponent> _inventoryPool;
		private EcsPool<SetActiveSlotComponent> _setActiveSlotPool;
		private EcsPool<ActiveSlotChangedComponent> _activeSlotChangedPool;
		private EcsFilter _playerInventoriesToChangeActiveSlotFilter;

		public void PreInit(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();
			_inventoryPool = world.GetPool<InventoryComponent>();
			_setActiveSlotPool = world.GetPool<SetActiveSlotComponent>();
			_activeSlotChangedPool = world.GetPool<ActiveSlotChangedComponent>();
			_playerInventoriesToChangeActiveSlotFilter = world
				.Filter<SetActiveSlotComponent>()
				.Inc<PlayerComponent>()
				.Inc<InventoryComponent>()
				.End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach(var inventoryEntity in _playerInventoriesToChangeActiveSlotFilter)
			{
				ref var inventory = ref _inventoryPool.Get(inventoryEntity);
				int previousActiveSlotIndex = inventory.ActiveSlotIndex;
				var setActiveSlot = _setActiveSlotPool.Get(inventoryEntity);
				inventory.ActiveSlotIndex = setActiveSlot.NewActiveSlotIndex;
				NotifyActiveSlotChanged(inventoryEntity, previousActiveSlotIndex);
			}
		}

		private void NotifyActiveSlotChanged(int entity, int previousActiveSlotIndex) 
		{
			ref var activeSlotChanged = ref _activeSlotChangedPool.Add(entity);
			activeSlotChanged.PreviousActiveSlotIndex = previousActiveSlotIndex;
		}
	}
}
