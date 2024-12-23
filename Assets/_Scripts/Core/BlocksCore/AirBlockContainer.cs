using UnityEngine;
using static Assets._Scripts.Core.BlocksCore.Block;

namespace Assets._Scripts.Core.BlocksCore
{
	public class AirBlockContainer : IBlockContainer
	{
		private readonly IBlockComponentContainer[] _blockComponentContainers = new IBlockComponentContainer[0];
		private readonly Block _air;

		public int Id { get; set; }
		public bool CanCreateBlockAsync => true;
		public IBlockComponentContainer[] BlockComponentContainers => _blockComponentContainers;

		public AirBlockContainer()
		{
			_air = BlockPool.Shared.Rent(true);
		}

		public void Initialize()
		{
			_air.Initialize(this);
		}

		public Block CreateBlock()
		{
			return _air;
		}

		public bool IsPlaceable(Vector3Int worldPosition)
		{
			return true;
		}
	}
}
