using UnityEngine;

namespace _Scripts.Core.BlocksCore
{
	public interface IBlockContainer
	{
		public int Id { get; }

		public void Initialize();
		public Block CreateBlock();
		public bool IsPlaceable(Vector3Int worldPosition);
		public bool TryGetComponentContainer<T>(out T result) where T : IBlockComponentContainer;
	}
}
