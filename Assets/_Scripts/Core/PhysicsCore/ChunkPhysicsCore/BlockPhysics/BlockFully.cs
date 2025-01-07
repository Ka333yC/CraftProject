using System;

namespace _Scripts.Core.PhysicsCore.ChunkPhysicsCore.BlockPhysics
{
	[Serializable]
	public class BlockFully
	{
		public bool IsForwardSideFull = true;
		public bool IsBackSideFull = true;
		public bool IsUpSideFull = true;
		public bool IsDownSideFull = true;
		public bool IsRightSideFull = true;
		public bool IsLeftSideFull = true;

		public bool IsFull(Face face)
		{
			return face switch
			{
				Face.Forward => IsForwardSideFull,
				Face.Back => IsBackSideFull,
				Face.Up => IsUpSideFull,
				Face.Down => IsDownSideFull,
				Face.Right => IsRightSideFull,
				Face.Left => IsLeftSideFull,
				_ => throw new ArgumentException($"Unknown face \"{face}\"")
			};
		}
	}
}
