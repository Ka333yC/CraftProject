using System;
using System.Collections.Generic;

namespace Assets.Scripts.Core.GraphicsCore.ChunkGraphicsCore.MeshUpdating.Components
{
	public struct ChunkGraphicsDirtyMeshComponent
	{
		public LinkedList<int> AssociatedDirtyMeshChunkEntities;
	}
}
