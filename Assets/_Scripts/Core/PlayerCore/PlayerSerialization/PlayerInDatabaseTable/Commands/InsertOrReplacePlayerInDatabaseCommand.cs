using Assets.Scripts.Core.ChunkCore.Saving;
using DataBase.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Core.PlayerCore.Saving.PlayerInDatabseCommands
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
