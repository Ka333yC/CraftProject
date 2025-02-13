using System;
using UnityEngine;

namespace _Scripts.Implementation.BlocksImplementation
{
	[Serializable]
	public class BlockTextureSide
	{
		[field: SerializeField]
		public Texture2D Texture { get; private set; }
		
		public Vector2[] UV { get; set; }
	}
}
