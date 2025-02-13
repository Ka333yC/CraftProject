using _Scripts.Core.BlocksCore;
using UnityEngine;

namespace _Scripts.Implementation.BlocksImplementation
{
	public abstract class BlockContainer : ScriptableObject, IBlockContainer
	{
		public abstract int Id { get; }

		public abstract void Initialize(int id);
		public abstract Block CreateBlock();
		public abstract bool IsPlaceable(Vector3Int worldPosition);
		public abstract bool TryGetComponentContainer<T>(out T result) where T : IBlockComponentContainer;
	}
}
