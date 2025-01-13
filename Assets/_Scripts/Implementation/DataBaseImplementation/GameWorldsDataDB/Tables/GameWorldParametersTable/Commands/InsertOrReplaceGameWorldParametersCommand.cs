using DataBase.Commands;

namespace _Scripts.Implementation.DataBaseImplementation.GameWorldsDataDB.Tables.GameWorldParametersTable.Commands
{
	public class InsertOrReplaceGameWorldParametersCommand : ICommand
	{
		public GameWorldParameters Value;

		public string Command()
		{
			string worldIdColumnName = "";
			string worldIdValue = "";
			if(Value.Id.HasValue)
			{
				worldIdColumnName = $"{nameof(GameWorldParameters.Id)}, ";
				worldIdValue = $"{Value.Id}, ";
			}

			return $"INSERT OR REPLACE INTO {nameof(GameWorldParameters)}" +
				$"({worldIdColumnName}" +
				$"{nameof(GameWorldParameters.Name)}, " +
				$"{nameof(GameWorldParameters.Seed)}, " +
				$"{nameof(GameWorldParameters.WorldFolderPath)}) " +
				$"VALUES({worldIdValue}" +
				$"'{Value.Name}', " +
				$"{Value.Seed}, " +
				$"'{Value.WorldFolderPath}')";
		}
	}
}
