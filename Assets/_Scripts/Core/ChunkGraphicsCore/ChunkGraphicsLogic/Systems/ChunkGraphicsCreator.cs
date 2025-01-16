using _Scripts.Core.ChunkCore.ChunkLogic.Components;
using _Scripts.Core.ChunkGraphicsCore.ChunkGraphicsLogic.Components;
using _Scripts.Core.ChunkGraphicsCore.ChunkGraphicsLogic.Components.Elements;
using _Scripts.Core.ChunkGraphicsCore.ChunkGraphicsLogic.Components.Elements.BlockGraphicsGetters;
using Leopotam.EcsLite;

namespace _Scripts.Core.ChunkGraphicsCore.ChunkGraphicsLogic.Systems
{
	public class ChunkGraphicsCreator : IEcsPreInitSystem, IEcsRunSystem
	{
		private EcsPool<ChunkGraphicsComponent> _chunkGraphicsPool;
		private EcsFilter _chunksWithoutChunkGraphicsFilter;

		public void PreInit(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();
			_chunkGraphicsPool = world.GetPool<ChunkGraphicsComponent>();
			_chunksWithoutChunkGraphicsFilter = world
				.Filter<ChunkComponent>()
				.Inc<ChunkInitializedTag>()
				.Exc<ChunkGraphicsComponent>()
				.End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach(var chunkWithoutChunkGraphicsEntity in _chunksWithoutChunkGraphicsFilter)
			{
				CreateChunkGraphics(chunkWithoutChunkGraphicsEntity, systems.GetWorld());
			}
		}

		private void CreateChunkGraphics(int chunkEntity, EcsWorld world)
		{
			ref var chunkGraphics = ref _chunkGraphicsPool.Add(chunkEntity);
			chunkGraphics.BlocksGraphicsGetter = new BlocksGraphicsGetter(chunkEntity, world);
			chunkGraphics.MeshPartsContainer = new GraphicsMeshPartsContainer(chunkGraphics.BlocksGraphicsGetter);
		}
	}
}
