using _Scripts.Core.PhysicsCore.ChunkPhysicsCore.ChunkPhysicsLogic.Components.Elements;
using _Scripts.Core.PhysicsCore.ChunkPhysicsCore.ChunkPhysicsLogic.Components.Elements.BlockPhysicsGetters;

namespace _Scripts.Core.PhysicsCore.ChunkPhysicsCore.ChunkPhysicsLogic.Components
{
	public struct ChunkPhysicsComponent
	{
		public BlocksPhysicsGetter BlocksPhysicsGetter;
		public ColliderMeshPartsContainer MeshPartsContainer;
	}
}
