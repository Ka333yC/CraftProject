using _Scripts.Apart.ObjectPoolPattern;
using _Scripts.Core.ChunkCore.ChunkLogic.Components;
using _Scripts.TempScripts;
using UnityEngine;

namespace _Scripts.Core.ChunkCore.ChunkLogic.Pools
{
	public class ChunkGameObjectCreator : ICreator<ChunkGameObject>
	{
		private static Transform _parent;

		static ChunkGameObjectCreator()
		{
			var chunkPhysicsParentGameObject = new GameObject("Chunks");
			_parent = chunkPhysicsParentGameObject.transform;
		}

		public ChunkGameObject Create()
		{
			return GameObject.Instantiate(Singleton.Instance.ChunkPrefab, _parent);
		}
	}
}
