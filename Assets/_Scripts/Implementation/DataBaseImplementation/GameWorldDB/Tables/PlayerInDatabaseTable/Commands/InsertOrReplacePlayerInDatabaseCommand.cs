using DataBase.Commands;

namespace _Scripts.Implementation.DataBaseImplementation.GameWorldDB.Tables.PlayerInDatabaseTable.Commands
{
	public class InsertOrReplacePlayerInDatabaseCommand : ICommand
	{
		public PlayerInDatabase Player;

		public string Command()
		{
			return $"INSERT OR REPLACE INTO {nameof(PlayerInDatabase)}" +
				$"({nameof(PlayerInDatabase.Nickname)}, " +
				$"{nameof(PlayerInDatabase.PositionX)}, " +
				$"{nameof(PlayerInDatabase.PositionY)}, " +
				$"{nameof(PlayerInDatabase.PositionZ)}) " +
				$"VALUES('{Player.Nickname}', " +
				$"{Player.PositionX.ToString().Replace(',', '.')}, " +
				$"{Player.PositionY.ToString().Replace(',', '.')}, " +
				$"{Player.PositionZ.ToString().Replace(',', '.')})";
		}
	}
}
