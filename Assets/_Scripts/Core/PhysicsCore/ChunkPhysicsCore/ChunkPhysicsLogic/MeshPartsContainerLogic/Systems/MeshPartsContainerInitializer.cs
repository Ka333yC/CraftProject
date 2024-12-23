using System;
using Assets.Scripts.Core.ChunkCore.LifeTimeControl.Components;
using Cysharp.Threading.Tasks;
using PhysicsCore.ChunkPhysicsCore.LifeTimeControl.Components;
using Leopotam.EcsLite;
using MeshCreation;
using PhysicsCore.ObjectPhysics.Components;
using System.Collections.Generic;
using UnityEngine;
using Extensions;
using System.Linq;
using Assets.Scripts.Core.PhysicsCore.ChunkPhysicsCore.MeshPartsContainerUpdating.Components;
using Extensions.Ecs;
using System.Diagnostics;
using Assets.Scripts.Core.PhysicsCore.ChunkPhysicsCore.MeshUpdating.Components;
using Assets.Scripts.Apart.Extensions.Ecs;
using ChunkCore.ChunksContainerScripts.Components;
using ChunkCore.ChunksContainerScripts;

namespace Assets.Scripts.Core.PhysicsCore.ChunkPhysicsCore.MeshPartsContainerUpdating.Systems
{
	public class MeshPartsContainerInitializer : IEcsPreInitSystem, IEcsInitSystem, IEcsRunSystem
	{
		private ChunksContainer _chunksContainer;

		private EcsWorld _world;
		private EcsPool<ChunkComponent> _chunkPool;
		private EcsPool<ChunkPhysicsComponent> _chunkPhysicsPool;
		private EcsPool<MeshPartsContainerInitializedTag> _initializedPool;
		private EcsPool<MeshPartsContainerInitializingTag> _initializingPool;
		private EcsPool<ChunkPhysicsDirtyMeshTag> _dirtyMeshPool;
		private EcsFilter _chunksToInitializeFilter;
		private EcsFilter _initializingFilter;

		public void PreInit(IEcsSystems systems)
		{
			_world = systems.GetWorld();
			_chunkPool = _world.GetPool<ChunkComponent>();
			_chunkPhysicsPool = _world.GetPool<ChunkPhysicsComponent>();
			_initializedPool = _world.GetPool<MeshPartsContainerInitializedTag>();
			_initializingPool = _world.GetPool<MeshPartsContainerInitializingTag>();
			_dirtyMeshPool = _world.GetPool<ChunkPhysicsDirtyMeshTag>();
			_chunksToInitializeFilter = _world
				.Filter<ChunkComponent>()
				.Inc<ChunkPhysicsComponent>()
				.Exc<MeshPartsContainerInitializedTag>()
				.Exc<MeshPartsContainerInitializingTag>()
				.End();
			_initializingFilter = _world
				.Filter<MeshPartsContainerInitializingTag>()
				.End();
		}

		public void Init(IEcsSystems systems)
		{
			_chunksContainer = GetChunksContainer(systems.GetWorld());
		}

		public void Run(IEcsSystems systems)
		{
			if(_initializingFilter.Any() || !_chunksToInitializeFilter.Any())
			{
				return;
			}

			var chunkToInitialize = _chunksContainer.GetChunkWithLowestPriority(_chunksToInitializeFilter);
			InitializeMeshPartsContainer(chunkToInitialize).Forget();
		}

		private async UniTask InitializeMeshPartsContainer(int chunkEntity)
		{
			var chunk = _chunkPool.Get(chunkEntity);
			var token = chunk.CancellationTokenSource.Token;
			var chunkPhysics = _chunkPhysicsPool.Get(chunkEntity);
			try
			{
				_initializingPool.Add(chunkEntity);
				await UniTask.RunOnThreadPool(() => chunkPhysics.MeshPartsContainer.UpdateAllMeshes(token), 
					cancellationToken: token);
				_initializedPool.Add(chunkEntity);
				_dirtyMeshPool.AddIfNotHas(chunkEntity);
			}
			catch(OperationCanceledException)
			{
			}
			finally
			{
				_initializingPool.Del(chunkEntity);
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
