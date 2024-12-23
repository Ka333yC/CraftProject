using System;
using Assets.Scripts.Core.ChunkCore.LifeTimeControl.Components;
using Assets.Scripts.Core.PhysicsCore.ChunkPhysicsCore.MeshPartsContainerUpdating.Components;
using Assets.Scripts.Core.PhysicsCore.ChunkPhysicsCore.MeshPartsContainerUpdating.WallUpdating.Components;
using Assets.Scripts.Core.PhysicsCore.ChunkPhysicsCore.MeshPartsContainerUpdating.WallUpdating.Mark.Components;
using ChunkCore;
using ChunkCore.ChunksContainerScripts;
using ChunkCore.ChunksContainerScripts.Components;
using Extensions.Ecs;
using PhysicsCore.ChunkPhysicsCore.LifeTimeControl.Components;
using Leopotam.EcsLite;
using PhysicsCore.ObjectPhysics.Components;
using UnityEngine;

namespace Assets.Scripts.Core.PhysicsCore.ChunkPhysicsCore.MeshPartsContainerUpdating.WallUpdating.Mark.Systems
{
	public class MarkBehindWallsDirtySystem : IEcsPreInitSystem, IEcsInitSystem, IEcsRunSystem
	{
		private ChunksContainer _chunksContainer;

		private EcsPool<ChunkComponent> _chunkPool;
		private EcsPool<ChunkPhysicsComponent> _chunkPhysicsPool;
		private EcsPool<MeshPartsContainerInitializedTag> _meshPartsContainerInitializedPool;
		private EcsPool<MeshPartsContainerInitializingTag> _meshPartsContainerInitializingPool;
		private EcsPool<DirtyWallsComponent> _dirtyWallsPool;
		private EcsPool<BehindWallsMarkedDirtyTag> _behindWallsMarkedDirtyPool;
		private EcsFilter _chunksToMarkDirtyWallsFilter;

		public void PreInit(IEcsSystems systems)
		{
			var world = systems.GetWorld();
			_chunkPool = world.GetPool<ChunkComponent>();
			_chunkPhysicsPool = world.GetPool<ChunkPhysicsComponent>();
			_meshPartsContainerInitializedPool = world.GetPool<MeshPartsContainerInitializedTag>();
			_meshPartsContainerInitializingPool = world.GetPool<MeshPartsContainerInitializingTag>();
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
				!(_meshPartsContainerInitializedPool.Has(chunkEntity) ||
				_meshPartsContainerInitializingPool.Has(chunkEntity)))
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
