using _Scripts.Implementation.DataBaseImplementation.GameWorldsDataDB.Tables.GameWorldParametersTable.Commands;

namespace _Scripts.Implementation.DataBaseImplementation.GameWorldsDataDB.Tables.GameWorldParametersTable
{
	public class GameWorldParameters
	{
		public int? Id { get; set; }
		public string Name { get; set; }
		public int Seed { get; set; }
		public string WorldFolderPath { get; set; }

		public static CreateGameWorldParametersCommand CreateTableCommand => new();
		public static InsertOrReplaceGameWorldParametersCommand InsertOrReplaceCommand => new();
		public static SelectGameWorldParametersCommand SelectCommand => new();
		public static SelectGameWorldParametersWhereIdCommand SelectWhereIdCommand => new();
		public static ExistsGameWorldParametersWhereNameCommand ExistsWhereNameCommand => new();
		public static DeleteGameWorldParametersCommand DeleteCommand => new();
	}
}
