using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Assets._Scripts.Core.BlocksCore;
using Assets.Scripts.Core.ChunkCore.ChunkLogic.Components.Elements;
using Assets.Scripts.Core.ChunkCore.ChunkLogic.Loading.Saving.Components;
using Assets.Scripts.Core.ChunkCore.LifeTimeControl.Components;
using Assets.Scripts.Core.ChunkCore.Saving;
using Assets.Scripts.Core.ChunkCore.Saving.Components;
using Assets.Scripts.Undone;
using ChunkCore;
using Cysharp.Threading.Tasks;
using DataBaseManagement;
using Leopotam.EcsLite;
using Newtonsoft.Json;
using TempScripts;
using UnityEngine;

namespace Assets.Scripts.Core.DataSave
{
	public class CompressedChunksSerializer : ChunkSerializer
	{
		private readonly EcsPool<ChunkComponent> _chunksPool;

		private readonly DataBaseCommandExecutor _commandExecutor;
		private readonly BlocksContainers _blocksContainers;

		public CompressedChunksSerializer(EcsWorld world, GameWorldDBCommandExecutor commandExecutor,
			BlocksContainers blocksContainers)
		{
			_chunksPool = world.GetPool<ChunkComponent>();

			_commandExecutor = commandExecutor.CommandExecutor;
			_blocksContainers = blocksContainers;
		}

		public override async UniTask Save(int chunkEntity) 
		{
			var chunk = _chunksPool.Get(chunkEntity);
			var chunkId = await GetChunkIdInDatabase(chunk.GridPosition);
			var serializedBlocks = await UniTask.RunOnThreadPool(() => SerializeBlocks(chunk.Blocks));
			var chunkInDatabase = new ChunkInDatabase();
			chunkInDatabase.Id = chunkId;
			chunkInDatabase.SetGridPosition(chunk.GridPosition);
			chunkInDatabase.SerializedBlocks = serializedBlocks;
			await WriteToDatabase(chunkInDatabase);
		}

		/// <returns>Возвращает true, если чанк был заполнен</returns>
		public override async UniTask<bool> Populate(int chunkEntity, CancellationToken token)
		{
			var chunk = _chunksPool.Get(chunkEntity);
			var chunkInDatabase = await GetChunkInDatabase(chunk.GridPosition);
			if(chunkInDatabase == null)
			{
				return false;
			}

			await UniTask.RunOnThreadPool(() => PopulateBlocks(chunk.Blocks, 
				chunkInDatabase.SerializedBlocks, token), cancellationToken: token);
			return true;
		}

		private async Task WriteToDatabase(ChunkInDatabase chunkInDatabase) 
		{
			var insertOrReplaceCommand = ChunkInDatabase.InsertOrReplaceCommand;
			insertOrReplaceCommand.Chunk = chunkInDatabase;
			await _commandExecutor.ExecuteNonQueryAsync(insertOrReplaceCommand);
		}

		private async Task<int?> GetChunkIdInDatabase(Vector3Int gridPosition)
		{
			int? result = null;
			var selectChunkIdCommand =
				ChunkInDatabase.SelectIdWhereGridPositionCommand;
			selectChunkIdCommand.GridPosition = gridPosition;
			await _commandExecutor.ExecuteReaderAsync(selectChunkIdCommand, (reader) =>
			{
				if(!reader.Read())
				{
					return;
				}

				result = reader.GetInt32(0);
			}).ConfigureAwait(false);

			return result;
		}

		private async Task<ChunkInDatabase> GetChunkInDatabase(Vector3Int gridPosition)
		{
			ChunkInDatabase result = null;
			var selectChunkCommand =
				ChunkInDatabase.SelectWhereGridPositionCommand;
			selectChunkCommand.GridPosition = gridPosition;
			await _commandExecutor.ExecuteReaderAsync(selectChunkCommand, (reader) =>
			{
				if(!reader.Read())
				{
					return;
				}

				result = new ChunkInDatabase();
				result.Id = reader.GetInt32(0);
				result.SetGridPosition(gridPosition);
				result.SerializedBlocks = reader.GetString(1);
			}).ConfigureAwait(false);

			return result;
		}

		private string SerializeBlocks(ChunkSizeBlocks blocks)
		{
			var сompressedBlocks = new CompressedBlocks(_blocksContainers);
			for(int y = 0; y < ChunkConstantData.ChunkScale.y; y++)
			{
				for(int x = 0; x < ChunkConstantData.ChunkScale.x; x++)
				{
					for(int z = 0; z < ChunkConstantData.ChunkScale.z; z++)
					{
						сompressedBlocks.AddLast(blocks[x, y, z]);
					}
				}
			}

			return JsonConvert.SerializeObject(сompressedBlocks);
		}

		private void PopulateBlocks(ChunkSizeBlocks blocks, string serializedBlocks, CancellationToken token)
		{
			var сompressedBlocks = new CompressedBlocks(_blocksContainers);
			JsonConvert.PopulateObject(serializedBlocks, сompressedBlocks);
			for(int y = ChunkConstantData.ChunkScale.y - 1; y >= 0; y--)
			{
				token.ThrowIfCancellationRequested();
				for(int x = ChunkConstantData.ChunkScale.x - 1; x >= 0; x--)
				{
					for(int z = ChunkConstantData.ChunkScale.z - 1; z >= 0; z--)
					{
						blocks[x, y, z] = сompressedBlocks.PopLast();
					}
				}
			}
		}
	}
}
