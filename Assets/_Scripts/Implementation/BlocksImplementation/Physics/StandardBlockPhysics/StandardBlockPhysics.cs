using _Scripts.Core;
using _Scripts.Core.MeshWrap;
using _Scripts.Core.PhysicsCore.ChunkPhysicsCore.BlockPhysics;

namespace _Scripts.Implementation.BlocksImplementation.Physics.StandardBlockPhysics
{
	public class StandardBlockPhysics : IPhysicsBlockComponent
	{
		private StandardBlockPhysicsElementContainer _physicsElement;

		public StandardBlockPhysics(StandardBlockPhysicsElementContainer physicsElement)
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
