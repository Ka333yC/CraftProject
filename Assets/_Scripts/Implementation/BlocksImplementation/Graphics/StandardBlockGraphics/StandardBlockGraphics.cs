using _Scripts.Core;
using _Scripts.Core.ChunkGraphicsCore.BlockGraphics;
using _Scripts.Core.MeshWrap;
using UnityEngine;

namespace _Scripts.Implementation.BlocksImplementation.Graphics.StandardBlockGraphics
{
	public class StandardBlockGraphics : IGraphicsBlockComponent
	{
		private readonly StandardBlockGraphicsElementContainer _graphicsElement;

		public StandardBlockGraphics(StandardBlockGraphicsElementContainer graphicsComponent)
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
