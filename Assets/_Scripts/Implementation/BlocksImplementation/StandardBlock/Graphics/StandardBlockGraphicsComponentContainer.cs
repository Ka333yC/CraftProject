using System;
using _Scripts.Apart.Extensions;
using _Scripts.Core.BlocksCore;
using _Scripts.Core.ChunkGraphicsCore.BlockGraphics;
using _Scripts.Implementation.Blocks.StandardBlock.Graphics;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Serialization;

namespace _Scripts.Implementation.BlocksImplementation.StandardBlock.Graphics
{
	[Serializable]
	public class StandardBlockGraphicsComponentContainer : IGraphicsBlockComponentContainer
	{
		[field: SerializeField] 
		public StandardMeshData MeshData { get; private set; }

		[field: SerializeField] 
		public StandardBlockTextureData TextureData { get; private set; }

		[field: SerializeField] 
		public BlockTransparency Transparency { get; private set; }

		public void InitializeBlock(Block block)
		{
			block.AddComponent(new StandardBlockGraphics(this));
		}

		public Texture2D[] GetTextures()
		{
			var textureSides = TextureData.GetAllTextureSides();
			Texture2D[] textures = new Texture2D[textureSides.Length];
			for(int i = 0; i < textureSides.Length; i++)
			{
				textures[i] = textureSides[i].Texture;
			}
			
			return textures;
		}

		public void SetTexturesRects(Rect[] rects)
		{
			var textureSides = TextureData.GetAllTextureSides();
			for(int i = 0; i < rects.Length; i++)
			{
				textureSides[i].UV = rects[i].GetEdges();
			}
		}
	}
}