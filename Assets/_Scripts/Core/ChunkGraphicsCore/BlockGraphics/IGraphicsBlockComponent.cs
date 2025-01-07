﻿using _Scripts.Core.BlocksCore;
using _Scripts.Core.MeshWrap;
using UnityEngine;

namespace _Scripts.Core.ChunkGraphicsCore.BlockGraphics
{
	public interface IGraphicsBlockComponent : IBlockComponent
	{
		public bool IsTransparent(Face face);
		public MeshDataPart GetMeshDataPart(Face face);
		public Vector2[] GetUV(Face face);
	}
}
