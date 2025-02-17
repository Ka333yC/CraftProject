using _Scripts.Core.BlocksCore;
using _Scripts.Core.ChunkCore.ChunkLogic.ChunkGeneration;
using _Scripts.TempScripts;
using Leopotam.EcsLite;
using Zenject;

namespace _Scripts.Implementation.ChunkImplementation.Generation.Systems
{
	public class ChunkGeneratorCreator : IEcsInitSystem
	{
		[Inject]
		private BlocksArchetypes _blocksArchetypes;

		public void Init(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();
			var pool = world.GetPool<ChunkGeneratorComponent>();
			var entity = world.NewEntity();
			ref var component = ref pool.Add(entity);
			// component.ChunkGenerator = new OverworldChunkGenerator(systems.GetWorld(),
			// 	0, Singleton.Instance.NoiseSettings, _blocksArchetypes);
			component.ChunkGenerator = new ClassicFlatChunkGenerator(systems.GetWorld(),
				0, Singleton.Instance.NoiseSettings, _blocksArchetypes);
		}
	}
}
