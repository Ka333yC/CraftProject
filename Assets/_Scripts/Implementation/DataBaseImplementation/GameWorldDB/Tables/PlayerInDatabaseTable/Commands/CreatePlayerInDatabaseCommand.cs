using Assets.Scripts.Core.ChunkCore.Saving;
using DataBase.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Core.PlayerCore.Saving.PlayerInDatabseCommands
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
