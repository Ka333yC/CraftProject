using System;
using System.Linq;
using Assets.Scripts.Apart.Extensions.Ecs;
using Assets.Scripts.Core.ChunkCore.ChunkLogic.Loading.Saving.Components;
using Assets.Scripts.Core.ChunkCore.LifeTimeControl.Components;
using Assets.Scripts.Core.ChunkCore.Saving.Components;
using ChunkCore.ChunksContainerScripts;
using ChunkCore.ChunksContainerScripts.Components;
using Cysharp.Threading.Tasks;
using Leopotam.EcsLite;

namespace Assets.Scripts.Core.ChunkCore.Saving.Systems
{
	public class ChunkSaveSystem : IEcsPreInitSystem, IEcsInitSystem, IEcsRunSystem
	{
		private ChunksContainer _chunksContainer;
		private ChunkSerializer _chunkSerializer;

		private EcsWorld _world;
		private EcsPool<ChunkComponent> _chunkPool;
		private EcsPool<ChunkSavingTag> _chunkSavingPool;
		private EcsPool<NeedSaveChunkTag> _needSaveChunkPool;
		private EcsFilter _chunksToSaveFilter;
		private EcsFilter _chunkSavingFilter;

		public void PreInit(IEcsSystems systems)
		{
			_world = systems.GetWorld();
			_chunkPool = _world.GetPool<ChunkComponent>();
			_chunkSavingPool = _world.GetPool<ChunkSavingTag>();
			_needSaveChunkPool = _world.GetPool<NeedSaveChunkTag>();
			_chunksToSaveFilter = _world
				.Filter<ChunkComponent>()
				.Inc<NeedSaveChunkTag>()
				.Exc<ChunkSavingTag>()
				.End();
			_chunkSavingFilter = _world
				.Filter<ChunkSavingTag>()
				.End();
		}

		public void Init(IEcsSystems systems)
		{
			var world = systems.GetWorld();
			_chunksContainer = GetChunksContainer(world);
			_chunkSerializer = GetChunkSerializer(world);
		}

		public void Run(IEcsSystems systems)
		{
			if(_chunkSavingFilter.Any() || !_chunksToSaveFilter.Any())
			{
				return;
			}

			var chunkToSave = _chunksContainer.GetChunkWithHighlyPriority(_chunksToSaveFilter);
			SaveChunk(chunkToSave).Forget();
		}

		private async UniTask SaveChunk(int chunkEntity)
		{
			var chunk = _chunkPool.Get(chunkEntity);
			var token = chunk.CancellationTokenSource.Token;
			try
			{
				_chunkSavingPool.Add(chunkEntity);
				_needSaveChunkPool.Del(chunkEntity);
				await _chunkSerializer.Save(chunkEntity, token);
			}
			catch(OperationCanceledException)
			{
			}
			finally
			{
				_chunkSavingPool.Del(chunkEntity);
			}
		}

		private ChunksContainer GetChunksContainer(EcsWorld world)
		{
			var pool = world.GetPool<ChunksContainerComponent>();
			var filter = world
				.Filter<ChunksContainerComponent>()
				.End();
			foreach(var entity in filter)
			{
				return pool.Get(entity).ChunksContainer;
			}

			throw new Exception($"{typeof(ChunksContainerComponent).Name} not found");
		}

		private ChunkSerializer GetChunkSerializer(EcsWorld world)
		{
			var pool = world.GetPool<ChunkSerializerComponent>();
			var filter = world
				.Filter<ChunkSerializerComponent>()
				.End();
			foreach(var entity in filter)
			{
				return pool.Get(entity).ChunkSerializer;
			}

			throw new Exception($"{typeof(ChunkSerializerComponent).Name} not found");
		}
	}
}
