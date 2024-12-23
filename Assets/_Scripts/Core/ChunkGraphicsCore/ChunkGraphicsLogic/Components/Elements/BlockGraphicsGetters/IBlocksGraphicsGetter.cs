using GraphicsCore.ChunkGraphicsCore.BlockGraphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Core.ChunkGraphicsCore.ChunkGraphicsScripts.Components.Elements.BlockGraphicsGetters
{
	public interface IBlocksGraphicsGetter
	{
		public IGraphicsBlockComponent GetBlockGraphics(Vector3Int blockPosition);
	}
}
