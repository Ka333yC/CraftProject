using Leopotam.EcsLite;
using ChunkCore;
using ChunkCore.Loading.Components;
using GraphicsCore.ChunkGraphicsCore.Cache.ChunkGraphicsMeshFilterPoolScripts;
using GraphicsCore.ChunkGraphicsCore.Cache.ChunkGraphicsMeshFilterPoolScripts.Components;
using GraphicsCore.ChunkGraphicsCore.LifeTimeControl.Components;
using System;
using Assets.Scripts.Core.ChunkCore.LifeTimeControl.Components;
using Assets.Scripts.Core.ChunkGraphicsCore.ChunkGraphicsScripts.Components.Elements;

namespace GraphicsCore.ChunkGraphicsCore.LifeTimeControl.Systems
{
	public class ChunkGraphicsCreator : IEcsPreInitSystem, IEcsInitSystem, IEcsRunSystem
	{
		private ChunkGraphicsGameObjectPool _chunkGraphicsGameObjectPool;

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

		public void Init(IEcsSystems systems)
		{
			_chunkGraphicsGameObjectPool = GetChunkGraphicsGameObjectPool(systems.GetWorld());
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
			var chunkGraphicsGameObject = _chunkGraphicsGameObjectPool.Get();
			var gridPosition = _chunkPool.Get(chunkEntity).GridPosition;
			chunkGraphicsGameObject.transform.position = ChunkConstantData.GridToWorldPosition(gridPosition);
			chunkGraphics.GameObject = chunkGraphicsGameObject;
			chunkGraphics.BlocksGraphicsGetter = new BlocksGraphicsGetter(chunkEntity, world);
			chunkGraphics.MeshPartsContainer =
				new GraphicsMeshPartsContainer(chunkGraphics.BlocksGraphicsGetter);
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
