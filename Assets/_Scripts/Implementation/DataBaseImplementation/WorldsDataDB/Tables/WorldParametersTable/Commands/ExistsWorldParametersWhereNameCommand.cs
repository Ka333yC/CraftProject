using DataBase.Commands;

namespace _Scripts.Implementation.DataBaseImplementation.WorldsDataDB.Tables.WorldParametersTable.Commands
{
	public class ExistsWorldParametersWhereNameCommand : ICommand
	{
		public string Name;

		public string Command()
		{
			var subRequest = $"SELECT * " +
				$"FROM {nameof(WorldParameters)} " +
				$"WHERE {nameof(WorldParameters.Name)} = '{Name}'";
			return $"SELECT EXISTS({subRequest})";
		}
	}
}
