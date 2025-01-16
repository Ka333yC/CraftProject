using System;
using System.Threading;
using _Scripts.Apart.Extensions.Ecs;
using _Scripts.Core.ChunkCore.ChunkLogic.Components;
using _Scripts.Core.ChunkCore.ChunksContainerLogic.Components;
using _Scripts.Core.ChunkCore.ChunksContainerLogic.Components.Elements;
using _Scripts.Core.ChunkGraphicsCore.ChunkGraphicsLogic.Components;
using _Scripts.Core.ChunkGraphicsCore.ChunkGraphicsLogic.MeshGeneration.Components;
using _Scripts.Core.ChunkGraphicsCore.ChunkGraphicsLogic.MeshPartsContainerLogic.Components;
using _Scripts.Core.ChunkGraphicsCore.ChunkGraphicsLogic.MeshPartsContainerLogic.WallUpdate.Components;
using Cysharp.Threading.Tasks;
using Leopotam.EcsLite;

namespace _Scripts.Core.ChunkGraphicsCore.ChunkGraphicsLogic.MeshPartsContainerLogic.WallUpdate.Systems
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
		private EcsFilter _chunksToUpdateWallFilter;
		private EcsFilter _wallGeneratingFilter;

		public void PreInit(IEcsSystems systems)
		{
			_world = systems.GetWorld();
			_chunkPool = _world.GetPool<ChunkComponent>();
			_chunkGraphicsPool = _world.GetPool<ChunkGraphicsComponent>();
			_dirtyWallsPool = _world.GetPool<DirtyWallsComponent>();
			_generatingPool = _world.GetPool<MeshPartsContainerWallGeneratingTag>();
			_dirtyMeshPool = _world.GetPool<ChunkGraphicsDirtyMeshComponent>();
			_chunksToUpdateWallFilter = _world
				.Filter<DirtyWallsComponent>()
				.Inc<ChunkComponent>()
				.Inc<ChunkGraphicsComponent>()
				.Inc<MeshPartsContainerInitializedTag>()
				.Exc<MeshPartsContainerWallGeneratingTag>()
				.End();
			_wallGeneratingFilter = _world
				.Filter<MeshPartsContainerWallGeneratingTag>()
				.End();
		}

		public void Init(IEcsSystems systems)
		{
			_chunksContainer = GetChunksContainer(systems.GetWorld());
		}

		public void Run(IEcsSystems systems)
		{
			if(_wallGeneratingFilter.Any() || !_chunksToUpdateWallFilter.Any())
			{
				return;
			}

			var chunk = _chunksContainer.GetChunkWithLowestPriority(_chunksToUpdateWallFilter);
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
