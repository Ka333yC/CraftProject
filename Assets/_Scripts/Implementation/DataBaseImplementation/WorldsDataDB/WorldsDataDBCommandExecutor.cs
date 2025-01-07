using System;
using _Scripts.Implementation.DataBaseImplementation.WorldsDataDB.Tables.WorldParametersTable;
using DataBaseManagement;

namespace _Scripts.Implementation.DataBaseImplementation.WorldsDataDB
{
	public class WorldsDataDBCommandExecutor : IDisposable
	{
		public readonly DataBaseCommandExecutor CommandExecutor = new DataBaseCommandExecutor();

		public WorldsDataDBCommandExecutor()
		{
			CommandExecutor.OpenConnection(SaveFilePathes.PathToWorldsDataDBFile);
			CommandExecutor.ExecuteNonQuery(WorldParameters.CreateTableCommand);
		}

		public void Dispose()
		{
			CommandExecutor.Dispose();
		}
	}
}
