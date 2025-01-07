using _Scripts.Core.ChunkCore.ChunkLogic.ChunkSerialization;
using Leopotam.EcsLite;
using Zenject;

namespace _Scripts.Implementation.ChunkImplementation.Serialization.Systems
{
	public class CompressedChunkSerializerCreator : IEcsInitSystem
	{
		[Inject]
		private DiContainer _container;

		public void Init(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();
			var pool = world.GetPool<ChunkSerializerComponent>();
			var entity = world.NewEntity();
			ref var component = ref pool.Add(entity);
			component.ChunkSerializer = _container.Instantiate<CompressedChunksSerializer>();
		}
	}
}
