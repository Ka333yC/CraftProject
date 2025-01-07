using DataBase.Commands;

namespace _Scripts.Implementation.DataBaseImplementation.WorldsDataDB.Tables.WorldParametersTable.Commands
{
	public class SelectGameWorldParametersCommand : ICommand
	{
		public string Command()
		{
			return $"SELECT {nameof(GameWorldParameters.Id)}, {nameof(GameWorldParameters.Name)}, " +
				$"{nameof(GameWorldParameters.Seed)}, {nameof(GameWorldParameters.WorldFolderPath)} " +
				$"FROM {nameof(GameWorldParameters)}";
		}
	}
}
