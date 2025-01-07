using System;
using UnityEngine;

namespace _Scripts.Implementation.BlocksImplementation.Graphics
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
