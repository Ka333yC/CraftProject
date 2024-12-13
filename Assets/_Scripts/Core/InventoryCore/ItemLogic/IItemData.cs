using System;
using UnityEngine;

namespace Assets.Scripts.InventoryCore
{
	public interface IItemData
	{
		public int Id { get; }
		public short StackSize { get; }
		public Sprite Icon { get; }

		public void Initialize(int id);
	}
}
