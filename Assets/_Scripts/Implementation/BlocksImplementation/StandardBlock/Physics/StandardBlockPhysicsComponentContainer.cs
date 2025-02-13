using _Scripts.Core.BlocksCore;
using _Scripts.Core.PhysicsCore.ChunkPhysicsCore.BlockPhysics;
using _Scripts.Implementation.BlocksImplementation;
using UnityEngine;

namespace _Scripts.Implementation.Blocks.StandardBlock.Physics
{
	[CreateAssetMenu(fileName = "BlockPhysicsComponent", menuName = "Blocks/Physics/Standard block physics component")]
	public class StandardBlockPhysicsComponentContainer : BlockComponentContainer, IPhysicsBlockComponentContainer
	{
		[field: SerializeField] 
		public StandardMeshData MeshData { get; private set; }

		public override void InitializeBlock(Block block)
		{
			block.AddComponent(new StandardBlockPhysics(this));
		}
	}
}
