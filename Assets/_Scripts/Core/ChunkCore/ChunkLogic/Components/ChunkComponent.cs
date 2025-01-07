using System.Threading;
using _Scripts.Core.ChunkCore.ChunkLogic.Components.Elements;
using UnityEngine;

namespace _Scripts.Core.ChunkCore.ChunkLogic.Components
{
	public struct ChunkComponent
	{
		public Vector3Int GridPosition;
		public ChunkGameObject GameObject;
		public ChunkSizeBlocks Blocks;
		// CancellationToken for cancellation all async Task related to chunk(entity)
		public CancellationTokenSource CancellationTokenSource;
	}
}