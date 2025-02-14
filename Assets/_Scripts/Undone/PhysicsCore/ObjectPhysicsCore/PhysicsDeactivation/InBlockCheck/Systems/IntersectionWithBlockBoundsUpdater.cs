using System;
using _Scripts.Apart.Extensions.Ecs;
using _Scripts.Core.Extensions;
using _Scripts.Undone.PhysicsCore.ObjectPhysicsCore.Components;
using _Scripts.Undone.PhysicsCore.ObjectPhysicsCore.PhysicsDeactivation.InBlockCheck.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Scripts.Undone.PhysicsCore.ObjectPhysicsCore.PhysicsDeactivation.InBlockCheck.Systems
{
	public class IntersectionWithBlockBoundsUpdater : IEcsPreInitSystem, IEcsInitSystem, IEcsRunSystem
	{
		private EntitiesBoundsContainer _intersectionWithBlockBoundsContainer;

		private EcsPool<ObjectPhysicsComponent> _objectPhysicsPool;
		private EcsPool<IntersectionWithBlockBoundsComponent> _intersectionWithBlockBoundsPool;
		private EcsPool<CheckIsObjectPhysicsInBlockTag> _checkIsObjectPhysicsInBlockPool;
		private EcsFilter _uninitializedIntersectionWithBlockBoundsFilter;
		private EcsFilter _intersectionWithBlockBoundsFilter;

		public void PreInit(IEcsSystems systems)
		{
			var world = systems.GetWorld();
			_objectPhysicsPool = world.GetPool<ObjectPhysicsComponent>();
			_intersectionWithBlockBoundsPool = world.GetPool<IntersectionWithBlockBoundsComponent>();
			_checkIsObjectPhysicsInBlockPool = world.GetPool<CheckIsObjectPhysicsInBlockTag>();
			_uninitializedIntersectionWithBlockBoundsFilter = world
				.Filter<ObjectPhysicsComponent>()
				.Exc<IntersectionWithBlockBoundsComponent>()
				.End();
			_intersectionWithBlockBoundsFilter = world
				.Filter<ObjectPhysicsComponent>()
				.Inc<IntersectionWithBlockBoundsComponent>()
				.End();
		}

		public void Init(IEcsSystems systems)
		{
			_intersectionWithBlockBoundsContainer = GetIntersectionWithBlockBoundsContainer(systems.GetWorld());
		}

		public void Run(IEcsSystems systems)
		{
			foreach(var objectPhysicsEntity in _uninitializedIntersectionWithBlockBoundsFilter)
			{
				Initialize(objectPhysicsEntity);
			}

			foreach(var objectPhysicsEntity in _intersectionWithBlockBoundsFilter)
			{
				UpdateBounds(objectPhysicsEntity);
			}
		}

		private void Initialize(int objectPhysicsEntity)
		{
			var fullSizeCollider = _objectPhysicsPool.Get(objectPhysicsEntity).FullSizeCollider;
			ref var intersectionWithBlockBounds = ref _intersectionWithBlockBoundsPool.Add(objectPhysicsEntity);
			var inBlockBounds = ConvertToBlockBounds(fullSizeCollider.bounds);
			intersectionWithBlockBounds.BlockBounds = inBlockBounds;
			intersectionWithBlockBounds.RoundedBlockBounds = inBlockBounds.Round();
			_intersectionWithBlockBoundsContainer.Add(intersectionWithBlockBounds.RoundedBlockBounds, 
				objectPhysicsEntity);
			_checkIsObjectPhysicsInBlockPool.Add(objectPhysicsEntity);
		}

		private void UpdateBounds(int objectPhysicsEntity)
		{
			var fullSizeCollider = _objectPhysicsPool.Get(objectPhysicsEntity).FullSizeCollider;
			ref var intersectionWithBlockBounds = ref _intersectionWithBlockBoundsPool.Get(objectPhysicsEntity);
			var inBlockBounds = intersectionWithBlockBounds.BlockBounds;
			inBlockBounds.center = fullSizeCollider.bounds.center;
			var roundedBlockBoundsAtPreviousFrame = intersectionWithBlockBounds.RoundedBlockBounds;
			var roundedBlockBoundsAtCurrentFrame = inBlockBounds.Round();
			if(roundedBlockBoundsAtPreviousFrame == roundedBlockBoundsAtCurrentFrame)
			{
				return;
			}

			intersectionWithBlockBounds.BlockBounds = inBlockBounds;
			intersectionWithBlockBounds.RoundedBlockBounds = roundedBlockBoundsAtCurrentFrame;
			_intersectionWithBlockBoundsContainer.Add(roundedBlockBoundsAtCurrentFrame, objectPhysicsEntity);
			_intersectionWithBlockBoundsContainer.Remove(roundedBlockBoundsAtPreviousFrame, objectPhysicsEntity);
			_checkIsObjectPhysicsInBlockPool.AddIfNotHas(objectPhysicsEntity);
		}

		private Bounds ConvertToBlockBounds(Bounds bounds)
		{
			// Если размер больше 2х, то отнимаем от проверки один метр(один блок), чтобы
			// проверять только по 0.5 с каждой стороны по оси. В противном случае проверяем с каждой стороны по
			// size * 0.5
			var size = bounds.size;
			size.x = size.x > 2 ? size.x - 1 : size.x / 2;
			size.y = size.y > 2 ? size.y - 1 : size.y / 2;
			size.z = size.z > 2 ? size.z - 1 : size.z / 2;
			bounds.size = size;
			return bounds;
		}

		private EntitiesBoundsContainer GetIntersectionWithBlockBoundsContainer(EcsWorld world)
		{
			var pool =
				world.GetPool<IntersectionWithBlockBoundsContainerComponent>();
			var filter = world
				.Filter<IntersectionWithBlockBoundsContainerComponent>()
				.End();
			foreach(var entity in filter)
			{
				return pool.Get(entity).Container;
			}

			throw new Exception($"{nameof(IntersectionWithBlockBoundsContainerComponent)} not found");
		}
	}
}
