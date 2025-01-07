using System;

namespace _Scripts.Core.ChunkGraphicsCore.BlockGraphics
{
	[Serializable]
	public class BlockTransparency
	{
		public bool IsForwardSideTransparent;
		public bool IsBackSideTransparent;
		public bool IsUpSideTransparent;
		public bool IsDownSideTransparent;
		public bool IsRightSideTransparent;
		public bool IsLeftSideTransparent;

		public bool IsTransparent(Face face)
		{
			return face switch
			{
				Face.Forward => IsForwardSideTransparent,
				Face.Back => IsBackSideTransparent,
				Face.Up => IsUpSideTransparent,
				Face.Down => IsDownSideTransparent,
				Face.Right => IsRightSideTransparent,
				Face.Left => IsLeftSideTransparent,
				_ => throw new ArgumentException($"Unknown face \"{face}\"")
			};
		}
	}
}
