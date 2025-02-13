using System;
using _Scripts.Core.BlocksCore;
using _Scripts.Core.PhysicsCore.ChunkPhysicsCore.BlockPhysics;
using _Scripts.Implementation.BlocksImplementation;
using UnityEngine;

namespace _Scripts.Implementation.Blocks.StandardBlock.Physics
{
	[Serializable]
	public class StandardBlockPhysicsComponentContainer : IPhysicsBlockComponentContainer
	{
		[field: SerializeField] 
		public StandardMeshData MeshData { get; private set; }

		public void InitializeBlock(Block block)
		{
			block.AddComponent(new StandardBlockPhysics(this));
		}
	}
}
