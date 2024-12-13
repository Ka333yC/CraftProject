using System;
using Assets._Scripts.Core.BlocksCore;
using Assets.Scripts.InventoryCore;
using UnityEngine;

namespace Assets.Scripts.Core.InventoryCore.ItemLogic.BlockItem
{
	public interface IBlockItemData : IItemData
	{
		public IBlockContainer BlockContainer { get; }
	}
}
