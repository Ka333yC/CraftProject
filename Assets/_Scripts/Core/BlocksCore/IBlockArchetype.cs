using UnityEngine;

namespace _Scripts.Core.BlocksCore
{
	public interface IBlockArchetype
	{
		public int Id { get; }

		public void Initialize(int id);
		public Block CreateBlock();
		public bool IsPlaceable(Vector3Int worldPosition);
		public bool TryGetComponent<T>(out T result) where T : IBlockComponent;
	}
}
