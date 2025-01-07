using _Scripts.Core.PhysicsCore.ChunkPhysicsCore.BlockPhysics;
using UnityEngine;

namespace _Scripts.Core.PhysicsCore.ChunkPhysicsCore.ChunkPhysicsLogic.Components.Elements.BlockPhysicsGetters
{
	public interface IBlocksPhysicsGetter
	{
		public IPhysicsBlockComponent GetBlockPhysics(Vector3Int blockPosition);
	}
}
