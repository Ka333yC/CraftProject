using Assets.Scripts.Core.ChunkCore.Saving.ChunkInDatabaseCommands;
using Assets.Scripts.Core.ChunkCore.Saving.ChunkSerialization.ChunkInDatabaseCommands;
using UnityEngine;

namespace Assets.Scripts.Core.ChunkCore.Saving
{
	// TODO: �������� dimension
	public class ChunkInDatabase
	{
		public int? Id { get; set; }

		public int GridPositionX { get; set; }

		public int GridPositionZ { get; set; }

		public string SerializedBlocks { get; set; }

		public static CreateChunkInDatabaseCommand CreateTableCommand
		{
			get
			{
				return new CreateChunkInDatabaseCommand();
			}
		}

		public static SelectChunkInDatabaseWhereGridPositionCommand SelectWhereGridPositionCommand
		{
			get
			{
				return new SelectChunkInDatabaseWhereGridPositionCommand();
			}
		}

		public static SelectChunkIdInDatabaseWhereGridPositionCommand SelectIdWhereGridPositionCommand
		{
			get
			{
				return new SelectChunkIdInDatabaseWhereGridPositionCommand();
			}
		}

		public static InsertOrReplaceChunkInDatabaseCommand InsertOrReplaceCommand
		{
			get
			{
				return new InsertOrReplaceChunkInDatabaseCommand();
			}
		}

		public void SetGridPosition(Vector3Int gridPosition)
		{
			GridPositionX = gridPosition.x;
			GridPositionZ = gridPosition.z;
		}
	}
}
