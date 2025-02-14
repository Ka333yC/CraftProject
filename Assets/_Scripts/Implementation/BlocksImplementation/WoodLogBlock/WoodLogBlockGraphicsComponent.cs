using System;
using _Scripts.Core;
using _Scripts.Core.BlocksCore;
using _Scripts.Core.ChunkGraphicsCore.BlockGraphics;
using _Scripts.Core.MeshWrap;
using UnityEngine;

namespace _Scripts.Implementation.BlocksImplementation.WoodLogBlock
{
	public class WoodLogBlockGraphicsComponent : IGraphicsBlockComponent
	{
		private readonly WoodLogBlockGraphicsComponentContainer _graphicsContainer;
		private readonly WoodLogBlockComponent _woodLogBlockComponent;
		
		public WoodLogBlockGraphicsComponent(WoodLogBlockGraphicsComponentContainer graphicsContainer, Block block)
		{
			_graphicsContainer = graphicsContainer;
			_woodLogBlockComponent = block.GetComponent<WoodLogBlockComponent>();
		}
		
		public bool IsTransparent(Face face)
		{
			return false;
		}

		public MeshDataPart GetMeshDataPart(Face face)
		{
			return _graphicsContainer.MeshData.GetSide(face);
		}

		public Vector2[] GetUV(Face face)
		{
			var textureData = _woodLogBlockComponent.Rotation switch
			{
				WoodLogRotation.Vertical => _graphicsContainer.TextureData.VerticalRotationTextureData,
				WoodLogRotation.HorizontalByX => _graphicsContainer.TextureData.HorizontalByXRotationTextureData,
				WoodLogRotation.HorizontalByZ => _graphicsContainer.TextureData.HorizontalByZRotationTextureData,
				_ => throw new NotImplementedException(),
			};
			
			return textureData.GetSide(face).UV;
		}
	}
}