using Leopotam.EcsLite;
using ChunkCore;
using ChunkCore.Loading.Components;
using GraphicsCore.ChunkGraphicsCore.LifeTimeControl.Components;
using System;
using Assets.Scripts.Core.ChunkCore.LifeTimeControl.Components;
using Assets.Scripts.Core.ChunkGraphicsCore.ChunkGraphicsScripts.Components.Elements;

namespace GraphicsCore.ChunkGraphicsCore.LifeTimeControl.Systems
{
	public class ChunkGraphicsCreator : IEcsPreInitSystem, IEcsRunSystem
	{
		private EcsPool<ChunkComponent> _chunkPool;
		private EcsPool<ChunkGraphicsComponent> _chunkGraphicsPool;
		private EcsFilter _chunkWithoutChunkGraphicsFilter;

		public void PreInit(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();
			_chunkPool = world.GetPool<ChunkComponent>();
			_chunkGraphicsPool = world.GetPool<ChunkGraphicsComponent>();
			_chunkWithoutChunkGraphicsFilter = world
				.Filter<ChunkComponent>()
				.Inc<ChunkInitializedTag>()
				.Exc<ChunkGraphicsComponent>()
				.End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach(var chunkWithoutChunkGraphicsEntity in _chunkWithoutChunkGraphicsFilter)
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
