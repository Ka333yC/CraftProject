using _Scripts.Core.InventoryCore.Components;
using _Scripts.Core.PlayerCore.Components;
using Leopotam.EcsLite;

namespace _Scripts.Core.InventoryCore.Systems
{
	public class ChangePlayerActiveSlotSystem : IEcsPreInitSystem, IEcsRunSystem
	{
		private EcsPool<SetActiveSlotComponent> _setActiveSlotPool;
		private EcsPool<ActiveSlotChangedComponent> _activeSlotChangedPool;
		private EcsPool<PlayerInventoryComponent> _playerInventoryPool;
		private EcsFilter _playerInventoriesToChangeActiveSlotFilter;

		public void PreInit(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();
			_setActiveSlotPool = world.GetPool<SetActiveSlotComponent>();
			_activeSlotChangedPool = world.GetPool<ActiveSlotChangedComponent>();
			_playerInventoryPool = world.GetPool<PlayerInventoryComponent>();
			_playerInventoriesToChangeActiveSlotFilter = world
				.Filter<SetActiveSlotComponent>()
				.Inc<PlayerComponent>()
				.Inc<PlayerInventoryComponent>()
				.End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach(var playerEntity in _playerInventoriesToChangeActiveSlotFilter)
			{
				ref var playerInventory = ref _playerInventoryPool.Get(playerEntity);
				int previousActiveSlotIndex = playerInventory.ActiveSlotIndex;
				ref var setActiveSlot = ref _setActiveSlotPool.Get(playerEntity);
				playerInventory.ActiveSlotIndex = setActiveSlot.NewActiveSlotIndex;
				NotifyActiveSlotChanged(playerEntity, previousActiveSlotIndex);
				_setActiveSlotPool.Del(playerEntity);
			}
		}

		private void NotifyActiveSlotChanged(int entity, int previousActiveSlotIndex) 
		{
			ref var activeSlotChanged = ref _activeSlotChangedPool.Add(entity);
			activeSlotChanged.PreviousActiveSlotIndex = previousActiveSlotIndex;
		}
	}
}
