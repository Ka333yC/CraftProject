using ChunkCore;
using Extensions;
using GraphicsCore.ChunkGraphicsCore.BlockGraphics;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Realization.Blocks.Cube.Graphics
{
	[Serializable]
	public class StandartBlockTextureData
	{
		[field: SerializeField] 
		public Texture2D[] Textures { get; private set; }

		[field: SerializeField] 
		public BlockTextureSide FrontSide { get; private set; }

		[field: SerializeField] 
		public BlockTextureSide BackSide { get; private set; }

		[field: SerializeField] 
		public BlockTextureSide TopSide { get; private set; }

		[field: SerializeField] 
		public BlockTextureSide BottomSide { get; private set; }

		[field: SerializeField] 
		public BlockTextureSide RightSide { get; private set; }

		[field: SerializeField] 
		public BlockTextureSide LeftSide { get; private set; }

		[field: SerializeField] 
		public BlockTextureSide[] Other { get; private set; }

		public void SetTextureRects(IEnumerable<Rect> rects)
		{
			var texturesUV = new List<Vector2[]>(rects.Count());
			foreach(Rect rect in rects)
			{
				Vector2[] uv = rect.GetEdges();
				texturesUV.Add(uv);
			}

			BlockTextureSide[] allTextureSides = GetAllTextureSides();
			foreach(BlockTextureSide textureSide in allTextureSides)
			{
				textureSide.UV = texturesUV[textureSide.TextureIndex];
			}
		}

		public BlockTextureSide GetSide(Face face)
		{
			return face switch
			{
				Face.Forward => FrontSide,
				Face.Back => BackSide,
				Face.Up => TopSide,
				Face.Down => BottomSide,
				Face.Right => RightSide,
				Face.Left => LeftSide,
				_ => throw new ArgumentException($"Unknown face \"{face}\"")
			};
		}

		private BlockTextureSide[] GetAllTextureSides()
		{
			var result = new BlockTextureSide[6 + Other.Length];
			result[0] = FrontSide;
			result[1] = BackSide;
			result[2] = TopSide;
			result[3] = BottomSide;
			result[4] = RightSide;
			result[5] = LeftSide;
			for(int i = 0; i < Other.Length; i++)
			{
				result[i + 6] = Other[i];
			}

			return result;
		}
	}
}
