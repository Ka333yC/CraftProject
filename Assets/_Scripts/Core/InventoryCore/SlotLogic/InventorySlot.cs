using System;
using System.Collections.Generic;
using Assets.Scripts.InventoryCore;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Core.InventoryCore
{
	public class InventorySlot
	{
		public readonly List<IInventorySlotFilter> Filters = new List<IInventorySlotFilter>(0);

		public InventorySlotsContainer Container;

		private Item _item;

		/// <summary>
		/// Вызывается если изменился сам Item или свойство у Item
		/// </summary>
		public event Action OnItemChanged;

		[CanBeNull]
		public Item Item 
		{
			get 
			{
				return _item;
			}

			protected set
			{
				if(_item != null)
				{
					_item.OnChanged -= InvokeOnItemChanged;
					_item.OnChanged -= SetItemNullIfCountZero;
				}

				_item = value;
				if(_item != null)
				{
					_item.OnChanged += InvokeOnItemChanged;
					_item.OnChanged += SetItemNullIfCountZero;
				}

				InvokeOnItemChanged();
			}
		}

		public bool HasItem 
		{
			get 
			{
				return Item != null;
			}
		}

		public virtual bool TryAddItem(Item sourceItem, int count)
		{
			if(!IsPassFilters(sourceItem)) 
			{
				return false;
			}

			if(!HasItem)
			{
				Item = sourceItem.Split(count);
				return true;
			}

			return Item.TryAdd(sourceItem, count);
		}

		public virtual bool IsPassFilters(Item item)
		{
			foreach(var filter in Filters)
			{
				if(!filter.IsPass(item))
				{
					return false;
				}
			}

			return true;
		}

		protected void InvokeOnItemChanged() 
		{
			OnItemChanged?.Invoke();
		}

		protected void SetItemNullIfCountZero() 
		{
			if(Item != null && Item.Count <= 0)
			{
				Item = null;
			}
		}
	}
}
