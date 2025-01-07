using System.Collections.Generic;
using _Scripts.Core.BlocksCore;
using _Scripts.Core.ChunkGraphicsCore.BlockGraphics;
using UnityEngine;

namespace _Scripts.Implementation.BlocksImplementation.Graphics.StandardBlockGraphics
{
	[CreateAssetMenu(fileName = "BlockGraphicsElement", menuName = "Blocks/Graphics/Standard block graphics element")]
	public class StandardBlockGraphicsElementContainer : BlockComponentContainer, IGraphicsBlockComponentContainer
	{
		[field: SerializeField] 
		public StandardMeshData MeshData { get; private set; }

		[field: SerializeField] 
		public StandardBlockTextureData TextureData { get; private set; }

		[field: SerializeField] 
		public BlockTransparency Transparency { get; private set; }

		public override void InitializeBlock(Block block)
		{
			block.AddComponent(new StandardBlockGraphics(this));
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