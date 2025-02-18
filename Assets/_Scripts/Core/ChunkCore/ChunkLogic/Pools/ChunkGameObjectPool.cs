using System.Collections.Generic;
using System.Linq;
using _Scripts.Apart.ObjectPoolPattern;
using _Scripts.Core.ChunkCore.ChunkLogic.Components;

namespace _Scripts.Core.ChunkCore.ChunkLogic.Pools
{
	public class ChunkGameObjectPool : IPool<ChunkGameObject>
	{
		private readonly Queue<ChunkGameObject> _freeObjects = new Queue<ChunkGameObject>();
		private readonly ICreator<ChunkGameObject> _creator;

		public ChunkGameObjectPool(ICreator<ChunkGameObject> creator)
		{
			_creator = creator;
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
