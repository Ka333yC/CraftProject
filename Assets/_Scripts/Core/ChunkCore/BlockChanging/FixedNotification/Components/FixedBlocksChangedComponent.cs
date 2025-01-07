using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Core.ChunkCore.BlockChanging.FixedNotification.Components
{
	public struct FixedBlocksChangedComponent
	{
		public LinkedList<Vector3Int> ChangedBlocksPositions;
	}
}
