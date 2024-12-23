using Leopotam.EcsLite;
using ChunkCore;
using PhysicsCore.ObjectPhysics.Components;
using PhysicsCore.ObjectPhysics.PositionUpdater.Components;
using Assets.Scripts.Implementation.ObjectPhysics.PositionsUpdate.Components;
using UnityEngine;
using Assets.Scripts.Core.ObjectPhysicsCore.PositionsUpdate;
using Assets.Scripts.Implementation.ObjectPhysics.InBlockCheck2.Components;
using Assets.Scripts.Implementation.ObjectPhysics.InBlockCheck2;
using System;
using Assets.Scripts.Core.ObjectPhysicsCore.PositionsUpdate.Components;
using UnityEngine.UIElements;

namespace PhysicsCore.ObjectPhysics.PositionUpdater.Systems
{
	public class ObjectPhysicsPositionsUpdater : IEcsPreInitSystem, IEcsInitSystem, IEcsRunSystem
	{
		private ObjectPhysicsPositionsContainer _positionsContainer;

		private EcsPool<ObjectPhysicsComponent> _objectPhysicsPool;
		private EcsPool<ObjectPhysicsPositionsComponent> _objectPhysicsPositionsPool;
		private EcsPool<BlockPositionChangedComponent> _blockPositionChangedPool;
		private EcsPool<GridPositionChangedComponent> _gridPositionChangedPool;
		private EcsFilter _uninitializedObjectPhysicsPositionsFilter;
		private EcsFilter _objectPhysicsPositionsFilter;

		public void PreInit(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();
			_objectPhysicsPool = world.GetPool<ObjectPhysicsComponent>();
			_objectPhysicsPositionsPool = world.GetPool<ObjectPhysicsPositionsComponent>();
			_blockPositionChangedPool = world.GetPool<BlockPositionChangedComponent>();
			_gridPositionChangedPool = world.GetPool<GridPositionChangedComponent>();
			_uninitializedObjectPhysicsPositionsFilter = world
				.Filter<ObjectPhysicsComponent>()
				.Exc<ObjectPhysicsPositionsComponent>()
				.End();
			_objectPhysicsPositionsFilter = world
				.Filter<ObjectPhysicsComponent>()
				.Inc<ObjectPhysicsPositionsComponent>()
				.End();
		}

		public void Init(IEcsSystems systems)
		{
			_positionsContainer = GetObjectPhysicsPositionsContainer(systems.GetWorld());
		}

		public void Run(IEcsSystems systems)
		{
			foreach(var objectPhysicsEntity in _uninitializedObjectPhysicsPositionsFilter)
			{
				Initialize(objectPhysicsEntity);
			}

			foreach(var objectPhysicsEntity in _objectPhysicsPositionsFilter)
			{
				Update(objectPhysicsEntity);
			}
		}

		private void Initialize(int objectPhysicsEntity)
		{
			ref var objectPhysics = ref _objectPhysicsPool.Get(objectPhysicsEntity);
			ref var objectPhysicsPositions = ref _objectPhysicsPositionsPool.Add(objectPhysicsEntity);
			var blockPosition =
				ChunkConstantData.WorldToBlockWorldPosition(objectPhysics.Rigidbody.position);
			var gridPosition = ChunkConstantData.WorldToGridPosition(blockPosition);
			objectPhysicsPositions.BlockPosition = blockPosition;
			objectPhysicsPositions.GridPosition = gridPosition;
			_positionsContainer.AddBlockPosition(blockPosition, objectPhysicsEntity);
			_positionsContainer.AddGridPosition(gridPosition, objectPhysicsEntity);
		}

		private void Update(int objectPhysicsEntity)
		{
			ref var objectPhysicsPositions = ref _objectPhysicsPositionsPool.Get(objectPhysicsEntity);
			var rigidbody = _objectPhysicsPool.Get(objectPhysicsEntity).Rigidbody;
			var isNewBlockPosition =
				UpdateBlockPosition(objectPhysicsEntity, rigidbody, ref objectPhysicsPositions);
			if(!isNewBlockPosition)
			{
				return;
			}

			UpdateGridPosition(objectPhysicsEntity, ref objectPhysicsPositions);
		}

		/// <returns>Is position changed</returns>
		private bool UpdateBlockPosition(int objectPhysicsEntity, Rigidbody rigidbody,
			ref ObjectPhysicsPositionsComponent objectPhysicsPositions)
		{
			var blockPositionAtPreviousFrame = objectPhysicsPositions.BlockPosition;
			var currentBlockPosition =
				ChunkConstantData.WorldToBlockWorldPosition(rigidbody.position);
			if(currentBlockPosition == blockPositionAtPreviousFrame)
			{
				return false;
			}

			objectPhysicsPositions.BlockPosition = currentBlockPosition;
			ref var blockPositionChanged = ref _blockPositionChangedPool.Add(objectPhysicsEntity);
			blockPositionChanged.PreviousBlockPosition = blockPositionAtPreviousFrame;
			_positionsContainer.AddBlockPosition(currentBlockPosition, objectPhysicsEntity);
			_positionsContainer.RemoveBlockPosition(blockPositionAtPreviousFrame, objectPhysicsEntity);
			return true;
		}

		private void UpdateGridPosition(int objectPhysicsEntity, 
			ref ObjectPhysicsPositionsComponent objectPhysicsPositions)
		{
			var gridPositionAtPreviousFrame = objectPhysicsPositions.GridPosition;
			var currentGridPosition =
				ChunkConstantData.WorldToGridPosition(objectPhysicsPositions.BlockPosition);
			if(currentGridPosition == gridPositionAtPreviousFrame)
			{
				return;
			}

			objectPhysicsPositions.GridPosition = currentGridPosition;
			ref var gridPositionChanged = ref _gridPositionChangedPool.Add(objectPhysicsEntity);
			gridPositionChanged.PreviousGridPosition = gridPositionAtPreviousFrame;
			_positionsContainer.AddGridPosition(currentGridPosition, objectPhysicsEntity);
			_positionsContainer.RemoveGridPosition(gridPositionAtPreviousFrame, objectPhysicsEntity);
		}

		private ObjectPhysicsPositionsContainer GetObjectPhysicsPositionsContainer(EcsWorld world)
		{
			var pool =
				world.GetPool<ObjectPhysicsPositionsContainerComponent>();
			var filter = world
				.Filter<ObjectPhysicsPositionsContainerComponent>()
				.End();
			foreach(var entity in filter)
			{
				return pool.Get(entity).Container;
			}

			throw new Exception($"{typeof(ObjectPhysicsPositionsContainerComponent).Name} not found");
		}
	}
}
