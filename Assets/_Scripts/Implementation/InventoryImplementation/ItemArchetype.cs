using _Scripts.Core.InventoryCore.ItemLogic;
using UnityEngine;

namespace _Scripts.Implementation.InventoryImplementation
{
	public abstract class ItemArchetype : ScriptableObject, IItemArchetype
	{
		public abstract int Id { get; }
		public abstract short StackSize { get; }
		public abstract Sprite Icon { get; }

		public abstract void Initialize(int id);
		public abstract Item Create();
	}
}
