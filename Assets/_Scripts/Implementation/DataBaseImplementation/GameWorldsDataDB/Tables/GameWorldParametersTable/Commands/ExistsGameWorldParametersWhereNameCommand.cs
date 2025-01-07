using DataBase.Commands;

namespace _Scripts.Implementation.DataBaseImplementation.WorldsDataDB.Tables.WorldParametersTable.Commands
{
	public class ExistsGameWorldParametersWhereNameCommand : ICommand
	{
		public string Name;

		public string Command()
		{
			var subRequest = $"SELECT * " +
				$"FROM {nameof(GameWorldParameters)} " +
				$"WHERE {nameof(GameWorldParameters.Name)} = '{Name}'";
			return $"SELECT EXISTS({subRequest})";
		}
	}
}
