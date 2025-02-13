using System;
using _Scripts.Core.InventoryCore;
using _Scripts.Core.InventoryCore.SlotLogic;
using _Scripts.Core.PlayerCore;

namespace _Scripts.Implementation.PlayerImplementation.PlayerInventory
{
	public class Toolbar : StandardInventory
	{
		private int _activeSlotIndex;
		
		public event Action<int> OnActiveSlotChanged;

		public int ActiveSlotIndex
		{
			get => _activeSlotIndex;
			set
			{
				if(_activeSlotIndex == value)
				{
					return;
				}
				
				_activeSlotIndex = value;
				OnActiveSlotChanged?.Invoke(_activeSlotIndex);
			}
		}

		public InventorySlot ActiveSlot => _slots[_activeSlotIndex];
		
		public Toolbar() : base(PlayerConstantData.ToolbarSize)
		{
		}
	}
}