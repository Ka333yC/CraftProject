using _Scripts.Core.BlocksCore;
using UnityEngine;

namespace _Scripts.Implementation.BlocksImplementation
{
	public abstract class BlockComponentContainer : ScriptableObject, IBlockComponentContainer
	{
		public abstract void InitializeBlock(Block block);
	}
}
