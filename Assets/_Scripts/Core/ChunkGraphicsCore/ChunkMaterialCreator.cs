using System.Collections.Generic;
using System.Linq;
using _Scripts.Core.BlocksCore;
using _Scripts.Core.ChunkGraphicsCore.BlockGraphics;
using UnityEngine;

namespace _Scripts.Core.ChunkGraphicsCore
{
	public class ChunkGraphicsTextureCreator
	{
		private readonly BlocksContainers _blocksContainers;

		public ChunkGraphicsTextureCreator(BlocksContainers blocksContainers)
		{
			_blocksContainers = blocksContainers;
		}

		public Texture2D CreateTexture()
		{
			// делаю так потому что https://gamedev.stackexchange.com/questions/133717/unity-texture-on-plane-fades-at-angle-generating-mipmaps
			var packedTexture = PackTextures();
			var mainTexture = new Texture2D(packedTexture.width, packedTexture.height);
			mainTexture.filterMode = FilterMode.Point;
			mainTexture.wrapMode = TextureWrapMode.Clamp;
			mainTexture.SetPixels32(packedTexture.GetPixels32());
			mainTexture.Apply(true, true);
			return mainTexture;
		}

		private Texture2D PackTextures()
		{
			var graphicsElementContainers =
				GetBlockGraphicsElementContainers();
			var allTextures = GetTextures(graphicsElementContainers);
			var packedTexure = new Texture2D(1, 1, TextureFormat.RGBA32, true);
			Rect[] rects = packedTexure.PackTextures(allTextures.ToArray(), 0);
			int i = 0;
			foreach(var graphicsComponent in
				graphicsElementContainers)
			{
				int count = graphicsComponent.Value.Count();
				var textureRects = rects.Skip(i).Take(count);
				graphicsComponent.Key.SetTexturesRects(textureRects);
				i += count;
			}

			return packedTexure;
		}

		private Dictionary<IGraphicsBlockComponentContainer, IEnumerable<Texture2D>> 
			GetBlockGraphicsElementContainers()
		{
			var graphicsElementContainers =
				new Dictionary<IGraphicsBlockComponentContainer, IEnumerable<Texture2D>>();
			foreach(var blockData in _blocksContainers)
			{
				if(blockData.TryGetComponentContainer(out IGraphicsBlockComponentContainer 
					graphicsElementContainer))
				{
					IEnumerable<Texture2D> textures = graphicsElementContainer.GetTextures();
					graphicsElementContainers.Add(graphicsElementContainer, textures);
				}
			}

			return graphicsElementContainers;
		}

		private IEnumerable<Texture2D> GetTextures(Dictionary<IGraphicsBlockComponentContainer, 
			IEnumerable<Texture2D>> graphicsElementContainers)
		{
			var allTextures = new List<Texture2D>();
			foreach(var graphicsElementContainer
				in graphicsElementContainers)
			{
				allTextures.AddRange(graphicsElementContainer.Value);
			}

			return allTextures;
		}
	}
}
