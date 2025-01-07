using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Core.ChunkCore.BlockChanging.Components
{
	public struct BlocksChangedComponent
	{
		public LinkedList<Vector3Int> ChangedBlocksPositions;
	}
}
