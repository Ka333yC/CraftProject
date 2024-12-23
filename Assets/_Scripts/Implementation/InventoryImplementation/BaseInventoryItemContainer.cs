using System;
using Assets.Scripts.Core.InventoryCore.ItemLogic;
using Assets.Scripts.InventoryCore;
using ChunkCore.BlockData;
using UnityEngine;

namespace Assets.Scripts.Undone.Realization.Blocks.InventoryBlockPresentation
{
	public abstract class BaseInventoryItemContainer : ScriptableObject, IItemContainer
	{
		public virtual int Id { get; private set; }

		[field: SerializeField] 
		public short StackSize { get; private set; } = 64;

		[field: SerializeField] 
		public Sprite Icon { get; private set; }

		public abstract Item Create();

		public virtual void Initialize(int id)
		{
			Id = id;
		}
	}
}
