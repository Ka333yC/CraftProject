using UnityEngine;
using static _Scripts.Core.BlocksCore.Block;

namespace _Scripts.Core.BlocksCore
{
	public class AirBlockContainer : IBlockContainer
	{
		private readonly Block _air;

		public int Id { get; private set; }

		public AirBlockContainer()
		{
			_air = BlockPool.Shared.Rent(true);
		}

		public void Initialize(int id)
		{
			Id = id;
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
