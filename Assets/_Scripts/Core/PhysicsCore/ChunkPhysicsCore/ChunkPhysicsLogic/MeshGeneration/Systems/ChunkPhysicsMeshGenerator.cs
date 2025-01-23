using System;
using _Scripts.Apart.Extensions.Ecs;
using _Scripts.Core.ChunkCore.ChunkLogic.Components;
using _Scripts.Core.ChunkCore.ChunksContainerLogic.Components;
using _Scripts.Core.ChunkCore.ChunksContainerLogic.Components.Elements;
using _Scripts.Core.MeshWrap;
using _Scripts.Core.PhysicsCore.ChunkPhysicsCore.ChunkPhysicsLogic.Components;
using _Scripts.Core.PhysicsCore.ChunkPhysicsCore.ChunkPhysicsLogic.MeshGeneration.Components;
using _Scripts.Core.PhysicsCore.ChunkPhysicsCore.ChunkPhysicsLogic.MeshPartsContainerLogic.Components;
using _Scripts.Core.PhysicsCore.Presets;
using Cysharp.Threading.Tasks;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace _Scripts.Core.PhysicsCore.ChunkPhysicsCore.ChunkPhysicsLogic.MeshGeneration.Systems
{
	public class ChunkPhysicsMeshGenerator : IEcsPreInitSystem, IEcsInitSystem, IEcsRunSystem
	{
		[Inject]
		private PhysicsPresets _physicsPresets;

		private ChunksContainer _chunksContainer;

		private EcsWorld _world;
		private EcsPool<ChunkComponent> _chunkPool;
		private EcsPool<ChunkPhysicsComponent> _chunkPhysicsPool;
		private EcsPool<ChunkPhysicsDirtyMeshTag> _dirtyMeshPool;
		private EcsPool<ChunkPhysicsMeshGeneratingTag> _meshGeneratingPool;
		private EcsPool<ChunkPhysicsMeshGeneratedNotificationTag> _meshGeneratedNotificationPool;
		private EcsFilter _chunksToGenerateMeshFilter;
		private EcsFilter _meshGeneratingFilter;

		public void PreInit(IEcsSystems systems)
		{
			_world = systems.GetWorld();
			_chunkPool = _world.GetPool<ChunkComponent>();
			_chunkPhysicsPool = _world.GetPool<ChunkPhysicsComponent>();
			_dirtyMeshPool = _world.GetPool<ChunkPhysicsDirtyMeshTag>();
			_meshGeneratingPool = _world.GetPool<ChunkPhysicsMeshGeneratingTag>();
			_meshGeneratedNotificationPool = _world.GetPool<ChunkPhysicsMeshGeneratedNotificationTag>();
			_chunksToGenerateMeshFilter = _world
				.Filter<ChunkPhysicsDirtyMeshTag>()
				.Inc<ChunkComponent>()
				.Inc<ChunkPhysicsComponent>()
				.Inc<MeshPartsContainerInitializedTag>()
				.Exc<ChunkPhysicsMeshGeneratingTag>()
				.End();
			_meshGeneratingFilter = _world
				.Filter<ChunkPhysicsMeshGeneratingTag>()
				.End();
		}

		public void Init(IEcsSystems systems)
		{
			_chunksContainer = GetChunksContainer(systems.GetWorld());
		}

		public void Run(IEcsSystems systems)
		{
			if(_meshGeneratingFilter.Any() || !_chunksToGenerateMeshFilter.Any())
			{
				return;
			}

			var chunkToGenerateMesh = _chunksContainer.GetChunkWithLowestPriority(_chunksToGenerateMeshFilter);
			GenerateMesh(chunkToGenerateMesh).Forget();
		}

		private async UniTask GenerateMesh(int chunkEntity)
		{
			var chunk = _chunkPool.Get(chunkEntity);
			var token = chunk.CancellationTokenSource.Token;
			var chunkPhysics = _chunkPhysicsPool.Get(chunkEntity);
			_dirtyMeshPool.Del(chunkEntity);
			try
			{
				_meshGeneratingPool.Add(chunkEntity);
				using MeshBuilder meshBuilder = new MeshBuilder();
				await UniTask.RunOnThreadPool(() =>
				{
					meshBuilder.AddRange(chunkPhysics.MeshPartsContainer.GetMeshParts());
					token.ThrowIfCancellationRequested();
					meshBuilder.Bake();
				});

				var mesh = meshBuilder.CreateMesh();
				var meshId = mesh.GetInstanceID();
				await UniTask.RunOnThreadPool(() => Physics.BakeMesh(meshId, false,
					_physicsPresets.CookingOptions), cancellationToken: token);
				// Проверяем на Has потому что за время генерации чанк мог быть уничтожен
				if(_chunkPool.Has(chunkEntity))
				{
					chunk.GameObject.MeshCollider.sharedMesh = mesh;
					_meshGeneratedNotificationPool.Add(chunkEntity);
				}
			}
			catch(OperationCanceledException)
			{
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
