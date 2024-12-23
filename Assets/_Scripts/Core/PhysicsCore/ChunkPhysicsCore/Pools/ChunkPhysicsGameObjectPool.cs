using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Core.ChunkPhysicsCore.Cache;
using ObjectPoolPattern;
using UnityEngine;

namespace PhysicsCore.ChunkPhysicsCore.Cache.ChunkPhysicsMeshColliderPoolScripts
{
	public class ChunkPhysicsGameObjectPool : IPool<ChunkPhysicsGameObject>
	{
		private readonly Queue<ChunkPhysicsGameObject> _freeObjects = new Queue<ChunkPhysicsGameObject>();
		private ICreator<ChunkPhysicsGameObject> _creator;

		public ChunkPhysicsGameObjectPool()
		{
			_creator = new ChunkPhysicsGameObjectCreator();
		}

		public void AddSize(int size)
		{
			for(int i = 0; i < size; i++)
			{
				_freeObjects.Enqueue(_creator.Create());
			}
		}

		public ChunkPhysicsGameObject Get()
		{
			var freeObject = _freeObjects.Any() ? _freeObjects.Dequeue() : _creator.Create();
			freeObject.gameObject.SetActive(true);
			return freeObject;
		}

		public void Return(ChunkPhysicsGameObject toSetFree)
		{
			toSetFree.MeshCollider.sharedMesh = null;
			toSetFree.gameObject.SetActive(false);
			_freeObjects.Enqueue(toSetFree);
		}
	}
}
