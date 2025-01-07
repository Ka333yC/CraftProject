using DataBase.Commands;
using UnityEngine;

namespace _Scripts.Implementation.DataBaseImplementation.GameWorldDB.Tables.ChunkInDatabaseTable.Commands
{
	public class SelectChunkInDatabaseWhereGridPositionCommand : ICommand
	{
		public Vector3Int GridPosition;

		public string Command()
		{
			return $"SELECT {nameof(ChunkInDatabase.Id)}, {nameof(ChunkInDatabase.SerializedBlocks)} " +
				$"FROM {nameof(ChunkInDatabase)} " +
				$"WHERE {nameof(ChunkInDatabase.GridPositionX)} = {GridPosition.x} AND " +
				$"{nameof(ChunkInDatabase.GridPositionZ)} = {GridPosition.z}";
		}
	}
}
