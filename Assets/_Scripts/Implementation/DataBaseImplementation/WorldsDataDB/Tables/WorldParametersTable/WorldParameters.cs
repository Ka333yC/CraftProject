using _Scripts.Implementation.DataBaseImplementation.WorldsDataDB.Tables.WorldParametersTable.Commands;

namespace _Scripts.Implementation.DataBaseImplementation.WorldsDataDB.Tables.WorldParametersTable
{
	public class WorldParameters
	{
		public int? Id { get; set; }

		public string Name { get; set; }

		public int Seed { get; set; }

		public string WorldFolderPath { get; set; }

		public static CreateWorldParametersCommand CreateTableCommand
		{
			get
			{
				return new CreateWorldParametersCommand();
			}
		}

		public static InsertOrReplaceWorldParametersCommand InsertOrReplaceCommand
		{
			get
			{
				return new InsertOrReplaceWorldParametersCommand();
			}
		}

		public static SelectWorldParametersCommand SelectCommand
		{
			get
			{
				return new SelectWorldParametersCommand();
			}
		}

		public static SelectWorldParametersWhereIdCommand SelectWhereIdCommand
		{
			get
			{
				return new SelectWorldParametersWhereIdCommand();
			}
		}

		public static ExistsWorldParametersWhereNameCommand ExistsWhereNameCommand
		{
			get
			{
				return new ExistsWorldParametersWhereNameCommand();
			}
		}

		public static DeleteWorldParametersCommand DeleteCommand
		{
			get
			{
				return new DeleteWorldParametersCommand();
			}
		}
	}
}
