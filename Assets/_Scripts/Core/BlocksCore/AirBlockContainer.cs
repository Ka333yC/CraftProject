using UnityEngine;

namespace Assets._Scripts.Core.BlocksCore
{
	public class AirBlockContainer : IBlockContainer
	{
		private readonly Block _air = new Block(true);

		public int Id { get; set; }
		public bool CanCreateBlockAsync => true;

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

		public bool TryGetContainerComponent<T>(out T result) where T : IBlockContainerComponent
		{
			result = default;
			return false;
		}
	}
}
