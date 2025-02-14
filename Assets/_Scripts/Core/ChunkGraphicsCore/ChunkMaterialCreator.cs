using System.Collections.Generic;
using System.Linq;
using _Scripts.Core.BlocksCore;
using _Scripts.Core.ChunkGraphicsCore.BlockGraphics;
using UnityEngine;

namespace _Scripts.Core.ChunkGraphicsCore
{
	public class ChunkGraphicsTextureCreator
	{
		private readonly BlocksArchetype _blocksContainers;

		public ChunkGraphicsTextureCreator(BlocksArchetype blocksContainers)
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
			var graphicsComponentContainers = GetBlockGraphicsComponentContainers();
			var allTextures = GetTextures(graphicsComponentContainers);
			var packedTexture = new Texture2D(1, 1, TextureFormat.RGBA32, true);
			Rect[] rects = packedTexture.PackTextures(allTextures.ToArray(), 0);
			int i = 0;
			foreach(var graphicsComponent in graphicsComponentContainers)
			{
				int count = graphicsComponent.Value.Count();
				var textureRects = rects.Skip(i).Take(count);
				graphicsComponent.Key.SetTexturesRects(textureRects.ToArray());
				i += count;
			}

			return packedTexture;
		}

		private Dictionary<IGraphicsBlockComponentContainer, IEnumerable<Texture2D>> GetBlockGraphicsComponentContainers()
		{
			var graphicsComponentContainers =
				new Dictionary<IGraphicsBlockComponentContainer, IEnumerable<Texture2D>>();
			foreach(var blockData in _blocksContainers)
			{
				if(blockData.TryGetComponentContainer(out IGraphicsBlockComponentContainer 
					graphicsComponentContainer))
				{
					IEnumerable<Texture2D> textures = graphicsComponentContainer.GetTextures();
					graphicsComponentContainers.Add(graphicsComponentContainer, textures);
				}
			}

			return graphicsComponentContainers;
		}

		private IEnumerable<Texture2D> GetTextures(Dictionary<IGraphicsBlockComponentContainer, 
			IEnumerable<Texture2D>> graphicsComponentContainers)
		{
			var allTextures = new List<Texture2D>();
			foreach(var graphicsComponentContainer
				in graphicsComponentContainers)
			{
				allTextures.AddRange(graphicsComponentContainer.Value);
			}

			return allTextures;
		}
	}
}
