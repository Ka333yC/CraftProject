using _Scripts.Core.BlocksCore;
using _Scripts.Core.MeshWrap;

namespace _Scripts.Core.PhysicsCore.ChunkPhysicsCore.BlockPhysics
{
	public interface IPhysicsBlockComponent : IBlockComponent
	{
		public bool IsFull(Face face);
		public MeshDataPart GetMeshDataPart(Face face);
	}
}
