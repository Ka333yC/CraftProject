using System;
using UnityEngine;

namespace Assets.Scripts.InventoryCore
{
	public abstract class Item
	{
		private int _count;

		public Action OnChanged;

		public abstract IItemData ItemData { get; }

		public virtual int Count 
		{
			get
			{
				return _count;
			}

			protected set 
			{
				_count = value;
				InvokeOnChanged();
			}
		}

		/// <summary>
		/// Count не копируется
		/// </summary>
		public abstract Item Duplicate();

		public virtual bool IsSimilar(Item item) 
		{
			return item != null && ItemData == item.ItemData;
		}

		public virtual bool TryAdd(Item sourceItem, int count) 
		{
			if(IsSimilar(sourceItem))
			{
				var countToAdd = Mathf.Min(Count + count, ItemData.StackSize) - Count;
				sourceItem.Count -= countToAdd;
				Count += countToAdd;
				return true;
			}

			return false;
		}

		public virtual void Remove(int count) 
		{
			Count -= count;
		}

		public virtual Item Split(int splittedItemCount)
		{
			var result = Duplicate();
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
