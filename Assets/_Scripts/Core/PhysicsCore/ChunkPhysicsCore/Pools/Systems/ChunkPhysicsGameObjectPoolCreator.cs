using Leopotam.EcsLite;
using PhysicsCore.ChunkPhysicsCore.Cache.ChunkPhysicsMeshColliderPoolScripts.Components;

namespace PhysicsCore.ChunkPhysicsCore.Cache.ChunkPhysicsMeshColliderPoolScripts.Systems
{
	public class ChunkPhysicsGameObjectPoolCreator : IEcsPreInitSystem, IEcsInitSystem
	{
		private EcsPool<ChunkPhysicsGameObjectPoolComponent> _meshColliderPoolComponentPool;

		public void PreInit(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();
			_meshColliderPoolComponentPool = world.GetPool<ChunkPhysicsGameObjectPoolComponent>();
		}

		public void Init(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();
			var entity = world.NewEntity();
			ref var meshColliderPoolComponent = ref _meshColliderPoolComponentPool.Add(entity);
			var pool = new ChunkPhysicsGameObjectPool();
			meshColliderPoolComponent.Pool = pool;
		}
	}
}
