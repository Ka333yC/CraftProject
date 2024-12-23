using Assets.Scripts.Core.ChunkPhysicsCore.Cache;
using ObjectPoolPattern;
using TempScripts;
using UnityEngine;

namespace PhysicsCore.ChunkPhysicsCore.Cache.ChunkPhysicsMeshColliderPoolScripts
{
	public class ChunkPhysicsGameObjectCreator : ICreator<ChunkPhysicsGameObject>
	{
		private static Transform _parent;

		static ChunkPhysicsGameObjectCreator()
		{
			var chunkPhysicsParentGameObject = new GameObject("Chunk physics parent");
			_parent = chunkPhysicsParentGameObject.transform;
		}

		public ChunkPhysicsGameObject Create()
		{
			return GameObject.Instantiate(Singleton.Instance.ChunkPhysicsPrefab, _parent);
		}
	}
}
