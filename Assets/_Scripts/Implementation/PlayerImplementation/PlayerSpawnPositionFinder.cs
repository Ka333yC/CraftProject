using _Scripts.Core;
using _Scripts.Core.BlocksCore;
using _Scripts.Core.ChunkCore.ChunkLogic.Components;
using _Scripts.Core.ChunkCore.ChunksContainerLogic.Components;
using _Scripts.Core.ChunkCore.ChunksContainerLogic.Components.Elements;
using _Scripts.TempScripts;
using Cysharp.Threading.Tasks;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Scripts.Implementation.PlayerImplementation
{
	// TODO: возможен бесконечный поиск, если мир был уничтожен до того, как позиция найдена. Добавить отмену
	// при уничтожении мира
	public class PlayerSpawnPositionFinder
	{
		private EcsWorld _world;
		private readonly EcsPool<ChunkInitializedTag> _chunkInitializedPool;
		private readonly EcsPool<ChunkComponent> _chunkPool;
		private readonly EcsPool<ChunksContainerComponent> _chunksContainerPool;
		private readonly EcsFilter _chunksContainersFilter;

		private readonly BlocksArchetype _blocksContainers;

		public PlayerSpawnPositionFinder(EcsWorld world, BlocksArchetype blocksContainers)
		{
			_world = world;
			_chunkInitializedPool = world.GetPool<ChunkInitializedTag>();
			_chunkPool = world.GetPool<ChunkComponent>();
			_chunksContainerPool = world.GetPool<ChunksContainerComponent>();
			_chunksContainersFilter = world
				.Filter<ChunksContainerComponent>()
				.End();

			_blocksContainers = blocksContainers;
		}

		public async UniTask<Vector3> FindSpawnPosition() 
		{
			bool isPositionFound = false;
			Vector3Int chunkPositionToCheck = new Vector3Int(0, 0, 0);
			var chunksContainer = await GetChunksContainer();
			while(!isPositionFound)
			{
				chunksContainer.AddChunkUser(chunkPositionToCheck, 0);
				try
				{
					var chunkEntity = await WaitUntilChunkGenerated(chunkPositionToCheck,
						chunksContainer);
					Vector3Int blockPositionToSpawnOn = Vector3Int.zero;
					var hasPositionFound = await UniTask.RunOnThreadPool(
						() => TryFindBlockPositionToSpawnOn(chunkEntity, out blockPositionToSpawnOn));
					if(hasPositionFound)
					{
						var positionToSpawn = blockPositionToSpawnOn + ChunkConstantData.ShiftToBlockCenter;
						// +1, т.к. был найден блок на котором заспавниться, позиция для спавна находится выше блока
						positionToSpawn.y += 1;
						return positionToSpawn;
					}
					else
					{
						chunkPositionToCheck += new Vector3Int(1, 0, 1);
					}
				}
				finally
				{
					chunksContainer.RemoveChunkUser(chunkPositionToCheck, 0);
				}
			}

			return default;
		}

		private async UniTask<ChunksContainer> GetChunksContainer()
		{
			ChunksContainer result = null;
			while(!TryGetChunksContainer(out result))
			{
				await UniTask.Yield();
			}

			return result;
		}

		private async UniTask<int> WaitUntilChunkGenerated(Vector3Int gridPosition, ChunksContainer chunksContainer)
		{
			int chunkEntity;
			while(!chunksContainer.TryGetChunk(gridPosition, out chunkEntity))
			{
				await UniTask.Yield();
			}

			while(!_chunkInitializedPool.Has(chunkEntity))
			{
				await UniTask.Yield();
			}

			return chunkEntity;
		}

		private bool TryFindBlockPositionToSpawnOn(int chunkEntity, out Vector3Int result)
		{
			var random = new System.Random();
			int randomStartXPosition = random.Next(0, ChunkConstantData.ChunkScale.x);
			int randomStarZPosition = random.Next(0, ChunkConstantData.ChunkScale.z);
			var blocks = _chunkPool.Get(chunkEntity).Blocks;
			var blockIdToSpawnOn = Singleton.Instance.BlockToSpawn.Id;
			for(int x = randomStartXPosition; x < ChunkConstantData.ChunkScale.x; x++)
			{
				for(int z = randomStarZPosition; z < ChunkConstantData.ChunkScale.z; z++)
				{
					// Минус 3, т.к. 2 блока над игроком должны быть воздухом
					for(int y = ChunkConstantData.ChunkScale.y - 3; y >= 0; y--)
					{
						if(blocks[x, y, z].Id == blockIdToSpawnOn &&
							blocks[x, y + 1, z] == _blocksContainers.Air &&
							blocks[x, y + 2, z] == _blocksContainers.Air)
						{
							result = new Vector3Int(x, y, z);
							return true;
						}
					}
				}
			}

			result = default;
			return false;
		}

		private bool TryGetChunksContainer(out ChunksContainer result)
		{
			foreach(var entity in _chunksContainersFilter)
			{
				result = _chunksContainerPool.Get(entity).ChunksContainer;
				return true;
			}

			result = default;
			return false;
		}
	}
}
