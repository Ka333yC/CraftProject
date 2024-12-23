using Assets.Scripts.Core.ChunkPhysicsCore.Cache;
using Assets.Scripts.Core.ChunkPhysicsCore.ChunkPhysicsScripts.Components.Elements.BlockPhysicsGetters;

namespace PhysicsCore.ChunkPhysicsCore.LifeTimeControl.Components
{
	public struct ChunkPhysicsComponent
	{
		public ChunkPhysicsGameObject GameObject;
		public BlocksPhysicsGetter BlocksPhysicsGetter;
		public ColliderMeshPartsContainer MeshPartsContainer;
	}
}
