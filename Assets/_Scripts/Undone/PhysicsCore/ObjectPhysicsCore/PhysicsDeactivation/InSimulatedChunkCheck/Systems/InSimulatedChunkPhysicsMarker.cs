using System;
using _Scripts.Apart.Extensions.Ecs;
using _Scripts.Core.ChunkCore.ChunkLogic.Components;
using _Scripts.Core.ChunkCore.ChunksContainerLogic.Components;
using _Scripts.Core.ChunkCore.ChunksContainerLogic.Components.Elements;
using _Scripts.Core.PhysicsCore.ChunkPhysicsCore.ChunkPhysicsLogic.Components;
using _Scripts.Undone.PhysicsCore.ObjectPhysicsCore.Components;
using _Scripts.Undone.PhysicsCore.ObjectPhysicsCore.PhysicsDeactivation.InSimulatedChunkCheck.Components;
using _Scripts.Undone.PhysicsCore.ObjectPhysicsCore.PositionsUpdate.Components;
using _Scripts.Undone.PhysicsCore.ObjectPhysicsCore.PositionsUpdate.Components.Elements;
using Leopotam.EcsLite;

namespace _Scripts.Undone.PhysicsCore.ObjectPhysicsCore.PhysicsDeactivation.InSimulatedChunkCheck.Systems
{
	public class InSimulatedChunkPhysicsMarker : IEcsPreInitSystem, IEcsInitSystem, IEcsRunSystem
	{
		private ChunksContainer _chunksContainer;
		private ObjectPhysicsPositionsContainer _objectPhysicsPositionsContainer;

		private EcsPool<ChunkComponent> _chunkPool;
		private EcsPool<ObjectPhysicsPositionsComponent> _objectPhysicsPositionsPool;
		private EcsPool<InSimulatedChunkPhysicsTag> _inSimulatedChunkPhysicsPool;
		private EcsPool<ChunkPhysicsSimulatedTag> _chunkPhysicsSimulatedPool;
		private EcsFilter _objectPhysicsChangedGridPositionFilter;
		private EcsFilter _chunksPhysicsBecomeSimulatedFilter;

		public void PreInit(IEcsSystems systems)
		{
			var world = systems.GetWorld();
			_chunkPool = world.GetPool<ChunkComponent>();
			_objectPhysicsPositionsPool = world.GetPool<ObjectPhysicsPositionsComponent>();
			_inSimulatedChunkPhysicsPool = world.GetPool<InSimulatedChunkPhysicsTag>();
			_chunkPhysicsSimulatedPool = world.GetPool<ChunkPhysicsSimulatedTag>();
			_objectPhysicsChangedGridPositionFilter = world
				.Filter<ObjectPhysicsComponent>()
				.Inc<GridPositionChangedComponent>()
				.End();
			_chunksPhysicsBecomeSimulatedFilter = world
				.Filter<ChunkPhysicsComponent>()
				.Inc<ChunkPhysicsBecomeSimulatedTag>()
				.Inc<ChunkPhysicsSimulatedTag>()
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

			foreach(var chunkEntity in _chunksPhysicsBecomeSimulatedFilter)
			{
				var gridPosition = _chunkPool.Get(chunkEntity).GridPosition;
				if(_objectPhysicsPositionsContainer.TryGetEntitiesByGridPosition(gridPosition, 
					out var objectPhysicsEntities))
				{
					foreach(var objectPhysicsEntity in objectPhysicsEntities)
					{
						_inSimulatedChunkPhysicsPool.AddIfNotHas(objectPhysicsEntity);
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

			throw new Exception($"{nameof(ChunksContainerComponent)} not found");
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

			throw new Exception($"{nameof(ObjectPhysicsPositionsContainerComponent)} not found");
		}
	}
}
