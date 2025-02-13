﻿using System.Collections.Generic;
using _Scripts.Core.BlocksCore;
using UnityEngine;

namespace _Scripts.Core.ChunkGraphicsCore.BlockGraphics
{
	public interface IGraphicsBlockComponentContainer : IBlockComponentContainer
	{
		public Texture2D[] GetTextures();
		public void SetTexturesRects(Rect[] rects);
	}
}
