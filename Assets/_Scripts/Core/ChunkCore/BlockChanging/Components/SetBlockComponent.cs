using Assets._Scripts.Core.BlocksCore;
using ChunkCore.LifeTimeControl;
using UnityEngine;

namespace ChunkCore.OnBlockChanged.Components
{
	public struct SetBlockComponent
	{
		public Vector3Int WorldPosition;
		public Block Block;
	}
}
