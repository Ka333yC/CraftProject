using System.Collections;
using System.Collections.Generic;
using Zenject;

namespace _Scripts.Core.InventoryCore.ItemLogic
{
	public class ItemsContainers : IEnumerable<IItemContainer>
	{
		private readonly List<IItemContainer> _containers = new List<IItemContainer>();

		[Inject]
		private DiContainer _diContainer;

		public IItemContainer this[int itemId] 
		{
			get 
			{
				return _containers[itemId];
			}
		}

		public void Initialize(params IItemContainer[] containers)
		{
			_containers.Add(new AirItemContainer());
			_containers.AddRange(containers);
			for(int i = 0; i < _containers.Count; i++)
			{
				var container = _containers[i];
				_diContainer.Inject(container);
				container.Initialize(i);
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
