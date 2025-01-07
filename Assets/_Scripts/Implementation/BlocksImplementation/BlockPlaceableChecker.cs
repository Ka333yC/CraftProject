using UnityEngine;

namespace _Scripts.Implementation.BlocksImplementation
{
	public abstract class BlockPlaceableChecker : ScriptableObject
	{
		public abstract bool IsPlaceable(Vector3Int worldPosition);
	}
}
