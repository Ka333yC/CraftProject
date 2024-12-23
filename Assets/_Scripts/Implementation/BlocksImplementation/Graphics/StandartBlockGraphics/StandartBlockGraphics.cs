using ChunkCore;
using GraphicsCore.ChunkGraphicsCore.BlockGraphics;
using MeshCreation.Preset;
using System.Collections.Generic;
using UnityEngine;

namespace Realization.Blocks.Cube.Graphics
{
	public class StandartBlockGraphics : IGraphicsBlockComponent
	{
		private readonly StandartBlockGraphicsElementContainer _graphicsElement;

		public StandartBlockGraphics(StandartBlockGraphicsElementContainer graphicsComponent)
		{
			_graphicsElement = graphicsComponent;
		}

		public bool IsTransparent(Face face)
		{
			return _graphicsElement.Transparency.IsTransparent(face);
		}

		public MeshDataPart GetMeshDataPart(Face face)
		{
			return _graphicsElement.MeshData.GetSide(face);
		}

		public Vector2[] GetUV(Face face)
		{
			return _graphicsElement.TextureData.GetSide(face).UV;
		}
	}
}
