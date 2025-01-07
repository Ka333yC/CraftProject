using System.Collections.Generic;
using _Scripts.Core.BlocksCore;
using _Scripts.Core.ChunkGraphicsCore.BlockGraphics;
using UnityEngine;

namespace _Scripts.Implementation.BlocksImplementation.Graphics.StandartBlockGraphics
{
	[CreateAssetMenu(fileName = "BlockGraphicsElement", menuName = "Blocks/Graphics/Standart block graphics element")]
	public class StandartBlockGraphicsElementContainer : BlockComponentContainer, IGraphicsBlockComponentContainer
	{
		[field: SerializeField] 
		public StandartMeshData MeshData { get; private set; }

		[field: SerializeField] 
		public StandartBlockTextureData TextureData { get; private set; }

		[field: SerializeField] 
		public BlockTransparency Transparency { get; private set; }

		public override void InitializeBlock(Block block)
		{
			block.AddComponent(new StandartBlockGraphics(this));
		}

		public IEnumerable<Texture2D> GetTextures()
		{
			return TextureData.Textures;
		}

		public void SetTexturesRects(IEnumerable<Rect> rects)
		{
			TextureData.SetTextureRects(rects);
		}
	}
}