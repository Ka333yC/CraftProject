using DataBase.Commands;

namespace _Scripts.Implementation.DataBaseImplementation.WorldsDataDB.Tables.WorldParametersTable.Commands
{
	public class SelectWorldParametersCommand : ICommand
	{
		public string Command()
		{
			return $"SELECT {nameof(WorldParameters.Id)}, {nameof(WorldParameters.Name)}, " +
				$"{nameof(WorldParameters.Seed)}, {nameof(WorldParameters.WorldFolderPath)} " +
				$"FROM {nameof(WorldParameters)}";
		}
	}
}
