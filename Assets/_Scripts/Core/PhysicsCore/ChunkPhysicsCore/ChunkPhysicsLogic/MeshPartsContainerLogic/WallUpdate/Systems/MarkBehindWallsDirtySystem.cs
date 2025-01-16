using System;
using _Scripts.Apart.Extensions.Ecs;
using _Scripts.Core.ChunkCore.ChunkLogic.Components;
using _Scripts.Core.ChunkCore.ChunksContainerLogic.Components;
using _Scripts.Core.ChunkCore.ChunksContainerLogic.Components.Elements;
using _Scripts.Core.PhysicsCore.ChunkPhysicsCore.ChunkPhysicsLogic.Components;
using _Scripts.Core.PhysicsCore.ChunkPhysicsCore.ChunkPhysicsLogic.MeshPartsContainerLogic.Components;
using _Scripts.Core.PhysicsCore.ChunkPhysicsCore.ChunkPhysicsLogic.MeshPartsContainerLogic.WallUpdate.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Scripts.Core.PhysicsCore.ChunkPhysicsCore.ChunkPhysicsLogic.MeshPartsContainerLogic.WallUpdate.Systems
{
	public class MarkBehindWallsDirtySystem : IEcsPreInitSystem, IEcsInitSystem, IEcsRunSystem
	{
		private ChunksContainer _chunksContainer;

		private EcsPool<ChunkComponent> _chunkPool;
		private EcsPool<ChunkPhysicsComponent> _chunkPhysicsPool;
		private EcsPool<MeshPartsContainerInitializedTag> _initializedPool;
		private EcsPool<MeshPartsContainerInitializingTag> _initializingPool;
		private EcsPool<DirtyWallsComponent> _dirtyWallsPool;
		private EcsPool<BehindWallsMarkedDirtyTag> _behindWallsMarkedDirtyPool;
		private EcsFilter _chunksToMarkDirtyWallsFilter;

		public void PreInit(IEcsSystems systems)
		{
			var world = systems.GetWorld();
			_chunkPool = world.GetPool<ChunkComponent>();
			_chunkPhysicsPool = world.GetPool<ChunkPhysicsComponent>();
			_initializedPool = world.GetPool<MeshPartsContainerInitializedTag>();
			_initializingPool = world.GetPool<MeshPartsContainerInitializingTag>();
			_dirtyWallsPool = world.GetPool<DirtyWallsComponent>();
			_behindWallsMarkedDirtyPool = world.GetPool<BehindWallsMarkedDirtyTag>();
			_chunksToMarkDirtyWallsFilter = world
				.Filter<ChunkComponent>()
				.Inc<ChunkPhysicsComponent>()
				.Exc<BehindWallsMarkedDirtyTag>()
				.End();
		}

		public void Init(IEcsSystems systems)
		{
			_chunksContainer = GetChunksContainer(systems.GetWorld());
		}

		public void Run(IEcsSystems systems)
		{
			foreach(var chunkEntity in _chunksToMarkDirtyWallsFilter)
			{
				ref var chunk = ref _chunkPool.Get(chunkEntity);
				MarkBehindChunksDirtyWalls(chunk.GridPosition);
				_behindWallsMarkedDirtyPool.Add(chunkEntity);
			}
		}

		private void MarkBehindChunksDirtyWalls(Vector3Int gridPosition)
		{
			var wall = Face.Forward;
			MarkDirtyWalls(gridPosition + wall.ToVector(), wall.Reverse());
			wall = Face.Back;
			MarkDirtyWalls(gridPosition + wall.ToVector(), wall.Reverse());
			wall = Face.Right;
			MarkDirtyWalls(gridPosition + wall.ToVector(), wall.Reverse());
			wall = Face.Left;
			MarkDirtyWalls(gridPosition + wall.ToVector(), wall.Reverse());
		}

		private void MarkDirtyWalls(Vector3Int gridPosition, Face dirtyWall)
		{
			if(!_chunksContainer.TryGetChunk(gridPosition, out int chunkEntity) ||
				!_chunkPhysicsPool.Has(chunkEntity) ||
				!(_initializedPool.Has(chunkEntity) ||
					_initializingPool.Has(chunkEntity)))
			{
				return;
			}

			ref var dirtyWalls = ref _dirtyWallsPool.AddOrGet(chunkEntity);
			dirtyWalls.Walls.AddFace(dirtyWall);
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
	}
}
