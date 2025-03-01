using DataBase.Commands;

namespace _Scripts.Implementation.DataBaseImplementation.GameWorldDB.Tables.ChunkInDatabaseTable.Commands
{
	public class InsertOrReplaceChunkInDatabaseCommand : ICommand
	{
		public ChunkInDatabase Chunk;

		public string Command()
		{
			string chunkIdColumnName = "";
			string chunkIdValue = "";
			if(Chunk.Id.HasValue)
			{
				chunkIdColumnName = $"{nameof(ChunkInDatabase.Id)}, ";
				chunkIdValue = $"{Chunk.Id}, ";
			}

			return $"INSERT OR REPLACE INTO {nameof(ChunkInDatabase)}" +
				$"({chunkIdColumnName}" +
				$"{nameof(ChunkInDatabase.GridPositionX)}, " +
				$"{nameof(ChunkInDatabase.GridPositionZ)}, " +
				$"{nameof(ChunkInDatabase.SerializedBlocks)}) " +
				$"VALUES({chunkIdValue}" +
				$"{Chunk.GridPositionX}, " +
				$"{Chunk.GridPositionZ}, " +
				$"'{Chunk.SerializedBlocks}')";
		}
	}
}
