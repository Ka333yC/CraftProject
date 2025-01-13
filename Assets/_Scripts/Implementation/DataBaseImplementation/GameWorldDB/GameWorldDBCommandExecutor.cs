using System;
using System.IO;
using _Scripts.Implementation.DataBaseImplementation.GameWorldDB.Tables.ChunkInDatabaseTable;
using _Scripts.Implementation.DataBaseImplementation.GameWorldDB.Tables.PlayerInDatabaseTable;
using _Scripts.Implementation.DataBaseImplementation.GameWorldsDataDB.Tables.GameWorldParametersTable;
using Cysharp.Threading.Tasks;
using DataBaseManagement;

namespace _Scripts.Implementation.DataBaseImplementation.GameWorldDB
{
	public class GameWorldDBCommandExecutor : IDisposable
	{
		public readonly DataBaseCommandExecutor CommandExecutor = new DataBaseCommandExecutor();

		public async UniTask Initialize(GameWorldParameters worldParameters)
		{
			var pathToDataBase = Path.Combine(worldParameters.WorldFolderPath,
				SaveFilePathes.GameWorldDatabaseFileName);
			await CommandExecutor.OpenConnectionAsync(pathToDataBase);
			await CommandExecutor.ExecuteNonQueryAsync(ChunkInDatabase.CreateTableCommand);
			await CommandExecutor.ExecuteNonQueryAsync(PlayerInDatabase.CreateTableCommand);
		}

		public void Dispose()
		{
			CommandExecutor.Dispose();
		}
	}
}
