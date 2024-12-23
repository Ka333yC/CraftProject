using Assets.Scripts.Core.ChunkGraphicsCore.Cache;
using Assets.Scripts.Core.ChunkGraphicsCore.ChunkGraphicsScripts.Components.Elements;
using UnityEngine;

namespace GraphicsCore.ChunkGraphicsCore.LifeTimeControl.Components
{
	public struct ChunkGraphicsComponent
	{
		public ChunkGraphicsGameObject GameObject;
		public GraphicsMeshPartsContainer MeshPartsContainer;
		public BlocksGraphicsGetter BlocksGraphicsGetter;
	}
}
