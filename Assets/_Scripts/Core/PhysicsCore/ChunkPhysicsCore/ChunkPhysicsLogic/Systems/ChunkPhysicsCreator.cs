using Assets.Scripts.Core.ChunkCore.LifeTimeControl.Components;
using Assets.Scripts.Core.ChunkPhysicsCore.ChunkPhysicsScripts.Components.Elements.BlockPhysicsGetters;
using ChunkCore;
using ChunkCore.Loading.Components;
using GraphicsCore.ChunkGraphicsCore.LifeTimeControl.Components;
using Leopotam.EcsLite;
using PhysicsCore.ChunkPhysicsCore.LifeTimeControl.Components;
using System;

namespace PhysicsCore.ChunkPhysicsCore.LifeTimeControl.Systems
{
	public class ChunkPhysicsCreator : IEcsPreInitSystem, IEcsRunSystem
	{
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
			chunkPhysics.BlocksPhysicsGetter = new BlocksPhysicsGetter(chunkEntity, world);
			chunkPhysics.MeshPartsContainer = new ColliderMeshPartsContainer(chunkPhysics.BlocksPhysicsGetter);
		}
	}
}
