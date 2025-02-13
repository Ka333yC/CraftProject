using _Scripts.Core.ChunkCore.ChunkLogic.Components;
using _Scripts.Core.ChunkCore.ChunkLogic.Pools.Components;
using Leopotam.EcsLite;
using Zenject;

namespace _Scripts.Core.ChunkCore.ChunkLogic.Pools.Systems
{
	public class ChunkGameObjectPoolCreator : IEcsPreInitSystem, IEcsInitSystem
	{
		[Inject]
		private ChunkGameObject _chunkPrefab;
		
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
			var creator = new ChunkGameObjectCreator(_chunkPrefab);
			var pool = new ChunkGameObjectPool(creator);
			component.Pool = pool;
		}
	}
}
