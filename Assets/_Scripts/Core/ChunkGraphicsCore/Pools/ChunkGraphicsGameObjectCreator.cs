using Assets.Scripts.Core.ChunkGraphicsCore.Cache;
using ObjectPoolPattern;
using TempScripts;
using UnityEngine;

namespace GraphicsCore.ChunkGraphicsCore.Cache.ChunkGraphicsMeshFilterPoolScripts
{
	public class ChunkGraphicsGameObjectCreator : ICreator<ChunkGraphicsGameObject>
	{
		private static Transform _parent;

		static ChunkGraphicsGameObjectCreator()
		{
			var chunkGraphicsParentGameObject = new GameObject("Chunk graphics parent");
			_parent = chunkGraphicsParentGameObject.transform;
		}

		public ChunkGraphicsGameObject Create()
		{
			return GameObject.Instantiate(Singleton.Instance.ChunkGraphicsPrefab, _parent);
		}
	}
}
