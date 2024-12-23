using UnityEngine;

namespace Assets._Scripts.Core.BlocksCore
{
	public interface IBlockContainer
	{
		public int Id { get; }
		public bool CanCreateBlockAsync { get; }

		public void Initialize();
		public Block CreateBlock();
		public bool IsPlaceable(Vector3Int worldPosition);
		public bool TryGetContainerComponent<T>(out T result) where T : IBlockComponentContainer;
	}
}
