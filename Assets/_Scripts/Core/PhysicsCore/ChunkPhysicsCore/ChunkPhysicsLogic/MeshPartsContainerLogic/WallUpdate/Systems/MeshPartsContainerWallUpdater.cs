using System;
using System.Threading;
using _Scripts.Apart.Extensions.Ecs;
using _Scripts.Core.ChunkCore.ChunkLogic.Components;
using _Scripts.Core.ChunkCore.ChunksContainerLogic.Components;
using _Scripts.Core.ChunkCore.ChunksContainerLogic.Components.Elements;
using _Scripts.Core.PhysicsCore.ChunkPhysicsCore.ChunkPhysicsLogic.Components;
using _Scripts.Core.PhysicsCore.ChunkPhysicsCore.ChunkPhysicsLogic.MeshGeneration.Components;
using _Scripts.Core.PhysicsCore.ChunkPhysicsCore.ChunkPhysicsLogic.MeshPartsContainerLogic.Components;
using _Scripts.Core.PhysicsCore.ChunkPhysicsCore.ChunkPhysicsLogic.MeshPartsContainerLogic.WallUpdate.Components;
using Cysharp.Threading.Tasks;
using Leopotam.EcsLite;

namespace _Scripts.Core.PhysicsCore.ChunkPhysicsCore.ChunkPhysicsLogic.MeshPartsContainerLogic.WallUpdate.Systems
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
