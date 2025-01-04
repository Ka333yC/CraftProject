using System;
using Leopotam.EcsLite;
using GraphicsCore.ChunkGraphicsCore.LifeTimeControl.Components;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Assets.Scripts.Core.ChunkCore.LifeTimeControl.Components;

namespace GraphicsCore.ChunkGraphicsCore.LifeTimeControl.Systems
{
	public class ChunkGraphicsDestroyer : IEcsPreInitSystem, IEcsRunSystem
	{
		private EcsPool<ChunkGraphicsComponent> _chunkGraphicsPool;
		private EcsFilter _chunkGraphicsToDestroyFilter;

		public void PreInit(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();
			_chunkGraphicsPool = world.GetPool<ChunkGraphicsComponent>();
			_chunkGraphicsToDestroyFilter = world
				.Filter<ChunkGraphicsComponent>()
				.Exc<ChunkComponent>()
				.End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach(int chunkGraphicsEntity in _chunkGraphicsToDestroyFilter)
			{
				DestroyChunkGraphics(chunkGraphicsEntity);
			}
		}

		private void DestroyChunkGraphics(int chunkGraphicsEntity) 
		{
			ref var chunkGraphics = ref _chunkGraphicsPool.Get(chunkGraphicsEntity);
			chunkGraphics.MeshPartsContainer.Dispose();
			_chunkGraphicsPool.Del(chunkGraphicsEntity);
		}
	}
}
