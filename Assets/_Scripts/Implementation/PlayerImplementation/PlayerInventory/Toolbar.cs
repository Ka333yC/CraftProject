using System;
using _Scripts.Core.InventoryCore;
using _Scripts.Core.PlayerCore;

namespace _Scripts.Implementation.PlayerImplementation.Inventory
{
	public class Toolbar : StandardInventory
	{
		public Toolbar() : base(PlayerConstantData.ToolbarSize)
		{
		}
	}
}