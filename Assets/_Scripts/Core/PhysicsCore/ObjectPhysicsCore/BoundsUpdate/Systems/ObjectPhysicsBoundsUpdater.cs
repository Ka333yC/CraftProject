using _Scripts.Core.Extensions;
using _Scripts.Core.PhysicsCore.ObjectPhysicsCore.BoundsUpdate.Components;
using _Scripts.Core.PhysicsCore.ObjectPhysicsCore.Components;
using Leopotam.EcsLite;

namespace _Scripts.Core.PhysicsCore.ObjectPhysicsCore.BoundsUpdate.Systems
{
	public class ObjectPhysicsBoundsUpdater : IEcsPreInitSystem, IEcsRunSystem
	{
		private EcsPool<ObjectPhysicsComponent> _objectPhysicsPool;
		private EcsPool<ObjectPhysicsBoundsComponent> _objectPhysicsBoundsPool;
		private EcsPool<BoundsChangedComponent> _boundsChangedPool;
		private EcsFilter _uninitializedObjectPhysicsBoundsFilter;
		private EcsFilter _objectPhysicsBoundsFilter;

		public void PreInit(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();
			_objectPhysicsPool = world.GetPool<ObjectPhysicsComponent>();
			_objectPhysicsBoundsPool = world.GetPool<ObjectPhysicsBoundsComponent>();
			_boundsChangedPool = world.GetPool<BoundsChangedComponent>();
			_uninitializedObjectPhysicsBoundsFilter = world
				.Filter<ObjectPhysicsComponent>()
				.Exc<ObjectPhysicsBoundsComponent>()
				.End();
			_objectPhysicsBoundsFilter = world
				.Filter<ObjectPhysicsComponent>()
				.Inc<ObjectPhysicsBoundsComponent>()
				.End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach(var objectPhysicsEntity in _uninitializedObjectPhysicsBoundsFilter)
			{
				Initialize(objectPhysicsEntity);
			}

			foreach(var objectPhysicsEntity in _objectPhysicsBoundsFilter)
			{
				Update(objectPhysicsEntity);
			}
		}

		private void Initialize(int objectPhysicsEntity)
		{
			ref var objectPhysics = ref _objectPhysicsPool.Get(objectPhysicsEntity);
			ref var objectPhysicsBounds = ref _objectPhysicsBoundsPool.Add(objectPhysicsEntity);
			objectPhysicsBounds.Bounds = objectPhysics.FullSizeCollider.bounds.Round();
		}

		private void Update(int objectPhysicsEntity)
		{
			ref var objectPhysics = ref _objectPhysicsPool.Get(objectPhysicsEntity);
			ref var objectPhysicsBounds = ref _objectPhysicsBoundsPool.Get(objectPhysicsEntity);
			var boundsAtPreviousFrame = objectPhysicsBounds.Bounds;
			var currentBounds = objectPhysics.FullSizeCollider.bounds.Round();
			if(boundsAtPreviousFrame == currentBounds)
			{
				return;
			}

			objectPhysicsBounds.Bounds = currentBounds;
			ref var boundsChanged = ref _boundsChangedPool.Add(objectPhysicsEntity);
			boundsChanged.PreviousBounds = boundsAtPreviousFrame;
		}
	}
}
