using System.Collections;
using System.Collections.Generic;
using Zenject;

namespace _Scripts.Core.InventoryCore.ItemLogic
{
	public class ItemsArchetypes : IEnumerable<IItemArchetype>
	{
		private readonly List<IItemArchetype> _archetypes = new List<IItemArchetype>();

		[Inject]
		private DiContainer _diContainer;

		public IItemArchetype this[int itemId] => _archetypes[itemId];

		public void Initialize(params IItemArchetype[] archetypes)
		{
			_archetypes.Add(new AirItemArchetype());
			_archetypes.AddRange(archetypes);
			for(int i = 0; i < _archetypes.Count; i++)
			{
				var archetype = _archetypes[i];
				_diContainer.Inject(archetype);
				archetype.Initialize(i);
			}
		}

		public IEnumerator<IItemArchetype> GetEnumerator()
		{
			return _archetypes.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
