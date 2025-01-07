using DataBase.Commands;

namespace _Scripts.Implementation.DataBaseImplementation.WorldsDataDB.Tables.WorldParametersTable.Commands
{
	public class InsertOrReplaceWorldParametersCommand : ICommand
	{
		public WorldParameters Value;

		public string Command()
		{
			string worldIdColumnName = "";
			string worldIdValue = "";
			if(Value.Id.HasValue)
			{
				worldIdColumnName = $"{nameof(WorldParameters.Id)}, ";
				worldIdValue = $"{Value.Id}, ";
			}

			return $"INSERT OR REPLACE INTO {nameof(WorldParameters)}" +
				$"({worldIdColumnName}" +
				$"{nameof(WorldParameters.Name)}, " +
				$"{nameof(WorldParameters.Seed)}, " +
				$"{nameof(WorldParameters.WorldFolderPath)}) " +
				$"VALUES({worldIdValue}" +
				$"'{Value.Name}', " +
				$"{Value.Seed}, " +
				$"'{Value.WorldFolderPath}')";
		}
	}
}
