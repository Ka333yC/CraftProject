using _Scripts.Core;
using _Scripts.Core.MeshWrap;
using _Scripts.Core.PhysicsCore.ChunkPhysicsCore.BlockPhysics;

namespace _Scripts.Implementation.BlocksImplementation.Physics.StandartBlockPhysics
{
	public class StandartBlockPhysics : IPhysicsBlockComponent
	{
		private StandartBlockPhysicsElementContainer _physicsElement;

		public StandartBlockPhysics(StandartBlockPhysicsElementContainer physicsElement)
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
