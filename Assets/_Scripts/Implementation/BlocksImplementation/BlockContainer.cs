using Assets._Scripts.Core.BlocksCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts.Implementation.BlocksImplementation
{
	public abstract class BlockContainer : ScriptableObject, IBlockContainer
	{
		public abstract int Id { get; set; }
		public abstract bool CanCreateBlockAsync { get; }

		public abstract void Initialize();
		public abstract Block CreateBlock();
		public abstract bool IsPlaceable(Vector3Int worldPosition);
		public abstract bool TryGetComponentContainer<T>(out T result) where T : IBlockComponentContainer;
	}
}
