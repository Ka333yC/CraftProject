using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts.Core.BlocksCore
{
	public class AirBlockContainer : IBlockContainer
	{
		public int Id { get; set; }

		public void Initialize()
		{
			Block.Air.Initialize(this);
		}

		public Block CreateBlock()
		{
			return Block.Air;
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
