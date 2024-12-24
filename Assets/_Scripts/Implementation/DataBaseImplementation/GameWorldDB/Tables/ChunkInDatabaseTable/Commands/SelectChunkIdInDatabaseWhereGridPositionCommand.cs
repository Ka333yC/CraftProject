using DataBase.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Core.ChunkCore.Saving.ChunkSerialization.ChunkInDatabaseCommands
{
	public class SelectChunkIdInDatabaseWhereGridPositionCommand : ICommand
	{
		public Vector3Int GridPosition;

		public string Command()
		{
			return $"SELECT {nameof(ChunkInDatabase.Id)} " +
				$"FROM {nameof(ChunkInDatabase)} " +
				$"WHERE {nameof(ChunkInDatabase.GridPositionX)} = {GridPosition.x} AND " +
				$"{nameof(ChunkInDatabase.GridPositionZ)} = {GridPosition.z}";
		}
	}
}
