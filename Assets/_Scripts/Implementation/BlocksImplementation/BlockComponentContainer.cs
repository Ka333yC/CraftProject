using Assets._Scripts.Core.BlocksCore;
using UnityEngine;

namespace ChunkCore.BlockData
{
	public abstract class BlockComponentContainer : ScriptableObject, IBlockComponentContainer
	{
		public abstract void InitializeBlock(Block block);
	}
}
