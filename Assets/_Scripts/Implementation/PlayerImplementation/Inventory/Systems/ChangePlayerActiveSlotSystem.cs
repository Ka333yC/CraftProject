using _Scripts.Core.InventoryCore.Components;
using _Scripts.Core.PlayerCore.Components;
using Leopotam.EcsLite;

namespace _Scripts.Core.InventoryCore.Systems
{
	public class ChangePlayerActiveSlotSystem : IEcsPreInitSystem, IEcsRunSystem
	{
		private EcsPool<SetActiveSlotComponent> _setActiveSlotPool;
		private EcsPool<ActiveSlotChangedComponent> _activeSlotChangedPool;
		private EcsPool<PlayerComponent> _playerPool;
		private EcsFilter _playerInventoriesToChangeActiveSlotFilter;

		public void PreInit(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();
			_setActiveSlotPool = world.GetPool<SetActiveSlotComponent>();
			_activeSlotChangedPool = world.GetPool<ActiveSlotChangedComponent>();
			_playerPool = world.GetPool<PlayerComponent>();
			_playerInventoriesToChangeActiveSlotFilter = world
				.Filter<SetActiveSlotComponent>()
				.Inc<PlayerComponent>()
				.Inc<InventoryComponent>()
				.End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach(var playerEntity in _playerInventoriesToChangeActiveSlotFilter)
			{
				ref var player = ref _playerPool.Get(playerEntity);
				int previousActiveSlotIndex = player.ActiveSlotIndex;
				ref var setActiveSlot = ref _setActiveSlotPool.Get(playerEntity);
				player.ActiveSlotIndex = setActiveSlot.NewActiveSlotIndex;
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
