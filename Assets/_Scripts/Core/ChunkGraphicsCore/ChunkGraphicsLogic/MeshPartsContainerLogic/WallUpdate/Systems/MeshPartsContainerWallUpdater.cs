using System;
using Assets.Scripts.Core.ChunkCore.LifeTimeControl.Components;
using Assets.Scripts.Core.GraphicsCore.ChunkGraphicsCore.MeshPartsContainerUpdating.Components;
using Assets.Scripts.Core.GraphicsCore.ChunkGraphicsCore.MeshUpdating.Components;
using Cysharp.Threading.Tasks;
using GraphicsCore.ChunkGraphicsCore.LifeTimeControl.Components;
using Leopotam.EcsLite;
using System.Collections.Generic;
using UnityEngine;
using Extensions;
using System.Linq;
using Assets.Scripts.Core.GraphicsCore.ChunkGraphicsCore.MeshPartsContainerUpdating.WallUpdating.Components;
using ChunkCore;
using System.Threading;
using Extensions.Ecs;
using Assets.Scripts.Apart.Extensions.Ecs;
using ChunkCore.ChunksContainerScripts.Components;
using ChunkCore.ChunksContainerScripts;
using Assets.Scripts.Core.ChunkGraphicsCore.MeshPartsContainerUpdating.WallUpdating.Components;

namespace Assets.Scripts.Core.GraphicsCore.ChunkGraphicsCore.MeshPartsContainerUpdating.WallUpdating.Systems
{
	public class MeshPartsContainerWallUpdater : IEcsPreInitSystem, IEcsInitSystem, IEcsRunSystem
	{
		private ChunksContainer _chunksContainer;

		private EcsWorld _world;
		private EcsPool<ChunkComponent> _chunkPool;
		private EcsPool<ChunkGraphicsComponent> _chunkGraphicsPool;
		private EcsPool<DirtyWallsComponent> _dirtyWallsPool;
		private EcsPool<MeshPartsContainerWallGeneratingTag> _generatingPool;
		private EcsPool<ChunkGraphicsDirtyMeshComponent> _dirtyMeshPool;
		private EcsFilter _chunkToUpdateWallFilter;
		private EcsFilter _generatingFilter;

		public void PreInit(IEcsSystems systems)
		{
			_world = systems.GetWorld();
			_chunkPool = _world.GetPool<ChunkComponent>();
			_chunkGraphicsPool = _world.GetPool<ChunkGraphicsComponent>();
			_dirtyWallsPool = _world.GetPool<DirtyWallsComponent>();
			_generatingPool = _world.GetPool<MeshPartsContainerWallGeneratingTag>();
			_dirtyMeshPool = _world.GetPool<ChunkGraphicsDirtyMeshComponent>();
			_chunkToUpdateWallFilter = _world
				.Filter<DirtyWallsComponent>()
				.Inc<ChunkComponent>()
				.Inc<ChunkGraphicsComponent>()
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
			var chunkGraphics = _chunkGraphicsPool.Get(chunkEntity);
			var dirtyWalls = _dirtyWallsPool.Get(chunkEntity).Walls;
			_dirtyWallsPool.Del(chunkEntity);
			try
			{
				_generatingPool.Add(chunkEntity);
				await UniTask.RunOnThreadPool(() => UpdateWalls(chunkGraphics, dirtyWalls, token), 
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

		private void UpdateWalls(ChunkGraphicsComponent chunkGraphics, Face wallsToUpdate, CancellationToken token) 
		{
			var wall = Face.Forward;
			if(wallsToUpdate.HasFace(wall))
			{
				chunkGraphics.MeshPartsContainer.UpdateWallMeshes(wall, token);
			}

			wall = Face.Back;
			if(wallsToUpdate.HasFace(wall))
			{
				chunkGraphics.MeshPartsContainer.UpdateWallMeshes(wall, token);
			}

			wall = Face.Right;
			if(wallsToUpdate.HasFace(wall))
			{
				chunkGraphics.MeshPartsContainer.UpdateWallMeshes(wall, token);
			}

			wall = Face.Left;
			if(wallsToUpdate.HasFace(wall))
			{
				chunkGraphics.MeshPartsContainer.UpdateWallMeshes(wall, token);
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
