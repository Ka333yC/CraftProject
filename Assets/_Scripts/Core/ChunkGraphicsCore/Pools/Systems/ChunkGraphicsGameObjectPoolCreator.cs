using Leopotam.EcsLite;
using GraphicsCore.ChunkGraphicsCore.Cache.ChunkGraphicsMeshFilterPoolScripts.Components;
using TempScripts;
using UnityEngine;

namespace GraphicsCore.ChunkGraphicsCore.Cache.ChunkGraphicsMeshFilterPoolScripts.Systems
{
	public class ChunkGraphicsGameObjectPoolCreator : IEcsPreInitSystem, IEcsInitSystem
	{
		private EcsPool<ChunkGraphicsGameObjectPoolComponent> _chunkGraphicsPoolComponentPool;

		public void PreInit(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();
			_chunkGraphicsPoolComponentPool = world.GetPool<ChunkGraphicsGameObjectPoolComponent>();
		}

		public void Init(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();
			var meshFilterPoolEntity = world.NewEntity();
			ref var meshFilterPoolComponent = ref _chunkGraphicsPoolComponentPool.Add(meshFilterPoolEntity);
			var pool = new ChunkGraphicsGameObjectPool();
			meshFilterPoolComponent.Pool = pool;
		}
	}
}
