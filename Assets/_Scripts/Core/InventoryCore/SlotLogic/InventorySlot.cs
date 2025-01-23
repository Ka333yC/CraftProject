using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Core.InventoryCore.ItemLogic;
using JetBrains.Annotations;
using UnityEngine;

namespace _Scripts.Core.InventoryCore.SlotLogic
{
	public class InventorySlot
	{
		private readonly List<IInventorySlotFilter> _filters;

		private Item _item;

		/// <summary>
		/// Вызывается если изменился сам Item или одно из его свойств
		/// </summary>
		public event Action OnItemChanged;

		public Item Item 
		{
			get => _item;
			private set
			{
				if(_item != null)
				{
					_item.OnChanged -= InvokeOnItemChanged;
					_item.OnChanged -= SetNullToItemIfCountIsZero;
				}

				_item = value;
				if(_item != null)
				{
					_item.OnChanged += InvokeOnItemChanged;
					_item.OnChanged += SetNullToItemIfCountIsZero;
				}

				InvokeOnItemChanged();
			}
		}

		public bool HasItem => _item != null;

		public InventorySlot()
		{
			_filters = new List<IInventorySlotFilter>(0);
		}

		public InventorySlot(List<IInventorySlotFilter> filters)
		{
			_filters = filters;
		}

		public bool TrySetItem(Item item)
		{
			if(!IsPassFilters(item))
			{
				return false;
			}

			Item = item;
			return true;
		}

		public int AddItem(Item sourceItem, int count)
		{
			if(!IsPassFilters(sourceItem))
			{
				return 0;
			}
			
			if(HasItem)
			{
				return _item.Add(sourceItem, count);
			}

			Item = sourceItem.Split(count);
			return count;
		}

		public bool IsPassFilters(Item item)
		{
			foreach(var filter in _filters)
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

		protected void SetNullToItemIfCountIsZero() 
		{
			if(Item != null && Item.Count == 0)
			{
				Item = null;
			}
		}
	}
}
