using UnityEngine;

namespace _Scripts.Core
{
	public static class ChunkConstantData
	{
		public static readonly Vector3Int ChunkScale = new Vector3Int(16, 256, 16);

		public static readonly Vector3 ShiftToBlockCenter = new Vector3(0.5f, 0, 0.5f);

		public static bool IsPositionInChunk(Vector3Int blockPosition)
		{
			return IsPositionInChunkByX(blockPosition) &&
				IsPositionInChunkByZ(blockPosition) &&
				IsPositionInChunkByY(blockPosition);
		}

		public static bool IsPositionInChunkByX(Vector3Int blockPosition)
		{
			return blockPosition.x >= 0 && blockPosition.x < ChunkScale.x;
		}

		public static bool IsPositionInChunkByY(Vector3Int blockPosition)
		{
			return blockPosition.y >= 0 && blockPosition.y < ChunkScale.y;
		}

		public static bool IsPositionInChunkByZ(Vector3Int blockPosition)
		{
			return blockPosition.z >= 0 && blockPosition.z < ChunkScale.z;
		}

		public static Vector3Int GridToWorldPosition(Vector3Int position)
		{
			return new Vector3Int(ChunkScale.x * position.x,
				ChunkScale.y * position.y, ChunkScale.z * position.z);
		}

		public static Vector3Int WorldToGridPosition(Vector3 position)
		{
			Vector3Int blockPositionAtChunk = WorldToBlockWorldPosition(position);
			return WorldToGridPosition(blockPositionAtChunk);
		}

		public static Vector3Int WorldToGridPosition(Vector3Int position)
		{
			if(position.x < 0)
			{
				position.x -= ChunkScale.x - 1;
			}

			if(position.z < 0)
			{
				position.z -= ChunkScale.z - 1;
			}

			return new Vector3Int(position.x / ChunkScale.x, 0,
				position.z / ChunkScale.z);
		}

		public static Vector3Int WorldToBlockPositionInChunk(Vector3Int position)
		{
			Vector3Int chunkWorldPosition = GridToWorldPosition(WorldToGridPosition(position));
			return position - chunkWorldPosition;
		}

		/// <returns>Возвращает координаты блока в мировом пространстве, но приемлимые для чанка</returns>
		public static Vector3Int WorldToBlockWorldPosition(Vector3 position)
		{
			return new Vector3Int(Mathf.FloorToInt(position.x),
				Mathf.FloorToInt(position.y), Mathf.FloorToInt(position.z));
		}
	}
}
