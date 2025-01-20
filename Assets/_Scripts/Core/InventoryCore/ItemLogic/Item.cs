using System;
using UnityEngine;

namespace _Scripts.Core.InventoryCore.ItemLogic
{
	public abstract class Item
	{
		private int _count;

		public event Action OnChanged;

		public abstract IItemData ItemData { get; }
		public virtual int Count 
		{
			get => _count;
			set 
			{
				_count = value;
				if(_count < 0 || _count > ItemData.StackSize)
				{
					throw new ArgumentOutOfRangeException();
				}
				
				InvokeOnChanged();
			}
		}

		public abstract Item Clone();

		public virtual bool IsSimilar(Item item) 
		{
			return item != null && ItemData == item.ItemData;
		}

		public virtual Item Split(int splittedItemCount)
		{
			var result = Clone();
			result.Count = splittedItemCount;
			Count -= splittedItemCount;
			return result;
		}

		protected void InvokeOnChanged() 
		{
			OnChanged?.Invoke();
		}
	}
}
