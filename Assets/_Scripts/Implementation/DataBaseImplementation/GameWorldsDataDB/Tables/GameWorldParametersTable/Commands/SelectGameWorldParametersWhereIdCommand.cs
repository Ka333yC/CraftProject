using DataBase.Commands;

namespace _Scripts.Implementation.DataBaseImplementation.WorldsDataDB.Tables.WorldParametersTable.Commands
{
	public class SelectGameWorldParametersWhereIdCommand : ICommand
	{
		public int Id;

		public string Command()
		{
			return $"SELECT {nameof(GameWorldParameters.Name)}, {nameof(GameWorldParameters.Seed)}, " +
				$"{nameof(GameWorldParameters.WorldFolderPath)} " +
				$"FROM {nameof(GameWorldParameters)} " +
				$"WHERE {nameof(GameWorldParameters.Id)} = {Id}";
		}
	}
}
