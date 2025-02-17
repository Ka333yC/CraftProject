using System;
using _Scripts.Core;
using _Scripts.Core.BlocksCore;
using _Scripts.Core.MeshWrap;
using _Scripts.Core.PhysicsCore.ChunkPhysicsCore.BlockPhysics;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Serialization;

namespace _Scripts.Implementation.BlocksImplementation.StandardBlock.Physics
{
	[Serializable]
	public class StandardBlockPhysicsComponent : IPhysicsBlockComponent
	{
		[SerializeField]
		private StandardMeshData _meshData;

		public void InitializeBlock(Block block)
		{
			block.AddComponent(this);
		}

		public IBlockComponent Clone()
		{
			var result = new StandardBlockPhysicsComponent();
			result._meshData = _meshData;
			return result;
		}

		public bool IsFull(Face face)
		{
			return _meshData.HasSide(face);
		}

		public MeshDataPart GetMeshDataPart(Face face)
		{
			return _meshData.GetSide(face);
		}
	}
}
