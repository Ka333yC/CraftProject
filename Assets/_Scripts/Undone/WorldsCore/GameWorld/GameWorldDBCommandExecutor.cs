using Assets._Scripts.Undone.WorldsCore;
using Assets.Scripts.Core.ChunkCore.Saving;
using Assets.Scripts.Core.PlayerCore;
using Assets.Scripts.Core.WorldsCore;
using DataBaseManagement;
using System;
using System.IO;

namespace Assets.Scripts.Undone
{
	public class GameWorldDBCommandExecutor : IDisposable
	{
		public readonly DataBaseCommandExecutor CommandExecutor = new DataBaseCommandExecutor();

		public GameWorldDBCommandExecutor(WorldLauncher worldLauncher)
		{
			var pathToWorldDB = Path.Combine(worldLauncher.WorldParameters.WorldFolderPath,
				SaveFilePathes.GameWorldDatabaseFileName);
			CommandExecutor.OpenConnection(pathToWorldDB);
			//CommandExecutor.ExecuteNonQuery(ChunkInDatabase.CreateTableCommand);
			//CommandExecutor.ExecuteNonQuery(PlayerInDatabase.CreateTableCommand);
		}

		public void Dispose()
		{
			CommandExecutor.Dispose();
		}
	}
}
