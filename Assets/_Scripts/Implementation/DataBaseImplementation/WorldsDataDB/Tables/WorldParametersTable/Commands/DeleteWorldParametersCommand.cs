using DataBase.Commands;

namespace _Scripts.Implementation.DataBaseImplementation.WorldsDataDB.Tables.WorldParametersTable.Commands
{
	public class DeleteWorldParametersCommand : ICommand
	{
		public int Id;

		public string Command()
		{
			return $"DELETE FROM {nameof(WorldParameters)} " +
				$"WHERE {nameof(WorldParameters.Id)} = {Id}";
		}
	}
}
