using System;
using UnityEngine;

namespace Assets.Scripts.Realization.Blocks.BlockDataPresentation
{
	public abstract class BlockPlaceableChecker : ScriptableObject
	{
		public abstract bool IsPlaceable(Vector3Int worldPosition);
	}
}
