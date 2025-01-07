using _Scripts.Core.ChunkGraphicsCore.BlockGraphics;
using UnityEngine;

namespace _Scripts.Core.ChunkGraphicsCore.ChunkGraphicsLogic.Components.Elements.BlockGraphicsGetters
{
	public interface IBlocksGraphicsGetter
	{
		public IGraphicsBlockComponent GetBlockGraphics(Vector3Int blockPosition);
	}
}
