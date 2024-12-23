using Assets.Scripts.Core.ChunkCore.LifeTimeControl.Components;
using GraphicsCore.ChunkGraphicsCore.Cache.ChunkGraphicsMeshFilterPoolScripts.Components;
using GraphicsCore.ChunkGraphicsCore.Cache.ChunkGraphicsMeshFilterPoolScripts;
using Leopotam.EcsLite;
using PhysicsCore.ChunkPhysicsCore.LifeTimeControl.Components;
using System;
using PhysicsCore.ChunkPhysicsCore.Cache.ChunkPhysicsMeshColliderPoolScripts.Components;
using PhysicsCore.ChunkPhysicsCore.Cache.ChunkPhysicsMeshColliderPoolScripts;

namespace PhysicsCore.ChunkPhysicsCore.LifeTimeControl.Systems
{
	public class ChunkPhysicsDestroyer : IEcsPreInitSystem, IEcsRunSystem
	{
		private EcsPool<ChunkPhysicsComponent> _chunkPhysicsPool;
		private EcsPool<ChunkPhysicsGameObjectPoolComponent> _chunkPhysicsGameObjectPool;
		private EcsFilter _chunkPhysicsGameObjectPoolFilter;
		private EcsFilter _chunkPhysicsToDestroyFilter;

		public void PreInit(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();
			_chunkPhysicsPool = world.GetPool<ChunkPhysicsComponent>();
			_chunkPhysicsGameObjectPool = world.GetPool<ChunkPhysicsGameObjectPoolComponent>();
			_chunkPhysicsGameObjectPoolFilter = world
				.Filter<ChunkPhysicsGameObjectPoolComponent>()
				.End();
			_chunkPhysicsToDestroyFilter = world
				.Filter<ChunkPhysicsComponent>()
				.Exc<ChunkComponent>()
				.End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach(int chunkPhysicsEntity in _chunkPhysicsToDestroyFilter)
			{
				ref var chunkPhysics = ref _chunkPhysicsPool.Get(chunkPhysicsEntity);
				chunkPhysics.BlocksPhysicsGetter.Dispose();
				chunkPhysics.MeshPartsContainer.Dispose();
				GetChunkPhysicsGameObjectPool().Return(chunkPhysics.GameObject);
				_chunkPhysicsPool.Del(chunkPhysicsEntity);
			}
		}

		private ChunkPhysicsGameObjectPool GetChunkPhysicsGameObjectPool()
		{
			foreach(var entity in _chunkPhysicsGameObjectPoolFilter)
			{
				return _chunkPhysicsGameObjectPool.Get(entity).Pool;
			}

			throw new Exception($"{typeof(ChunkPhysicsGameObjectPoolComponent).Name} not found");
		}
	}
}
