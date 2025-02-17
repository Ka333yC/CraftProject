using _Scripts.Core.BlocksCore;
using _Scripts.Core.InventoryCore.ItemLogic;
using _Scripts.Implementation.BlocksImplementation;
using _Scripts.Implementation.BlocksImplementation.WoodLogBlock;
using _Scripts.Implementation.InventoryImplementation.Block;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace _Scripts.Implementation.InventoryImplementation.WoodLogBlock
{
	[CreateAssetMenu(fileName = "ItemArchetype", menuName = "Items/Archetypes/Wood log block item archetype")]
	public class WoodLogBlockItemArchetype : ItemArchetype, IBlockItemData
	{
		[SerializeField]
		private short _stackSize = 64;
		[SerializeField]
		private Sprite _icon;
		[SerializeField] 
		private WoodLogBlockArchetype _blockArchetype;

		[Inject]
		private EcsWorld _world;

		private int _id;
		
		public WoodLogBlockArchetype BlockArchetype => _blockArchetype;
		public override int Id => _id;
		public override short StackSize => _stackSize;
		public override Sprite Icon => _icon;
		IBlockArchetype IBlockItemData.BlockArchetype => _blockArchetype;

		public override Item Create()
		{
			return new WoodLogBlockItem(this, _world);
		}

		public override void Initialize(int id)
		{
			_id = id;
			_blockArchetype.Initialize(id);
		}

		[Inject]
		private void Inject(DiContainer diContainer) 
		{
			diContainer.Inject(_blockArchetype);
		}
	}
}