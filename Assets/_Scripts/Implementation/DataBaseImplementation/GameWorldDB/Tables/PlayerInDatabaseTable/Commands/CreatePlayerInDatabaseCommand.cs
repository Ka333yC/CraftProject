using DataBase.Commands;

namespace _Scripts.Implementation.DataBaseImplementation.GameWorldDB.Tables.PlayerInDatabaseTable.Commands
{
	public class CreatePlayerInDatabaseCommand : ICommand
	{
		public string Command()
		{
			return $"CREATE TABLE IF NOT EXISTS {nameof(PlayerInDatabase)}" +
				$"({nameof(PlayerInDatabase.Nickname)} TEXT PRIMARY KEY," +
				$"{nameof(PlayerInDatabase.PositionX)} REAL NOT NULL," +
				$"{nameof(PlayerInDatabase.PositionY)} REAL NOT NULL," +
				$"{nameof(PlayerInDatabase.PositionZ)} REAL NOT NULL)";
		}
	}
}
