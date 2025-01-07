using _Scripts.Core.BlocksCore;
using _Scripts.Core.PhysicsCore.ChunkPhysicsCore.BlockPhysics;
using UnityEngine;

namespace _Scripts.Implementation.BlocksImplementation.Physics.StandartBlockPhysics
{
	[CreateAssetMenu(fileName = "BlockPhysicsElement", menuName = "Blocks/Physics/Standart block physics element")]
	public class StandartBlockPhysicsElementContainer : BlockComponentContainer, IPhysicsBlockComponentContainer
	{
		[field: SerializeField] 
		public StandartMeshData MeshData { get; private set; }

		public override void InitializeBlock(Block block)
		{
			block.AddComponent(new StandartBlockPhysics(this));
		}
	}
}
