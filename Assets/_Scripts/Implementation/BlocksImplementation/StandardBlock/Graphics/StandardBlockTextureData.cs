using System;
using _Scripts.Core;
using UnityEngine;

namespace _Scripts.Implementation.BlocksImplementation.StandardBlock.Graphics
{
	[Serializable]
	public class StandardBlockTextureData
	{
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

		public BlockTextureSide[] GetAllTextureSides()
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
