using Assets.Scripts.Core.InventoryCore;
using Assets.Scripts.Core.PlayerCore.Components;
using Leopotam.EcsLite;

namespace Assets.Scripts.Core.PlayerCore.ActiveSlot.Systems
{
	public class ChangeActiveSlotSystem : IEcsPreInitSystem, IEcsRunSystem
	{
		private EcsPool<InventoryComponent> _inventoryPool;
		private EcsPool<SetActiveSlotComponent> _setActiveSlotPool;
		private EcsPool<ActiveSlotChangedComponent> _activeSlotChangedPool;
		private EcsFilter _inventoriesToChangeActiveSlotFilter;

		public void PreInit(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();
			_inventoryPool = world.GetPool<InventoryComponent>();
			_setActiveSlotPool = world.GetPool<SetActiveSlotComponent>();
			_activeSlotChangedPool = world.GetPool<ActiveSlotChangedComponent>();
			_inventoriesToChangeActiveSlotFilter = world
				.Filter<InventoryComponent>()
				.Inc<SetActiveSlotComponent>()
				.End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach(var inventoryEntity in _inventoriesToChangeActiveSlotFilter)
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
