using System;
using Leopotam.EcsLite;
using GraphicsCore.ChunkGraphicsCore.Cache.ChunkGraphicsMeshFilterPoolScripts;
using GraphicsCore.ChunkGraphicsCore.Cache.ChunkGraphicsMeshFilterPoolScripts.Components;
using GraphicsCore.ChunkGraphicsCore.LifeTimeControl.Components;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Assets.Scripts.Core.ChunkCore.LifeTimeControl.Components;

namespace GraphicsCore.ChunkGraphicsCore.LifeTimeControl.Systems
{
	public class ChunkGraphicsDestroyer : IEcsPreInitSystem, IEcsInitSystem, IEcsRunSystem
	{
		private ChunkGraphicsGameObjectPool _chunkGraphicsGameObjectPool;

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

		public void Init(IEcsSystems systems)
		{
			_chunkGraphicsGameObjectPool = GetChunkGraphicsGameObjectPool(systems.GetWorld());
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
			_chunkGraphicsGameObjectPool.Return(chunkGraphics.GameObject);
			_chunkGraphicsPool.Del(chunkGraphicsEntity);
		}

		private ChunkGraphicsGameObjectPool GetChunkGraphicsGameObjectPool(EcsWorld world)
		{
			var pool = world.GetPool<ChunkGraphicsGameObjectPoolComponent>();
			var filter = world
				.Filter<ChunkGraphicsGameObjectPoolComponent>()
				.End();
			foreach(var entity in filter)
			{
				return pool.Get(entity).Pool;
			}

			throw new Exception($"{typeof(ChunkGraphicsGameObjectPoolComponent).Name} not found");
		}
	}
}
