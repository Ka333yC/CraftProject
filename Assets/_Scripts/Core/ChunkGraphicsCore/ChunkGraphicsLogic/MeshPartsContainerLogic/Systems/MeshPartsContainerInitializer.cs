using System;
using Assets.Scripts.Core.ChunkCore.LifeTimeControl.Components;
using Assets.Scripts.Core.GraphicsCore.ChunkGraphicsCore.MeshUpdating.Components;
using Cysharp.Threading.Tasks;
using GraphicsCore.ChunkGraphicsCore.LifeTimeControl.Components;
using Leopotam.EcsLite;
using MeshCreation;
using System.Collections.Generic;
using UnityEngine;
using Extensions;
using System.Linq;
using Assets.Scripts.Core.GraphicsCore.ChunkGraphicsCore.MeshPartsContainerUpdating.Components;
using ChunkCore.ChunksContainerScripts.Components;
using ChunkCore.ChunksContainerScripts;
using ChunkCore;
using Assets.Scripts.Core.GraphicsCore.ChunkGraphicsCore.MeshPartsContainerUpdating.WallUpdating.Components;
using Extensions.Ecs;
using System.Diagnostics;
using Assets.Scripts.Apart.Extensions.Ecs;

namespace Assets.Scripts.Core.GraphicsCore.ChunkGraphicsCore.MeshPartsContainerUpdating.Systems
{
	public class MeshPartsContainerInitializer : IEcsPreInitSystem, IEcsInitSystem, IEcsRunSystem
	{
		private ChunksContainer _chunksContainer;

		private EcsWorld _world;
		private EcsPool<ChunkComponent> _chunkPool;
		private EcsPool<ChunkGraphicsComponent> _chunkGraphicsPool;
		private EcsPool<MeshPartsContainerInitializedTag> _initializedPool;
		private EcsPool<MeshPartsContainerInitializingTag> _initializingPool;
		private EcsPool<ChunkGraphicsDirtyMeshComponent> _dirtyMeshPool;
		private EcsFilter _chunksToInitializeFilter;
		private EcsFilter _initializingFilter;

		public void PreInit(IEcsSystems systems)
		{
			_world = systems.GetWorld();
			_chunkPool = _world.GetPool<ChunkComponent>();
			_chunkGraphicsPool = _world.GetPool<ChunkGraphicsComponent>();
			_initializedPool = _world.GetPool<MeshPartsContainerInitializedTag>();
			_initializingPool = _world.GetPool<MeshPartsContainerInitializingTag>();
			_dirtyMeshPool = _world.GetPool<ChunkGraphicsDirtyMeshComponent>();
			_chunksToInitializeFilter = _world
				.Filter<ChunkComponent>()
				.Inc<ChunkGraphicsComponent>()
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
			var chunkGraphics = _chunkGraphicsPool.Get(chunkEntity);
			try
			{
				_initializingPool.Add(chunkEntity);
				await UniTask.RunOnThreadPool(() =>	chunkGraphics.MeshPartsContainer.UpdateAllMeshes(token), 
					cancellationToken: token);
				if(_chunkPool.Has(chunkEntity))
				{
					_initializedPool.Add(chunkEntity);
					_dirtyMeshPool.AddOrGet(chunkEntity);
				}
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
