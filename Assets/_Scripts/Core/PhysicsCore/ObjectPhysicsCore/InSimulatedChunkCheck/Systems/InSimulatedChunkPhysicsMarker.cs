using System;
using ChunkCore.ChunksContainerScripts.Components;
using ChunkCore.ChunksContainerScripts;
using Leopotam.EcsLite;
using PhysicsCore.ObjectPhysics.Components;
using PhysicsCore.ObjectPhysics.PositionUpdater.Components;
using Assets.Scripts.Core.PhysicsCore.ObjectPhysics.PhysicsDeactivating;
using UnityEngine;
using Assets.Scripts.Core.PhysicsCore.ChunkPhysicsCore.MeshPartsContainerUpdating.Components;
using Assets.Scripts.Implementation.ObjectPhysics.PositionsUpdate.Components;
using Assets.Scripts.Core.PhysicsCore.ObjectPhysics.PhysicsDeactivating.Components;
using Extensions.Ecs;
using PhysicsCore.ChunkPhysicsCore.LifeTimeControl.Components;
using Assets.Scripts.Core.ObjectPhysicsCore.InSimulatedChunkCheck.Components;
using Assets.Scripts.Core.ChunkCore.LifeTimeControl.Components;
using Assets.Scripts.Core.ObjectPhysicsCore.PositionsUpdate;
using Assets.Scripts.Core.ObjectPhysicsCore.PositionsUpdate.Components;

namespace Assets.Scripts.Core.PhysicsCore.ObjectPhysics.Systems
{
	public class InSimulatedChunkPhysicsMarker : IEcsPreInitSystem, IEcsInitSystem, IEcsRunSystem
	{
		private ChunksContainer _chunksContainer;
		private ObjectPhysicsPositionsContainer _objectPhysicsPositionsContainer;

		private EcsPool<ChunkComponent> _chunkPool;
		private EcsPool<ObjectPhysicsComponent> _objectPhysicsPool;
		private EcsPool<ObjectPhysicsPositionsComponent> _objectPhysicsPositionsPool;
		private EcsPool<InSimulatedChunkPhysicsTag> _inSimulatedChunkPhysicsPool;
		private EcsPool<ChunkPhysicsSimulatedTag> _chunkPhysicsSimulatedPool;
		private EcsFilter _objectPhysicsChangedGridPositionFilter;
		private EcsFilter _chunkPhysicsBecomeSimulatedFilter;

		public void PreInit(IEcsSystems systems)
		{
			var world = systems.GetWorld();
			_chunkPool = world.GetPool<ChunkComponent>();
			_objectPhysicsPool = world.GetPool<ObjectPhysicsComponent>();
			_objectPhysicsPositionsPool = world.GetPool<ObjectPhysicsPositionsComponent>();
			_inSimulatedChunkPhysicsPool = world.GetPool<InSimulatedChunkPhysicsTag>();
			_chunkPhysicsSimulatedPool = world.GetPool<ChunkPhysicsSimulatedTag>();
			_objectPhysicsChangedGridPositionFilter = world
				.Filter<ObjectPhysicsComponent>()
				.Inc<GridPositionChangedComponent>()
				.End();
			_chunkPhysicsBecomeSimulatedFilter = world
				.Filter<ChunkPhysicsComponent>()
				.Inc<ChunkPhysicsBecomeSimulatedTag>()
				.Inc<ChunkComponent>()
				.End();
		}

		public void Init(IEcsSystems systems)
		{
			var world = systems.GetWorld();
			_chunksContainer = GetChunksContainer(world);
			_objectPhysicsPositionsContainer = GetObjectPhysicsPositionsContainer(world);
		}

		public void Run(IEcsSystems systems)
		{
			foreach(var objectPhysicsEntity in _objectPhysicsChangedGridPositionFilter)
			{
				var objectPhysicsPositions = 
					_objectPhysicsPositionsPool.Get(objectPhysicsEntity);
				if(_chunksContainer.TryGetChunk(objectPhysicsPositions.GridPosition, out int chunkEntity) &&
					_chunkPhysicsSimulatedPool.Has(chunkEntity))
				{
					_inSimulatedChunkPhysicsPool.AddIfNotHas(objectPhysicsEntity);
				}
				else
				{
					_inSimulatedChunkPhysicsPool.DelIfHas(objectPhysicsEntity);
				}
			}

			foreach(var chunkEntity in _chunkPhysicsBecomeSimulatedFilter)
			{
				var gridPosition = _chunkPool.Get(chunkEntity).GridPosition;
				if(_objectPhysicsPositionsContainer.TryGetEntitiesByGridPosition(gridPosition, 
					out var objectPhysicsEntities))
				{
					foreach(var objectPhysicsEntity in objectPhysicsEntities)
					{
						_inSimulatedChunkPhysicsPool.Add(objectPhysicsEntity);
					}
				}
			}
		}

		private ChunksContainer GetChunksContainer(EcsWorld world)
		{
			var pool = world.GetPool<ChunksContainerComponent>();
			var filter = world
				.Filter<ChunksContainerComponent>()
				.End();
			foreach(var entity in filter)
			{
				return pool.Get(entity).ChunksContainer;
			}

			throw new Exception($"{typeof(ChunksContainerComponent).Name} not found");
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
