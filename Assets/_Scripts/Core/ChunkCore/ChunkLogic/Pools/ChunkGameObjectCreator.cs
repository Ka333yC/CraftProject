using _Scripts.Apart.ObjectPoolPattern;
using _Scripts.Core.ChunkCore.ChunkLogic.Components;
using _Scripts.TempScripts;
using UnityEngine;

namespace _Scripts.Core.ChunkCore.ChunkLogic.Pools
{
	public class ChunkGameObjectCreator : ICreator<ChunkGameObject>
	{
		private readonly Transform _parent;
		private readonly ChunkGameObject _chunkPrefab;

		public ChunkGameObjectCreator(ChunkGameObject chunkPrefab)
		{
			var chunkPhysicsParentGameObject = new GameObject("Chunks");
			_parent = chunkPhysicsParentGameObject.transform;
			_chunkPrefab = chunkPrefab;
		}

		public ChunkGameObject Create()
		{
			return GameObject.Instantiate(_chunkPrefab, _parent);
		}
	}
}
