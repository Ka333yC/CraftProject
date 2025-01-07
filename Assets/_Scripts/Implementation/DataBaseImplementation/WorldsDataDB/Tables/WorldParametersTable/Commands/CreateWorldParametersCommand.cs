using DataBase.Commands;

namespace _Scripts.Implementation.DataBaseImplementation.WorldsDataDB.Tables.WorldParametersTable.Commands
{
	public class CreateWorldParametersCommand : ICommand
	{
		public string Command()
		{
			return $"CREATE TABLE IF NOT EXISTS {nameof(WorldParameters)}" +
				$"({nameof(WorldParameters.Id)} INTEGER PRIMARY KEY AUTOINCREMENT," +
				$"{nameof(WorldParameters.Name)} TEXT NOT NULL," +
				$"{nameof(WorldParameters.Seed)} INTEGER NOT NULL," +
				$"{nameof(WorldParameters.WorldFolderPath)} TEXT NOT NULL)";
		}
	}
}
