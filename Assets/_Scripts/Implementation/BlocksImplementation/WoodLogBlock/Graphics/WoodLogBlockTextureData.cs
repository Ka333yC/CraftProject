using System;
using _Scripts.Implementation.BlocksImplementation.StandardBlock.Graphics;
using UnityEngine;

namespace _Scripts.Implementation.BlocksImplementation.WoodLogBlock.Graphics
{
	[Serializable]
	public class WoodLogBlockTextureData
	{
		[field: SerializeField] 
		public StandardBlockTextureData VerticalRotationTextureData { get; private set; }
		[field: SerializeField] 
		public StandardBlockTextureData HorizontalByXRotationTextureData { get; private set; }
		[field: SerializeField] 
		public StandardBlockTextureData HorizontalByZRotationTextureData { get; private set; }
	}
}