using Assets.Scripts.Core.ChunkCore.LifeTimeControl.Components;
using Assets.Scripts.Core.ChunkPhysicsCore.ChunkPhysicsScripts.Components.Elements.BlockPhysicsGetters;
using ChunkCore;
using ChunkCore.Loading.Components;
using GraphicsCore.ChunkGraphicsCore.Cache.ChunkGraphicsMeshFilterPoolScripts;
using GraphicsCore.ChunkGraphicsCore.Cache.ChunkGraphicsMeshFilterPoolScripts.Components;
using GraphicsCore.ChunkGraphicsCore.LifeTimeControl.Components;
using Leopotam.EcsLite;
using PhysicsCore.ChunkPhysicsCore.Cache.ChunkPhysicsMeshColliderPoolScripts;
using PhysicsCore.ChunkPhysicsCore.Cache.ChunkPhysicsMeshColliderPoolScripts.Components;
using PhysicsCore.ChunkPhysicsCore.LifeTimeControl.Components;
using System;

namespace PhysicsCore.ChunkPhysicsCore.LifeTimeControl.Systems
{
	public class ChunkPhysicsCreator : IEcsPreInitSystem, IEcsInitSystem, IEcsRunSystem
	{
		private ChunkPhysicsGameObjectPool _chunkPhysicsGameObjectPool;

		private EcsPool<ChunkComponent> _chunkPool;
		private EcsPool<ChunkPhysicsComponent> _chunksPhysicsPool;
		private EcsFilter _chunkWithoutChunkPhysicsFilter;

		public void PreInit(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();
			_chunkPool = world.GetPool<ChunkComponent>();
			_chunksPhysicsPool = world.GetPool<ChunkPhysicsComponent>();
			_chunkWithoutChunkPhysicsFilter = world
				.Filter<ChunkComponent>()
				.Inc<ChunkInitializedTag>()
				.Exc<ChunkPhysicsComponent>()
				.End();
		}

		public void Init(IEcsSystems systems)
		{
			_chunkPhysicsGameObjectPool = GetChunkPhysicsGameObjectPool(systems.GetWorld());
		}

		public void Run(IEcsSystems systems)
		{
			foreach(var chunkWithoutChunkPhysicsEntity in _chunkWithoutChunkPhysicsFilter)
			{
				CreateChunkPhysics(chunkWithoutChunkPhysicsEntity, systems.GetWorld());
			}
		}

		private void CreateChunkPhysics(int chunkEntity, EcsWorld world)
		{
			ref var chunkPhysics = ref _chunksPhysicsPool.Add(chunkEntity);
			var chunkPhysicsGameObject = _chunkPhysicsGameObjectPool.Get();
			var gridPosition = _chunkPool.Get(chunkEntity).GridPosition;
			chunkPhysicsGameObject.transform.position = ChunkConstantData.GridToWorldPosition(gridPosition);
			chunkPhysics.GameObject = chunkPhysicsGameObject;
			chunkPhysics.BlocksPhysicsGetter = new BlocksPhysicsGetter(chunkEntity, world);
			chunkPhysics.MeshPartsContainer =
				new ColliderMeshPartsContainer(chunkPhysics.BlocksPhysicsGetter);
		}

		private ChunkPhysicsGameObjectPool GetChunkPhysicsGameObjectPool(EcsWorld world)
		{
			var pool = world.GetPool<ChunkPhysicsGameObjectPoolComponent>();
			var filter = world
				.Filter<ChunkPhysicsGameObjectPoolComponent>()
				.End();
			foreach(var entity in filter)
			{
				return pool.Get(entity).Pool;
			}

			throw new Exception($"{typeof(ChunkPhysicsGameObjectPoolComponent).Name} not found");
		}
	}
}
