using System;
using Assets.Scripts.InventoryCore;

namespace Assets.Scripts.Core.InventoryCore
{
	public interface IInventorySlotFilter
	{
		public bool IsPass(Item item);
	}
}
