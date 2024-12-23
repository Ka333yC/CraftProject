using Assets._Scripts.Core.BlocksCore;
using ChunkCore.BlockData;
using GraphicsCore.ChunkGraphicsCore.BlockGraphics;
using Leopotam.EcsLite;
using MeshCreation.Preset;
using System.Collections.Generic;
using TempScripts;
using UnityEngine;
using Zenject;

namespace Realization.Blocks.Cube.Graphics
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

		public override bool CanInitializeAsync { get; } = true;

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