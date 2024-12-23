using Assets.Scripts.Core.InventoryCore.ItemLogic;
using Assets.Scripts.InventoryCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts.Implementation.InventoryImplementation
{
	public abstract class ItemContainer : ScriptableObject, IItemContainer
	{
		public abstract int Id { get; }
		public abstract short StackSize { get; }
		public abstract Sprite Icon { get; }

		public abstract void Initialize(int id);
		public abstract Item Create();
	}
}
