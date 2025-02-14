using System;
using System.Collections.Generic;
using _Scripts.Apart.Extensions;
using _Scripts.Core;
using _Scripts.Core.BlocksCore;
using _Scripts.Core.ChunkGraphicsCore.BlockGraphics;
using _Scripts.Core.MeshWrap;
using _Scripts.Implementation.BlocksImplementation.WoodLogBlock.Graphics;
using UnityEngine;

namespace _Scripts.Implementation.BlocksImplementation.WoodLogBlock
{
	[Serializable]
	public class WoodLogBlockGraphicsComponentContainer : IGraphicsBlockComponentContainer
	{
		[field: SerializeField] 
		public StandardMeshData MeshData { get; private set; }
		[field: SerializeField] 
		public WoodLogBlockTextureData TextureData { get; private set; }
		
		public void InitializeBlock(Block block)
		{
			block.AddComponent(new WoodLogBlockGraphicsComponent(this, block));
		}

		public Texture2D[] GetTextures()
		{
			List<Texture2D> textures = new();
			foreach(var textureSide in TextureData.VerticalRotationTextureData.GetAllTextureSides())
			{
				textures.Add(textureSide.Texture);
			}

			foreach(var textureSide in TextureData.HorizontalByXRotationTextureData.GetAllTextureSides())
			{
				textures.Add(textureSide.Texture);
			}

			foreach(var textureSide in TextureData.HorizontalByZRotationTextureData.GetAllTextureSides())
			{
				textures.Add(textureSide.Texture);
			}

			return textures.ToArray();
		}

		public void SetTexturesRects(Rect[] rects)
		{
			int i = 0;
			var textureData = TextureData.VerticalRotationTextureData.GetAllTextureSides();
			for(int j = 0; j < textureData.Length; i++, j++)
			{
				textureData[j].UV = rects[i].GetEdges();
			}
			
			textureData = TextureData.HorizontalByXRotationTextureData.GetAllTextureSides();
			for(int j = 0; j < textureData.Length; i++, j++)
			{
				textureData[j].UV = rects[i].GetEdges();
			}
			
			textureData = TextureData.HorizontalByZRotationTextureData.GetAllTextureSides();
			for(int j = 0; j < textureData.Length; i++, j++)
			{
				textureData[j].UV = rects[i].GetEdges();
			}
		}
	}
}