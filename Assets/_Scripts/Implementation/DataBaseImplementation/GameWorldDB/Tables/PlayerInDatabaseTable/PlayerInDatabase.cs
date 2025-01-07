using _Scripts.Implementation.DataBaseImplementation.GameWorldDB.Tables.PlayerInDatabaseTable.Commands;
using UnityEngine;

namespace _Scripts.Implementation.DataBaseImplementation.GameWorldDB.Tables.PlayerInDatabaseTable
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
