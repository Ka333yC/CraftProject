using System.Collections.Generic;
using UnityEngine;

namespace ChunkCore.OnBlockChanged.Components
{
	public struct BlocksChangedComponent
	{
		public LinkedList<Vector3Int> ChangedBlocksPositions;
	}
}
