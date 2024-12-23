using System;
using Assets.Scripts.Core.ChunkCore.LifeTimeControl.Components;
using Assets.Scripts.Core.PhysicsCore.ChunkPhysicsCore.MeshPartsContainerUpdating.Components;
using Cysharp.Threading.Tasks;
using PhysicsCore.ChunkPhysicsCore.LifeTimeControl.Components;
using Leopotam.EcsLite;
using PhysicsCore.ObjectPhysics.Components;
using System.Collections.Generic;
using UnityEngine;
using Extensions;
using System.Linq;
using Assets.Scripts.Core.PhysicsCore.ChunkPhysicsCore.MeshPartsContainerUpdating.WallUpdating.Components;
using ChunkCore;
using System.Threading;
using Extensions.Ecs;
using Assets.Scripts.Core.PhysicsCore.ChunkPhysicsCore.MeshUpdating.Components;
using Assets.Scripts.Apart.Extensions.Ecs;
using ChunkCore.ChunksContainerScripts.Components;
using ChunkCore.ChunksContainerScripts;
using Assets.Scripts.Core.ChunkPhysicsCore.MeshPartsContainerUpdating.WallUpdating.Components;

namespace Assets.Scripts.Core.PhysicsCore.ChunkPhysicsCore.MeshPartsContainerUpdating.WallUpdating.Systems
{
	public class MeshPartsContainerWallUpdater : IEcsPreInitSystem, IEcsInitSystem, IEcsRunSystem
	{
		private ChunksContainer _chunksContainer;

		private EcsWorld _world;
		private EcsPool<ChunkComponent> _chunkPool;
		private EcsPool<ChunkPhysicsComponent> _chunkPhysicsPool;
		private EcsPool<DirtyWallsComponent> _dirtyWallsPool;
		private EcsPool<MeshPartsContainerWallGeneratingTag> _generatingPool;
		private EcsPool<ChunkPhysicsDirtyMeshTag> _dirtyMeshPool;
		private EcsFilter _chunkToUpdateWallFilter;
		private EcsFilter _generatingFilter;

		public void PreInit(IEcsSystems systems)
		{
			_world = systems.GetWorld();
			_chunkPool = _world.GetPool<ChunkComponent>();
			_chunkPhysicsPool = _world.GetPool<ChunkPhysicsComponent>();
			_dirtyWallsPool = _world.GetPool<DirtyWallsComponent>();
			_generatingPool = _world.GetPool<MeshPartsContainerWallGeneratingTag>();
			_dirtyMeshPool = _world.GetPool<ChunkPhysicsDirtyMeshTag>();
			_chunkToUpdateWallFilter = _world
				.Filter<DirtyWallsComponent>()
				.Inc<ChunkComponent>()
				.Inc<ChunkPhysicsComponent>()
				.Inc<MeshPartsContainerInitializedTag>()
				.Exc<MeshPartsContainerWallGeneratingTag>()
				.End();
			_generatingFilter = _world
				.Filter<MeshPartsContainerWallGeneratingTag>()
				.End();
		}

		public void Init(IEcsSystems systems)
		{
			_chunksContainer = GetChunksContainer(systems.GetWorld());
		}

		public void Run(IEcsSystems systems)
		{
			if(_generatingFilter.Any() || !_chunkToUpdateWallFilter.Any())
			{
				return;
			}

			var chunk = _chunksContainer.GetChunkWithLowestPriority(_chunkToUpdateWallFilter);
			UpdateWall(chunk).Forget();
		}

		private async UniTask UpdateWall(int chunkEntity)
		{
			var chunk = _chunkPool.Get(chunkEntity);
			var token = chunk.CancellationTokenSource.Token;
			var chunkPhysics = _chunkPhysicsPool.Get(chunkEntity);
			var dirtyWalls = _dirtyWallsPool.Get(chunkEntity).Walls;
			_dirtyWallsPool.Del(chunkEntity);
			try
			{
				_generatingPool.Add(chunkEntity);
				await UniTask.RunOnThreadPool(() => UpdateWalls(chunkPhysics, dirtyWalls, token),
					cancellationToken: token);
				_dirtyMeshPool.AddIfNotHas(chunkEntity);
			}
			catch(OperationCanceledException)
			{
			}
			finally
			{
				_generatingPool.Del(chunkEntity);
			}
		}

		private void UpdateWalls(ChunkPhysicsComponent chunkPhysics, Face wallsToUpdate, CancellationToken token) 
		{
			var wall = Face.Forward;
			if(wallsToUpdate.HasFace(wall))
			{
				chunkPhysics.MeshPartsContainer.UpdateWallMeshes(wall, token);
			}

			wall = Face.Back;
			if(wallsToUpdate.HasFace(wall))
			{
				chunkPhysics.MeshPartsContainer.UpdateWallMeshes(wall, token);
			}

			wall = Face.Right;
			if(wallsToUpdate.HasFace(wall))
			{
				chunkPhysics.MeshPartsContainer.UpdateWallMeshes(wall, token);
			}

			wall = Face.Left;
			if(wallsToUpdate.HasFace(wall))
			{
				chunkPhysics.MeshPartsContainer.UpdateWallMeshes(wall, token);
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
	}
}
