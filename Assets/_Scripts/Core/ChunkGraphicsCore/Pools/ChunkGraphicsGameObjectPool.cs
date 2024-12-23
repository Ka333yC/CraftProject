using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Core.ChunkGraphicsCore.Cache;
using ObjectPoolPattern;
using UnityEngine;

namespace GraphicsCore.ChunkGraphicsCore.Cache.ChunkGraphicsMeshFilterPoolScripts
{
	public class ChunkGraphicsGameObjectPool : IPool<ChunkGraphicsGameObject>
	{
		private readonly Queue<ChunkGraphicsGameObject> _freeObjects = new Queue<ChunkGraphicsGameObject>();
		private ICreator<ChunkGraphicsGameObject> _creator;
		private int _size = 0;

		public ChunkGraphicsGameObjectPool()
		{
			_creator = new ChunkGraphicsGameObjectCreator();
		}

		public void AddSize(int size)
		{
			for(int i = 0; i < size; i++)
			{
				_freeObjects.Enqueue(_creator.Create());
			}
		}

		public ChunkGraphicsGameObject Get()
		{
			var freeObject = _freeObjects.Any() ? _freeObjects.Dequeue() : _creator.Create();
			freeObject.gameObject.SetActive(true);
			return freeObject;
		}

		public void Return(ChunkGraphicsGameObject toSetFree)
		{
			toSetFree.MeshFilter.mesh = null;
			toSetFree.gameObject.SetActive(false);
			_freeObjects.Enqueue(toSetFree);
		}
	}
}
