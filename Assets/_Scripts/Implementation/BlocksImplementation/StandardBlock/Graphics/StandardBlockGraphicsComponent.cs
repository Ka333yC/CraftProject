using System;
using _Scripts.Apart.Extensions;
using _Scripts.Core;
using _Scripts.Core.BlocksCore;
using _Scripts.Core.ChunkGraphicsCore.BlockGraphics;
using _Scripts.Core.MeshWrap;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Serialization;

namespace _Scripts.Implementation.BlocksImplementation.StandardBlock.Graphics
{
	[MovedFrom(true, sourceClassName: "StandardBlockGraphics")]
	[Serializable]
	public class StandardBlockGraphicsComponent : IGraphicsBlockComponent
	{
		[SerializeField]
		private StandardMeshData _meshData;
		[SerializeField]
		private StandardBlockTextureData _textureData;
		[SerializeField]
		private BlockTransparency _transparency;

		public void InitializeBlock(Block block)
		{
			block.AddComponent(this);
		}

		public IBlockComponent Clone()
		{
			var result = new StandardBlockGraphicsComponent();
			result._meshData = _meshData;
			result._textureData = _textureData;
			result._transparency = _transparency;
			return result;
		}

		public Texture2D[] GetTextures()
		{
			var textureSides = _textureData.GetAllTextureSides();
			Texture2D[] textures = new Texture2D[textureSides.Length];
			for(int i = 0; i < textureSides.Length; i++)
			{
				textures[i] = textureSides[i].Texture;
			}
			
			return textures;
		}

		public void SetTexturesRects(Rect[] rects)
		{
			var textureSides = _textureData.GetAllTextureSides();
			for(int i = 0; i < rects.Length; i++)
			{
				textureSides[i].UV = rects[i].GetEdges();
			}
		}

		public bool IsTransparent(Face face)
		{
			return _transparency.IsTransparent(face);
		}

		public MeshDataPart GetMeshDataPart(Face face)
		{
			return _meshData.GetSide(face);
		}

		public Vector2[] GetUV(Face face)
		{
			return _textureData.GetSide(face).UV;
		}
	}
}
