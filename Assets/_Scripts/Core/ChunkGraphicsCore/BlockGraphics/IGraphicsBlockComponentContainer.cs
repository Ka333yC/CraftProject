using Assets._Scripts.Core.BlocksCore;
using System.Collections.Generic;
using UnityEngine;

namespace GraphicsCore.ChunkGraphicsCore.BlockGraphics
{
	public interface IGraphicsBlockComponentContainer : IBlockComponentContainer
	{
		public IEnumerable<Texture2D> GetTextures();
		public void SetTexturesRects(IEnumerable<Rect> rects);
	}
}
