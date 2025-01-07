using _Scripts.Core.ChunkCore.ChunkLogic.Pools.Components;
using Leopotam.EcsLite;

namespace _Scripts.Core.ChunkCore.ChunkLogic.Pools.Systems
{
	public class ChunkGameObjectPoolCreator : IEcsPreInitSystem, IEcsInitSystem
	{
		private EcsPool<ChunkGameObjectPoolComponent> _chunkGameObjectPoolComponentPool;

		public void PreInit(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();
			_chunkGameObjectPoolComponentPool = world.GetPool<ChunkGameObjectPoolComponent>();
		}

		public void Init(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();
			var entity = world.NewEntity();
			ref var component = ref _chunkGameObjectPoolComponentPool.Add(entity);
			var pool = new ChunkGameObjectPool();
			component.Pool = pool;
		}
	}
}
