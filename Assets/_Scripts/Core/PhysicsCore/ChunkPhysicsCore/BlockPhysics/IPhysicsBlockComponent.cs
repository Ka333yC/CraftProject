using System.Collections.Generic;
using Assets._Scripts.Core.BlocksCore;
using ChunkCore;
using MeshCreation.Preset;
using UnityEngine;

namespace PhysicsCore.ChunkPhysicsCore.BlockPhysics
{
	public interface IPhysicsBlockComponent : IBlockComponent
	{
		public bool IsFull(Face face);
		public MeshDataPart GetMeshDataPart(Face face);
	}
}
