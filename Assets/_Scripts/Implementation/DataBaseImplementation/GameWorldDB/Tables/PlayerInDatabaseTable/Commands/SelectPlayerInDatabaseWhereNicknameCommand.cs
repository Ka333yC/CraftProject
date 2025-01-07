using DataBase.Commands;

namespace _Scripts.Implementation.DataBaseImplementation.GameWorldDB.Tables.PlayerInDatabaseTable.Commands
{
	public class SelectPlayerInDatabaseWhereNicknameCommand : ICommand
	{
		public string Nickname;

		public string Command()
		{
			return $"SELECT {nameof(PlayerInDatabase.PositionX)}, {nameof(PlayerInDatabase.PositionY)}, " +
				$"{nameof(PlayerInDatabase.PositionZ)} " +
				$"FROM {nameof(PlayerInDatabase)} " +
				$"WHERE {nameof(PlayerInDatabase.Nickname)} = '{Nickname}'";
		}
	}
}
