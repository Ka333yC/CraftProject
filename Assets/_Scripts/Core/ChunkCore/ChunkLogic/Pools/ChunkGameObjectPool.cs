using System.Collections.Generic;
using System.Linq;
using Assets._Scripts.Core.ChunkCore.ChunkLogic.Components;
using ObjectPoolPattern;
using UnityEngine;

namespace PhysicsCore.ChunkPhysicsCore.Cache.ChunkPhysicsMeshColliderPoolScripts
{
	public class ChunkGameObjectPool : IPool<ChunkGameObject>
	{
		private readonly Queue<ChunkGameObject> _freeObjects = new Queue<ChunkGameObject>();
		private ICreator<ChunkGameObject> _creator;

		public ChunkGameObjectPool()
		{
			_creator = new ChunkGameObjectCreator();
		}

		public ChunkGameObject Get()
		{
			var freeObject = _freeObjects.Any() ? _freeObjects.Dequeue() : _creator.Create();
			freeObject.gameObject.SetActive(true);
			return freeObject;
		}

		public void Return(ChunkGameObject toSetFree)
		{
			toSetFree.MeshCollider.sharedMesh = null;
			toSetFree.gameObject.SetActive(false);
			_freeObjects.Enqueue(toSetFree);
		}
	}
}
