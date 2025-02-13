using _Scripts.Core.PhysicsCore.ObjectPhysicsCore.Components;
using _Scripts.Core.PhysicsCore.ObjectPhysicsCore.PositionsUpdate.Components;
using _Scripts.Core.PhysicsCore.ObjectPhysicsCore.PositionsUpdate.StandardNotification.Components;
using Leopotam.EcsLite;

namespace _Scripts.Core.PhysicsCore.ObjectPhysicsCore.PositionsUpdate.StandardNotification.Systems
{
	public class StandardPositionsChangedNotifySystem : IEcsPreInitSystem, IEcsRunSystem
	{
		private EcsPool<BlockPositionChangedComponent> _blockPositionChangedPool;
		private EcsPool<GridPositionChangedComponent> _gridPositionChangedPool;
		private EcsPool<StandardBlockPositionChangedComponent> _standardBlockPositionChangedPool;
		private EcsPool<StandardGridPositionChangedComponent> _standardGridPositionChangedPool;
		private EcsFilter _positionsChangedBlockPositionFilter;
		private EcsFilter _positionsChangedGridPositionFilter;

		public void PreInit(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();
			_blockPositionChangedPool = world.GetPool<BlockPositionChangedComponent>();
			_gridPositionChangedPool = world.GetPool<GridPositionChangedComponent>();
			_standardBlockPositionChangedPool = world.GetPool<StandardBlockPositionChangedComponent>();
			_standardGridPositionChangedPool = world.GetPool<StandardGridPositionChangedComponent>();
			// Exc если прошлое уведомление не было обработано, то просто пропускаем
			// т.к. Standard system помнит только самое первое изменение, а не последующие
			_positionsChangedBlockPositionFilter = world
				.Filter<ObjectPhysicsComponent>()
				.Inc<BlockPositionChangedComponent>()
				.Exc<StandardBlockPositionChangedComponent>()
				.End();
			_positionsChangedGridPositionFilter = world
				.Filter<ObjectPhysicsComponent>()
				.Inc<GridPositionChangedComponent>()
				.Exc<StandardGridPositionChangedComponent>()
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
				ref var StandardBlockPositionChanged = 
					ref _standardBlockPositionChangedPool.Add(objectPhysicsEntity);
				StandardBlockPositionChanged.PreviousBlockPosition = blockPositionChanged.PreviousBlockPosition;
			}
		}

		private void NotifyGridPositionsChanged()
		{
			foreach(var objectPhysicsEntity in _positionsChangedGridPositionFilter)
			{
				var gridPositionChanged =
					_gridPositionChangedPool.Get(objectPhysicsEntity);
				ref var StandardGridPositionChanged = ref _standardGridPositionChangedPool.Add(objectPhysicsEntity);
				StandardGridPositionChanged.PreviousGridPosition = gridPositionChanged.PreviousGridPosition;
			}
		}
	}
}
