using _Scripts.Core;
using _Scripts.Core.MeshWrap;
using _Scripts.Core.PhysicsCore.ChunkPhysicsCore.BlockPhysics;

namespace _Scripts.Implementation.Blocks.StandardBlock.Physics
{
	public class StandardBlockPhysics : IPhysicsBlockComponent
	{
		private readonly StandardBlockPhysicsComponentContainer _physicsElement;

		public StandardBlockPhysics(StandardBlockPhysicsComponentContainer physicsElement)
		{
			_physicsElement = physicsElement;
		}

		public bool IsFull(Face face)
		{
			return _physicsElement.MeshData.HasSide(face);
		}

		public MeshDataPart GetMeshDataPart(Face face)
		{
			return _physicsElement.MeshData.GetSide(face);
		}
	}
}
