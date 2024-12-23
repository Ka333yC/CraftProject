using System;
using UnityEngine;

namespace GraphicsCore.ChunkGraphicsCore.BlockGraphics
{
	[Serializable]
	public class BlockTextureSide
	{
		[HideInInspector]
		public Vector2[] UV;

		[field: SerializeField]
		public int TextureIndex { get; private set; }
	}
}
