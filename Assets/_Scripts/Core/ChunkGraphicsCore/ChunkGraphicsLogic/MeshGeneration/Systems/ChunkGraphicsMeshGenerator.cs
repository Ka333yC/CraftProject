using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Scripts.Apart.Extensions.Ecs;
using _Scripts.Core.ChunkCore.ChunkLogic.Components;
using _Scripts.Core.ChunkCore.ChunksContainerLogic.Components;
using _Scripts.Core.ChunkCore.ChunksContainerLogic.Components.Elements;
using _Scripts.Core.ChunkGraphicsCore.ChunkGraphicsLogic.Components;
using _Scripts.Core.ChunkGraphicsCore.ChunkGraphicsLogic.MeshGeneration.Components;
using _Scripts.Core.ChunkGraphicsCore.ChunkGraphicsLogic.MeshPartsContainerLogic.Components;
using _Scripts.Core.MeshWrap;
using Cysharp.Threading.Tasks;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.Pool;

namespace _Scripts.Core.ChunkGraphicsCore.ChunkGraphicsLogic.MeshGeneration.Systems
{
	public class ChunkGraphicsMeshGenerator : IEcsPreInitSystem, IEcsInitSystem, IEcsRunSystem
	{
		private ChunksContainer _chunksContainer;

		private EcsWorld _world;
		private EcsPool<ChunkComponent> _chunkPool;
		private EcsPool<ChunkGraphicsComponent> _chunkGraphicsPool;
		private EcsPool<ChunkGraphicsDirtyMeshComponent> _dirtyMeshPool;
		private EcsPool<ChunkGraphicsMeshGeneratingTag> _meshGeneratingPool;
		private EcsFilter _chunksWithMeshGeneratingFilter;
		private EcsFilter _chunksToGenerateMeshFilter;

		public void PreInit(IEcsSystems systems)
		{
			_world = systems.GetWorld();
			_chunkPool = _world.GetPool<ChunkComponent>();
			_chunkGraphicsPool = _world.GetPool<ChunkGraphicsComponent>();
			_dirtyMeshPool = _world.GetPool<ChunkGraphicsDirtyMeshComponent>();
			_meshGeneratingPool = _world.GetPool<ChunkGraphicsMeshGeneratingTag>();
			_chunksWithMeshGeneratingFilter = _world
				.Filter<ChunkGraphicsMeshGeneratingTag>()
				.End();
			_chunksToGenerateMeshFilter = _world
				.Filter<ChunkGraphicsDirtyMeshComponent>()
				.Inc<ChunkComponent>()
				.Inc<ChunkGraphicsComponent>()
				.Inc<MeshPartsContainerInitializedTag>()
				.Exc<ChunkGraphicsMeshGeneratingTag>()
				.End();
		}

		public void Init(IEcsSystems systems)
		{
			_chunksContainer = GetChunksContainer(systems.GetWorld());
		}

		public void Run(IEcsSystems systems)
		{
			if(_chunksWithMeshGeneratingFilter.Any() || !_chunksToGenerateMeshFilter.Any())
			{
				return;
			}

			var chunkToGenerateMesh = _chunksContainer.GetChunkWithLowestPriority(_chunksToGenerateMeshFilter);
			var associatedChunkEntities =
					_dirtyMeshPool.Get(chunkToGenerateMesh).AssociatedDirtyMeshChunkEntities;
			GenerateMeshes(chunkToGenerateMesh, associatedChunkEntities).Forget();
			_dirtyMeshPool.Del(chunkToGenerateMesh);
		}

		private async UniTask GenerateMeshes(int chunkEntity, LinkedList<int> associatedChunkEntities)
		{
			var chunksWithGeneratingMeshes = DictionaryPool<int, Task<Mesh>>.Get();
			var generateMeshTask = GenerateMeshForChunk(chunkEntity);
			chunksWithGeneratingMeshes.Add(chunkEntity, generateMeshTask);
			if(associatedChunkEntities != null)
			{
				foreach(var associatedChunkEntity in associatedChunkEntities)
				{
					if(!chunksWithGeneratingMeshes.ContainsKey(associatedChunkEntity))
					{
						generateMeshTask = GenerateMeshForChunk(associatedChunkEntity);
						chunksWithGeneratingMeshes.Add(associatedChunkEntity, generateMeshTask);
					}
				}
			}

			await Task.WhenAll(chunksWithGeneratingMeshes.Values);
			if(_world.IsAlive())
			{
				foreach(var chunkWithGeneratingMesh in chunksWithGeneratingMeshes)
				{
					if(_chunkPool.Has(chunkWithGeneratingMesh.Key))
					{
						var chunk = _chunkPool.Get(chunkWithGeneratingMesh.Key);
						chunk.GameObject.MeshFilter.mesh = chunkWithGeneratingMesh.Value.Result;
					}
				}
			}

			DictionaryPool<int, Task<Mesh>>.Release(chunksWithGeneratingMeshes);
		}

		private async Task<Mesh> GenerateMeshForChunk(int chunkEntity)
		{
			var chunk = _chunkPool.Get(chunkEntity);
			var token = chunk.CancellationTokenSource.Token;
			var chunkGraphics = _chunkGraphicsPool.Get(chunkEntity);
			_meshGeneratingPool.Add(chunkEntity);
			try
			{
				using MeshBuilder meshBuilder = new MeshBuilder();
				await UniTask.RunOnThreadPool(() =>
				{
					meshBuilder.AddRange(chunkGraphics.MeshPartsContainer.GetMeshParts());
					token.ThrowIfCancellationRequested();
					meshBuilder.Bake();
				}, cancellationToken: token);

				return meshBuilder.CreateMesh();
			}
			catch(OperationCanceledException)
			{
				return null;
			}
			finally
			{
				_meshGeneratingPool.Del(chunkEntity);
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
	}
}
