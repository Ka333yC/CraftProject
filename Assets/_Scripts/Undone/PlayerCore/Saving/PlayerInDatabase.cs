using Assets.Scripts.Core.ChunkCore.Saving.ChunkInDatabaseCommands;
using Assets.Scripts.Core.PlayerCore.Saving.PlayerInDatabseCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Core.PlayerCore
{
	public class PlayerInDatabase
	{
		public string Nickname;
		public float PositionX;
		public float PositionY;
		public float PositionZ;

		public static CreatePlayerInDatabaseCommand CreateTableCommand
		{
			get
			{
				return new CreatePlayerInDatabaseCommand();
			}
		}

		public static SelectPlayerInDatabaseWhereNicknameCommand SelectWhereNicknameCommand
		{
			get
			{
				return new SelectPlayerInDatabaseWhereNicknameCommand();
			}
		}

		public static InsertOrReplacePlayerInDatabaseCommand InsertOrReplaceCommand
		{
			get
			{
				return new InsertOrReplacePlayerInDatabaseCommand();
			}
		}

		public void SetPosition(Vector3 position)
		{
			PositionX = position.x;
			PositionY = position.y;
			PositionZ = position.z;
		}
	}
}
