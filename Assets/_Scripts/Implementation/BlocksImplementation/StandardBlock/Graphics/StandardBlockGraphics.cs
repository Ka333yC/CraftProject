using _Scripts.Core;
using _Scripts.Core.ChunkGraphicsCore.BlockGraphics;
using _Scripts.Core.MeshWrap;
using UnityEngine;

namespace _Scripts.Implementation.BlocksImplementation.StandardBlock.Graphics
{
	public class StandardBlockGraphics : IGraphicsBlockComponent
	{
		private readonly StandardBlockGraphicsComponentContainer _graphicsContainer;

		public StandardBlockGraphics(StandardBlockGraphicsComponentContainer graphicsComponent)
		{
			_graphicsContainer = graphicsComponent;
		}

		public bool IsTransparent(Face face)
		{
			return _graphicsContainer.Transparency.IsTransparent(face);
		}

		public MeshDataPart GetMeshDataPart(Face face)
		{
			return _graphicsContainer.MeshData.GetSide(face);
		}

		public Vector2[] GetUV(Face face)
		{
			return _graphicsContainer.TextureData.GetSide(face).UV;
		}
	}
}
