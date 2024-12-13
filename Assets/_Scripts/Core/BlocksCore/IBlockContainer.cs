using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts.Core.BlocksCore
{
	public interface IBlockContainer
	{
		public int Id { get; }

		public void Initialize();
		public Block CreateBlock();
		public bool TryGetContainerComponent<T>(out T result) where T : IBlockContainerComponent;
		public bool IsPlaceable(Vector3Int worldPosition);
	}
}
