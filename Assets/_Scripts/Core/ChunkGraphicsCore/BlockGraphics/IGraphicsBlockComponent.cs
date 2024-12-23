using System.Collections.Generic;
using Assets._Scripts.Core.BlocksCore;
using ChunkCore;
using MeshCreation.Preset;
using UnityEngine;

namespace GraphicsCore.ChunkGraphicsCore.BlockGraphics
{
	public interface IGraphicsBlockComponent : IBlockComponent
	{
		public bool IsTransparent(Face face);
		public MeshDataPart GetMeshDataPart(Face face);
		public Vector2[] GetUV(Face face);
	}
}
