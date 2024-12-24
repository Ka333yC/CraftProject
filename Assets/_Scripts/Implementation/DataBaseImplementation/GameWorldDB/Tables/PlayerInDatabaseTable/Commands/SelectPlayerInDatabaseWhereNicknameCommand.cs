using Assets.Scripts.Core.ChunkCore.Saving;
using DataBase.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Core.PlayerCore.Saving.PlayerInDatabseCommands
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
