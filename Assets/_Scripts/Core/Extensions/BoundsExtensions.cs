using UnityEngine;

namespace _Scripts.Core.Extensions
{
	public static class BoundsExtensions
	{
		public static BoundsInt Round(this Bounds bounds)
		{
			var min = ChunkConstantData.WorldToBlockWorldPosition(bounds.min);
			var max = ChunkConstantData.WorldToBlockWorldPosition(bounds.max);
			var size = max - min + Vector3Int.one;
			return new BoundsInt(min, size);
		}
	}
}
