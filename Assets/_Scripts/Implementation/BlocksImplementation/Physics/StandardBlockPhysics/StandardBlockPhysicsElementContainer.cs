using _Scripts.Core.BlocksCore;
using _Scripts.Core.PhysicsCore.ChunkPhysicsCore.BlockPhysics;
using UnityEngine;

namespace _Scripts.Implementation.BlocksImplementation.Physics.StandardBlockPhysics
{
	[CreateAssetMenu(fileName = "BlockPhysicsElement", menuName = "Blocks/Physics/Standard block physics element")]
	public class StandardBlockPhysicsElementContainer : BlockComponentContainer, IPhysicsBlockComponentContainer
	{
		[field: SerializeField] 
		public StandardMeshData MeshData { get; private set; }

		public override void InitializeBlock(Block block)
		{
			block.AddComponent(new StandardBlockPhysics(this));
		}
	}
}
