using System.Threading;
using Assets._Scripts.Core.ChunkCore.ChunkLogic.Components;
using Assets.Scripts.Core.ChunkCore.ChunkLogic.Components.Elements;
using UnityEngine;

namespace Assets.Scripts.Core.ChunkCore.LifeTimeControl.Components
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