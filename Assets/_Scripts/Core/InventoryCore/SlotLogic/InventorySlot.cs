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
		protected readonly IInventorySlotFilter[] _filters;

		private Item _item;

		/// <summary>
		/// Вызывается если изменился сам Item или одно из его свойств
		/// </summary>
		public event Action OnItemChanged;

		[CanBeNull]
		public Item Item 
		{
			get => _item;
			protected set
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

		public bool HasItem => Item != null;

		public InventorySlot()
		{
			_filters = Array.Empty<IInventorySlotFilter>();
		}

		public InventorySlot(IEnumerable<IInventorySlotFilter> filters)
		{
			_filters = filters.ToArray();
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

			// ReSharper disable once PossibleNullReferenceException
			if(!Item.IsSimilar(sourceItem))
			{
				return false;
			}
			
			var fitsCount = Mathf.Min(Item.Count + count, Item.ItemData.StackSize) - Item.Count;
			sourceItem.Count -= fitsCount;
			Item.Count += fitsCount;
			return true;
		}

		public virtual bool IsPassFilters(Item item)
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
