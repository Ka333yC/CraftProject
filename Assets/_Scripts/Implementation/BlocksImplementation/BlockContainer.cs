using _Scripts.Core.BlocksCore;
using UnityEngine;

namespace _Scripts.Implementation.BlocksImplementation
{
	public abstract class BlockContainer : ScriptableObject, IBlockContainer
	{
		public abstract int Id { get; set; }

		public abstract void Initialize();
		public abstract Block CreateBlock();
		public abstract bool IsPlaceable(Vector3Int worldPosition);
		public abstract bool TryGetComponentContainer<T>(out T result) where T : IBlockComponentContainer;
	}
}
