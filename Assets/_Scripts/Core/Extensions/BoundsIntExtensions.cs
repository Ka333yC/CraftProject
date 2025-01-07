using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Core.Extensions
{
	public static class BoundsIntExtensions
	{
		public static IEnumerable<Vector3Int> GetPositions(this BoundsInt bounds)
		{
			var max = bounds.max;
			for(int x = bounds.xMin; x < max.x; x++)
			{
				for(int y = bounds.yMin; y < max.y; y++)
				{
					for(int z = bounds.zMin; z < max.z; z++)
					{
						yield return new Vector3Int(x, y, z);
					}
				}
			}

			yield break;
		}

		public static IEnumerable<Vector3Int> GetGridPositions(this BoundsInt bounds)
		{
			for(int x = bounds.xMin; x < bounds.xMax; x += ChunkConstantData.ChunkScale.x)
			{
				for(int z = bounds.zMin; z < bounds.zMax; z += ChunkConstantData.ChunkScale.z)
				{
					var gridPosition = ChunkConstantData.WorldToGridPosition(new Vector3Int(x, 0, z));
					yield return gridPosition;
				}
			}

			yield break;
		}
	}
}
