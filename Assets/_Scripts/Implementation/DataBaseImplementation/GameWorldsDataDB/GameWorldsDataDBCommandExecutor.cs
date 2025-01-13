using System;
using _Scripts.Implementation.DataBaseImplementation.GameWorldsDataDB.Tables.GameWorldParametersTable;
using DataBaseManagement;

namespace _Scripts.Implementation.DataBaseImplementation.GameWorldsDataDB
{
	public class GameWorldsDBCommandExecutor : IDisposable
	{
		public readonly DataBaseCommandExecutor CommandExecutor = new DataBaseCommandExecutor();

		public GameWorldsDBCommandExecutor()
		{
			CommandExecutor.OpenConnection(SaveFilePathes.PathToGameWorldsDBFile);
			CommandExecutor.ExecuteNonQuery(GameWorldParameters.CreateTableCommand);
		}

		public void Dispose()
		{
			CommandExecutor.Dispose();
		}
	}
}
