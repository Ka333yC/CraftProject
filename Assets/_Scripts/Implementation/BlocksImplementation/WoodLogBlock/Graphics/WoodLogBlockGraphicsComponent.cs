using System;
using System.Collections.Generic;
using _Scripts.Apart.Extensions;
using _Scripts.Core;
using _Scripts.Core.BlocksCore;
using _Scripts.Core.ChunkGraphicsCore.BlockGraphics;
using _Scripts.Core.MeshWrap;
using UnityEngine;

namespace _Scripts.Implementation.BlocksImplementation.WoodLogBlock.Graphics
{
	[Serializable]
	public class WoodLogBlockGraphicsComponent : IGraphicsBlockComponent
	{
		[SerializeField]
		private StandardMeshData _meshData;
		[SerializeField]
		private WoodLogBlockTextureData _textureData;
		
		private WoodLogBlockComponent _woodLogBlockComponent;
		
		public void InitializeBlock(Block block)
		{
			_woodLogBlockComponent = block.GetComponent<WoodLogBlockComponent>();
			block.AddComponent(this);
		}

		public IBlockComponent Clone()
		{
			var result = new WoodLogBlockGraphicsComponent();
			result._meshData = _meshData;
			result._textureData = _textureData;
			return result;
		}

		public Texture2D[] GetTextures()
		{
			List<Texture2D> textures = new();
			foreach(var textureSide in _textureData.VerticalRotationTextureData.GetAllTextureSides())
			{
				textures.Add(textureSide.Texture);
			}

			foreach(var textureSide in _textureData.HorizontalByXRotationTextureData.GetAllTextureSides())
			{
				textures.Add(textureSide.Texture);
			}

			foreach(var textureSide in _textureData.HorizontalByZRotationTextureData.GetAllTextureSides())
			{
				textures.Add(textureSide.Texture);
			}

			return textures.ToArray();
		}

		public void SetTexturesRects(Rect[] rects)
		{
			int i = 0;
			var textureData = _textureData.VerticalRotationTextureData.GetAllTextureSides();
			for(int j = 0; j < textureData.Length; i++, j++)
			{
				textureData[j].UV = rects[i].GetEdges();
			}
			
			textureData = _textureData.HorizontalByXRotationTextureData.GetAllTextureSides();
			for(int j = 0; j < textureData.Length; i++, j++)
			{
				textureData[j].UV = rects[i].GetEdges();
			}
			
			textureData = _textureData.HorizontalByZRotationTextureData.GetAllTextureSides();
			for(int j = 0; j < textureData.Length; i++, j++)
			{
				textureData[j].UV = rects[i].GetEdges();
			}
		}
		
		public bool IsTransparent(Face face)
		{
			return false;
		}

		public MeshDataPart GetMeshDataPart(Face face)
		{
			return _meshData.GetSide(face);
		}

		public Vector2[] GetUV(Face face)
		{
			var textureData = _woodLogBlockComponent.Rotation switch
			{
				WoodLogRotation.Vertical => _textureData.VerticalRotationTextureData,
				WoodLogRotation.HorizontalByX => _textureData.HorizontalByXRotationTextureData,
				WoodLogRotation.HorizontalByZ => _textureData.HorizontalByZRotationTextureData,
				_ => throw new NotImplementedException(),
			};
			
			return textureData.GetSide(face).UV;
		}
	}
}