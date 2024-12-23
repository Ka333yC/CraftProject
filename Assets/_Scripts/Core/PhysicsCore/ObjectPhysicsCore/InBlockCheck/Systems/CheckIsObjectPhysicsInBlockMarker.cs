using ChunkCore.OnBlockChanged.FixedNotification.Components;
using ChunkCore;
using Extensions.Ecs;
using Leopotam.EcsLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Core.ChunkCore.LifeTimeControl.Components;
using Assets.Scripts.Implementation.ObjectPhysics.InBlockCheck2.Components;
using ChunkCore.Loading.Components;

namespace Assets.Scripts.Implementation.ObjectPhysics.InBlockCheck2.Systems
{
	public class CheckIsObjectPhysicsInBlockMarker : IEcsPreInitSystem, IEcsInitSystem, IEcsRunSystem
	{
		private EntitiesBoundsContainer _intersectionWithBlockBoundsContainer;

		private EcsPool<ChunkComponent> _chunkPool;
		private EcsPool<FixedBlocksChangedComponent> _fixedBlocksChangedPool;
		private EcsPool<CheckIsObjectPhysicsInBlockTag> _checkIsObjectPhysicsInBlockPool;
		private EcsFilter _chunksWithChangedBlocksFilter;
		private EcsFilter _initializedChunksFilter;

		public void PreInit(IEcsSystems systems)
		{
			var world = systems.GetWorld();
			_chunkPool = world.GetPool<ChunkComponent>();
			_fixedBlocksChangedPool = world.GetPool<FixedBlocksChangedComponent>();
			_checkIsObjectPhysicsInBlockPool = world.GetPool<CheckIsObjectPhysicsInBlockTag>();
			_chunksWithChangedBlocksFilter = world
				.Filter<ChunkComponent>()
				.Inc<FixedBlocksChangedComponent>()
				.End();
			_initializedChunksFilter = world
				.Filter<ChunkComponent>()
				.Inc<FixedChunkInitializedNotificationTag>()
				.End();
		}

		public void Init(IEcsSystems systems)
		{
			_intersectionWithBlockBoundsContainer = GetIntersectionWithBlockBoundsContainer(systems.GetWorld());
		}

		public void Run(IEcsSystems systems)
		{
			MarkOnBlockChanged();
			MarkOnChunkInitialized();
		}

		private void MarkOnBlockChanged()
		{
			foreach(var chunkEntity in _chunksWithChangedBlocksFilter)
			{
				var chunkPosition = _chunkPool.Get(chunkEntity).GridPosition;
				var worldChunkPosition = ChunkConstantData.GridToWorldPosition(chunkPosition);
				var changedBlocks =
					_fixedBlocksChangedPool.Get(chunkEntity).ChangedBlocksPositions;
				foreach(var blockPosition in changedBlocks)
				{
					var worldBlockPosition = worldChunkPosition + blockPosition;
					if(_intersectionWithBlockBoundsContainer.TryGetEntitiesByBlockPosition(worldBlockPosition,
						out var objectPhysicsEntities))
					{
						for(int i = 0; i < objectPhysicsEntities.Count; i++)
						{
							_checkIsObjectPhysicsInBlockPool.AddIfNotHas(objectPhysicsEntities[i]);
						}
					}
				}
			}
		}

		private void MarkOnChunkInitialized()
		{
			foreach(var chunkEntity in _initializedChunksFilter)
			{
				var chunkPosition = _chunkPool.Get(chunkEntity).GridPosition;
				var worldChunkPosition = ChunkConstantData.GridToWorldPosition(chunkPosition);
				var changedBlocks =
					_fixedBlocksChangedPool.Get(chunkEntity).ChangedBlocksPositions;
				foreach(var blockPosition in changedBlocks)
				{
					var worldBlockPosition = worldChunkPosition + blockPosition;
					if(_intersectionWithBlockBoundsContainer.TryGetEntitiesByBlockPosition(worldBlockPosition,
						out var objectPhysicsEntities))
					{
						for(int i = 0; i < objectPhysicsEntities.Count; i++)
						{
							_checkIsObjectPhysicsInBlockPool.AddIfNotHas(objectPhysicsEntities[i]);
						}
					}
				}
			}
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

			throw new Exception($"{typeof(IntersectionWithBlockBoundsContainerComponent).Name} not found");
		}
	}
}
