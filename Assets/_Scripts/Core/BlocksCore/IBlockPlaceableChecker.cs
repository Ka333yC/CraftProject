using UnityEngine;

namespace _Scripts.Core.BlocksCore
{
	public interface IBlockPlaceableChecker
	{
		public bool IsPlaceable(Vector3Int worldPosition);
	}
}