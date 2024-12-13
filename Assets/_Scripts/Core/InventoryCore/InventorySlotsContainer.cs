using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.InventoryCore;

namespace Assets.Scripts.Core.InventoryCore
{
	public class InventorySlotsContainer : IEnumerable<InventorySlot>
	{
		protected readonly List<InventorySlot> _container = new List<InventorySlot>(1);

		public InventorySlot this[int index] 
		{
			get 
			{
				return _container[index];
			}
		}

		public IEnumerator<InventorySlot> GetEnumerator()
		{
			return _container.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void AddSlot(InventorySlot slot) 
		{
			_container.Add(slot);
			slot.Container = this;
		}

		public bool TryAddItem(Item sourceItem, int count) 
		{
			int startSourceItemCount = sourceItem.Count;
			// Пытаемся добавить в такой же предмет, куда можно "стакнуть"
			if(TryAddItem(sourceItem, count, true)) 
			{
				int addedCount = startSourceItemCount - sourceItem.Count;
				count -= addedCount;
			}

			// Если ещё остались предметы для добавления, пытаемся добавить в пустые слоты
			if(count != 0)
			{
				TryAddItem(sourceItem, count, false);
			}

			return startSourceItemCount != sourceItem.Count;
		}

		private bool TryAddItem(Item sourceItem, int count, bool hasItem) 
		{
			bool isAdded = false;
			int startSourceItemCount = sourceItem.Count;
			foreach(var slot in _container)
			{
				if(slot.HasItem == hasItem && slot.TryAddItem(sourceItem, count))
				{
					int addedCount = startSourceItemCount - sourceItem.Count;
					count -= addedCount;
					isAdded = true;
					if(count == 0) 
					{
						break;
					}
				}
			}

			return isAdded;
		}
	}
}
