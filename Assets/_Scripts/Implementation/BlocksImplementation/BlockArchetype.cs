using _Scripts.Core.BlocksCore;
using UnityEngine;

namespace _Scripts.Implementation.BlocksImplementation
{
	public abstract class BlockArchetype : ScriptableObject, IBlockArchetype
	{
		public abstract int Id { get; }

		public abstract void Initialize(int id);
		public abstract Block CreateBlock();
		public abstract bool IsPlaceable(Vector3Int worldPosition);
		public abstract bool TryGetComponent<T>(out T result) where T : IBlockComponent;
	}
}
