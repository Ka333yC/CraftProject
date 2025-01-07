using DataBase.Commands;

namespace _Scripts.Implementation.DataBaseImplementation.WorldsDataDB.Tables.WorldParametersTable.Commands
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
