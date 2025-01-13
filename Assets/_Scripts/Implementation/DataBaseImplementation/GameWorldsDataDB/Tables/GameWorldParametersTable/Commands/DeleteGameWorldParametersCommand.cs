using DataBase.Commands;

namespace _Scripts.Implementation.DataBaseImplementation.GameWorldsDataDB.Tables.GameWorldParametersTable.Commands
{
	public class DeleteGameWorldParametersCommand : ICommand
	{
		public int Id;

		public string Command()
		{
			return $"DELETE FROM {nameof(GameWorldParameters)} " +
				$"WHERE {nameof(GameWorldParameters.Id)} = {Id}";
		}
	}
}
