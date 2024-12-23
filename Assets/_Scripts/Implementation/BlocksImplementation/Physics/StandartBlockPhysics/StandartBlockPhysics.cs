using ChunkCore;
using MeshCreation.Preset;
using PhysicsCore.ChunkPhysicsCore.BlockPhysics;
using System.Collections.Generic;
using UnityEngine;

namespace Realization.Blocks.Cube.Physics
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
