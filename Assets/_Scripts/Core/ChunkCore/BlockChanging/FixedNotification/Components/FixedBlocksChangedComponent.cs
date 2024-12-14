using System.Collections.Generic;
using UnityEngine;

namespace ChunkCore.OnBlockChanged.FixedNotification.Components
{
	public struct FixedBlocksChangedComponent
	{
		public LinkedList<Vector3Int> ChangedBlocksPositions;
	}
}
