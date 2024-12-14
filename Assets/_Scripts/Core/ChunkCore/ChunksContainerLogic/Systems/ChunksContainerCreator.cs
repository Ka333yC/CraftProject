using Leopotam.EcsLite;
using ChunkCore.ChunksContainerScripts.Components;

namespace ChunkCore.ChunksContainerScripts.Systems
{
	public class ChunksContainerCreator : IEcsPreInitSystem, IEcsInitSystem
	{
		private EcsPool<ChunksContainerComponent> _chunksContainerPool;

		public void PreInit(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();
			_chunksContainerPool = world.GetPool<ChunksContainerComponent>();
		}

		public void Init(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();
			int chunksContainerEntity = world.NewEntity();
			ref var chunksContainerComponent = ref _chunksContainerPool.Add(chunksContainerEntity);
			chunksContainerComponent.ChunksContainer = new ChunksContainer();
		}
	}
}