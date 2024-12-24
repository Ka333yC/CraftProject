using ChunkCore.LifeTimeControl;
using DataBase.Commands;
using System;

namespace Assets.Scripts.Core.ChunkCore.Saving.ChunkInDatabaseCommands
{
	public class CreateChunkInDatabaseCommand : ICommand
	{
		public string Command()
		{
			return $"CREATE TABLE IF NOT EXISTS {nameof(ChunkInDatabase)}" +
				$"({nameof(ChunkInDatabase.Id)} INTEGER PRIMARY KEY AUTOINCREMENT," +
				$"{nameof(ChunkInDatabase.GridPositionX)} INTEGER," +
				$"{nameof(ChunkInDatabase.GridPositionZ)} INTEGER," +
				$"{nameof(ChunkInDatabase.SerializedBlocks)} TEXT NOT NULL," +
				$"UNIQUE ({nameof(ChunkInDatabase.GridPositionX)}, {nameof(ChunkInDatabase.GridPositionZ)}))";
		}
	}
}
