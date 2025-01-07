using System;
using System.IO;
using _Scripts.Implementation.DataBaseImplementation.GameWorldDB.Tables.ChunkInDatabaseTable;
using _Scripts.Implementation.DataBaseImplementation.GameWorldDB.Tables.PlayerInDatabaseTable;
using _Scripts.Undone.WorldsCore;
using DataBaseManagement;

namespace _Scripts.Implementation.DataBaseImplementation.GameWorldDB
{
	public class GameWorldDBCommandExecutor : IDisposable
	{
		public readonly DataBaseCommandExecutor CommandExecutor = new DataBaseCommandExecutor();

		public GameWorldDBCommandExecutor(WorldLauncher worldLauncher)
		{
			var pathToWorldDB = Path.Combine(worldLauncher.WorldParameters.WorldFolderPath,
				SaveFilePathes.GameWorldDatabaseFileName);
			CommandExecutor.OpenConnection(pathToWorldDB);
			CommandExecutor.ExecuteNonQuery(ChunkInDatabase.CreateTableCommand);
			CommandExecutor.ExecuteNonQuery(PlayerInDatabase.CreateTableCommand);
		}

		public void Dispose()
		{
			CommandExecutor.Dispose();
		}
	}
}
