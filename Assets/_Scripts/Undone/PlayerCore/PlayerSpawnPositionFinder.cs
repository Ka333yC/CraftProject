using System;
using System.Threading;
using System.Threading.Tasks;
using Assets._Scripts.Core.BlocksCore;
using Assets.Scripts.Core.ChunkCore.LifeTimeControl.Components;
using ChunkCore;
using ChunkCore.ChunksContainerScripts;
using ChunkCore.ChunksContainerScripts.Components;
using ChunkCore.Loading.Components;
using Leopotam.EcsLite;
using TempScripts;
using UnityEngine;

namespace Assets.Scripts.Core.PlayerCore.FindCoordinatesToSpawn
{
	public class PlayerSpawnPositionFinder
	{
		private EcsPool<ChunkInitializedTag> _chunkInitializedPool;
		private EcsPool<ChunkComponent> _chunkPool;
		private EcsPool<ChunksContainerComponent> _chunksContainerPool;
		private EcsFilter _chunksContainerFilter;

		private BlocksContainers _blocksContainers;

		public PlayerSpawnPositionFinder(EcsWorld world, BlocksContainers blocksContainers)
		{
			_chunkInitializedPool = world.GetPool<ChunkInitializedTag>();
			_chunkPool = world.GetPool<ChunkComponent>();
			_chunksContainerPool = world.GetPool<ChunksContainerComponent>();
			_chunksContainerFilter = world
				.Filter<ChunksContainerComponent>()
				.End();

			_blocksContainers = blocksContainers;
		}

		public async Task<Vector3> FindSpawnPosition(CancellationToken token) 
		{
			bool isPositionFound = false;
			Vector3Int chunkPositionToCheck = new Vector3Int(0, 0, 0);
			var chunksContainer = GetChunksContainer();
			while(!isPositionFound)
			{
				chunksContainer.AddChunkUser(chunkPositionToCheck, 0);
				var chunkEntity = await WaitUntilChunkGenerated(chunkPositionToCheck,
					chunksContainer,  token);
				var blockPositionToSpawnOn = await Task.Run(() =>
				{
					return FindBlockPositionToSpawnOn(chunkEntity, token); 
				});

				chunksContainer.RemoveChunkUser(chunkPositionToCheck, 0);
				if(blockPositionToSpawnOn.HasValue)
				{
					var positionToSpawn = blockPositionToSpawnOn.Value + ChunkConstantData.ShiftToBlockCenter;
					// +1, т.к. был найден блок на котором заспавниться, позиция для спавна находится выше блока
					positionToSpawn.y += 1;
					return positionToSpawn;
				}
				else
				{
					chunkPositionToCheck += new Vector3Int(1, 0, 1);
				}
			}

			return default;
		}

		private async Task<int> WaitUntilChunkGenerated(Vector3Int gridPosition, ChunksContainer chunksContainer,
			CancellationToken token)
		{
			int chunkEntity;
			while(!chunksContainer.TryGetChunk(gridPosition, out chunkEntity))
			{
				await Task.Yield();
				token.ThrowIfCancellationRequested();
			}

			while(!_chunkInitializedPool.Has(chunkEntity))
			{
				await Task.Yield();
				token.ThrowIfCancellationRequested();
			}

			return chunkEntity;
		}

		private Vector3Int? FindBlockPositionToSpawnOn(int chunkEntity, CancellationToken token)
		{
			var random = new System.Random();
			int randomStartXPosition = random.Next(0, ChunkConstantData.ChunkScale.x);
			int randomStarZPosition = random.Next(0, ChunkConstantData.ChunkScale.z);
			var blocks = _chunkPool.Get(chunkEntity).Blocks;
			var blockIdToSpawnOn = Singleton.Instance.BlockToSpawn.Id;
			for(int x = randomStartXPosition; x < ChunkConstantData.ChunkScale.x; x++)
			{
				token.ThrowIfCancellationRequested();
				for(int z = randomStarZPosition; z < ChunkConstantData.ChunkScale.z; z++)
				{
					// Минус 3, т.к. 2 блока над игроком должны быть воздухом
					for(int y = ChunkConstantData.ChunkScale.y - 3; y >= 0; y--)
					{
						if(blocks[x, y, z].Id == blockIdToSpawnOn &&
							blocks[x, y + 1, z] == _blocksContainers.Air &&
							blocks[x, y + 2, z] == _blocksContainers.Air)
						{
							return new Vector3Int(x, y, z);
						}
					}
				}
			}

			return null;
		}

		private ChunksContainer GetChunksContainer()
		{
			foreach(var entity in _chunksContainerFilter)
			{
				return _chunksContainerPool.Get(entity).ChunksContainer;
			}

			throw new Exception($"{typeof(ChunksContainerComponent).Name} not found");
		}
	}
}
