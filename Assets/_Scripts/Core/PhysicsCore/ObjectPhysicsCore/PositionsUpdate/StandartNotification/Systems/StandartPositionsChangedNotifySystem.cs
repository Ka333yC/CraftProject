using _Scripts.Core.PhysicsCore.ObjectPhysicsCore.Components;
using _Scripts.Core.PhysicsCore.ObjectPhysicsCore.PositionsUpdate.Components;
using _Scripts.Core.PhysicsCore.ObjectPhysicsCore.PositionsUpdate.StandartNotification.Components;
using Leopotam.EcsLite;

namespace _Scripts.Core.PhysicsCore.ObjectPhysicsCore.PositionsUpdate.StandartNotification.Systems
{
	public class StandartPositionsChangedNotifySystem : IEcsPreInitSystem, IEcsRunSystem
	{
		private EcsPool<BlockPositionChangedComponent> _blockPositionChangedPool;
		private EcsPool<GridPositionChangedComponent> _gridPositionChangedPool;
		private EcsPool<StandartBlockPositionChangedComponent> _standartBlockPositionChangedPool;
		private EcsPool<StandartGridPositionChangedComponent> _standartGridPositionChangedPool;
		private EcsFilter _positionsChangedBlockPositionFilter;
		private EcsFilter _positionsChangedGridPositionFilter;

		public void PreInit(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();
			_blockPositionChangedPool = world.GetPool<BlockPositionChangedComponent>();
			_gridPositionChangedPool = world.GetPool<GridPositionChangedComponent>();
			_standartBlockPositionChangedPool = world.GetPool<StandartBlockPositionChangedComponent>();
			_standartGridPositionChangedPool = world.GetPool<StandartGridPositionChangedComponent>();
			// Exc если прошлое уведомление не было обработано, то просто пропускаем
			// т.к. standart system помнит только самое первое изменение, а не последующие
			_positionsChangedBlockPositionFilter = world
				.Filter<ObjectPhysicsComponent>()
				.Inc<BlockPositionChangedComponent>()
				.Exc<StandartBlockPositionChangedComponent>()
				.End();
			_positionsChangedGridPositionFilter = world
				.Filter<ObjectPhysicsComponent>()
				.Inc<GridPositionChangedComponent>()
				.Exc<StandartGridPositionChangedComponent>()
				.End();
		}

		public void Run(IEcsSystems systems)
		{
			NotifyBlockPositionsChanged();
			NotifyGridPositionsChanged();
		}

		private void NotifyBlockPositionsChanged()
		{
			foreach(var objectPhysicsEntity in _positionsChangedBlockPositionFilter)
			{
				var blockPositionChanged =
					_blockPositionChangedPool.Get(objectPhysicsEntity);
				ref var standartBlockPositionChanged = 
					ref _standartBlockPositionChangedPool.Add(objectPhysicsEntity);
				standartBlockPositionChanged.PreviousBlockPosition = blockPositionChanged.PreviousBlockPosition;
			}
		}

		private void NotifyGridPositionsChanged()
		{
			foreach(var objectPhysicsEntity in _positionsChangedGridPositionFilter)
			{
				var gridPositionChanged =
					_gridPositionChangedPool.Get(objectPhysicsEntity);
				ref var standartGridPositionChanged = ref _standartGridPositionChangedPool.Add(objectPhysicsEntity);
				standartGridPositionChanged.PreviousGridPosition = gridPositionChanged.PreviousGridPosition;
			}
		}
	}
}
