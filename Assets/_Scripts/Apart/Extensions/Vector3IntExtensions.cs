using UnityEngine;

namespace _Scripts.Apart.Extensions
{
	public static class Vector3IntExtensions
	{
		public static bool IsBeetween(this Vector3Int positionToCheck, Vector3Int low, Vector3Int high)
		{
			return positionToCheck.x >= low.x && positionToCheck.z >= low.z &&
				positionToCheck.x <= high.x && positionToCheck.z <= high.z;
		}
	}
}
