using Assets._Scripts.Core.ChunkCore.ChunkLogic.Components;
using ObjectPoolPattern;
using TempScripts;
using UnityEngine;

namespace PhysicsCore.ChunkPhysicsCore.Cache.ChunkPhysicsMeshColliderPoolScripts
{
	public class ChunkGameObjectCreator : ICreator<ChunkGameObject>
	{
		private static Transform _parent;

		static ChunkGameObjectCreator()
		{
			var chunkPhysicsParentGameObject = new GameObject("Chunk physics parent");
			_parent = chunkPhysicsParentGameObject.transform;
		}

		public ChunkGameObject Create()
		{
			return GameObject.Instantiate(Singleton.Instance.ChunkPrefab, _parent);
		}
	}
}
