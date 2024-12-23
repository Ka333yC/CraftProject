using UnityEngine;
using static Assets._Scripts.Core.BlocksCore.Block;

namespace Assets._Scripts.Core.BlocksCore
{
	public class AirBlockContainer : IBlockContainer
	{
		private readonly Block _air;

		public int Id { get; set; }

		public AirBlockContainer()
		{
			_air = BlockPool.Shared.Rent(true);
		}

		public void Initialize()
		{
			_air.Container = this;
		}

		public Block CreateBlock()
		{
			return _air;
		}

		public bool IsPlaceable(Vector3Int worldPosition)
		{
			return true;
		}

		public bool TryGetComponentContainer<T>(out T result) where T : IBlockComponentContainer
		{
			result = default;
			return false;
		}
	}
}
