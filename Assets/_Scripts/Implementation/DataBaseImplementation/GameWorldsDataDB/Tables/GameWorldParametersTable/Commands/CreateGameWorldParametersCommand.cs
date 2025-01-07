using DataBase.Commands;

namespace _Scripts.Implementation.DataBaseImplementation.WorldsDataDB.Tables.WorldParametersTable.Commands
{
	public class CreateGameWorldParametersCommand : ICommand
	{
		public string Command()
		{
			return $"CREATE TABLE IF NOT EXISTS {nameof(GameWorldParameters)}" +
				$"({nameof(GameWorldParameters.Id)} INTEGER PRIMARY KEY AUTOINCREMENT," +
				$"{nameof(GameWorldParameters.Name)} TEXT NOT NULL," +
				$"{nameof(GameWorldParameters.Seed)} INTEGER NOT NULL," +
				$"{nameof(GameWorldParameters.WorldFolderPath)} TEXT NOT NULL)";
		}
	}
}
