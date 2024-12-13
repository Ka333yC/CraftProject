using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Core.InventoryCore.ItemLogic.AirItem;

namespace Assets.Scripts.Core.InventoryCore.ItemLogic
{
	public class ItemsContainers : IEnumerable<IItemContainer>
	{
		private readonly List<IItemContainer> _containers = new List<IItemContainer>();

		public IItemContainer this[int itemId] 
		{
			get 
			{
				return _containers[itemId];
			}
		}

		public void Initialize(params IItemContainer[] containers)
		{
			_containers.Clear();
			_containers.Add(new AirItemContainer());
			_containers.AddRange(containers);
			for(int i = 0; i < _containers.Count; i++)
			{
				_containers[i].Initialize(i);
			}
		}

		public IEnumerator<IItemContainer> GetEnumerator()
		{
			return _containers.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
