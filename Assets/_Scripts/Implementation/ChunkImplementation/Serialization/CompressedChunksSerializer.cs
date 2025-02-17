using System.Threading;
using System.Threading.Tasks;
using _Scripts.Core;
using _Scripts.Core.BlocksCore;
using _Scripts.Core.ChunkCore.ChunkLogic.ChunkSerialization.Elements;
using _Scripts.Core.ChunkCore.ChunkLogic.Components;
using _Scripts.Core.ChunkCore.ChunkLogic.Components.Elements;
using _Scripts.Implementation.DataBaseImplementation.GameWorldDB;
using _Scripts.Implementation.DataBaseImplementation.GameWorldDB.Tables.ChunkInDatabaseTable;
using Cysharp.Threading.Tasks;
using DataBaseManagement;
using Leopotam.EcsLite;
using Newtonsoft.Json;
using UnityEngine;

namespace _Scripts.Implementation.ChunkImplementation.Serialization
{
	public class CompressedChunksSerializer : ChunkSerializer
	{
		private readonly EcsPool<ChunkComponent> _chunksPool;

		private readonly DataBaseCommandExecutor _commandExecutor;
		private readonly BlocksArchetypes _blocksArchetypes;

		public CompressedChunksSerializer(EcsWorld world, GameWorldDBCommandExecutor commandExecutor,
			BlocksArchetypes blocksArchetypes)
		{
			_chunksPool = world.GetPool<ChunkComponent>();

			_commandExecutor = commandExecutor.CommandExecutor;
			_blocksArchetypes = blocksArchetypes;
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

		private async UniTask WriteToDatabase(ChunkInDatabase chunkInDatabase) 
		{
			var insertOrReplaceCommand = ChunkInDatabase.InsertOrReplaceCommand;
			insertOrReplaceCommand.Chunk = chunkInDatabase;
			await _commandExecutor.ExecuteNonQueryAsync(insertOrReplaceCommand);
		}

		private async UniTask<int?> GetChunkIdInDatabase(Vector3Int gridPosition)
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
			});

			return result;
		}

		private async UniTask<ChunkInDatabase> GetChunkInDatabase(Vector3Int gridPosition)
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
			});

			return result;
		}

		private string SerializeBlocks(ChunkSizeBlocks blocks)
		{
			var compressedBlocks = new CompressedBlocks(_blocksArchetypes);
			for(int y = 0; y < ChunkConstantData.ChunkScale.y; y++)
			{
				for(int x = 0; x < ChunkConstantData.ChunkScale.x; x++)
				{
					for(int z = 0; z < ChunkConstantData.ChunkScale.z; z++)
					{
						compressedBlocks.AddLast(blocks[x, y, z]);
					}
				}
			}

			return JsonConvert.SerializeObject(compressedBlocks);
		}

		private void PopulateBlocks(ChunkSizeBlocks blocks, string serializedBlocks, CancellationToken token)
		{
			var сompressedBlocks = new CompressedBlocks(_blocksArchetypes);
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
