using _Scripts.Core.BlocksCore;
using _Scripts.Core.InventoryCore.ItemLogic;
using _Scripts.Implementation.BlocksImplementation;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace _Scripts.Implementation.InventoryImplementation.Block
{
	[CreateAssetMenu(fileName = "BlockInventoryItemContainer", menuName = "Blocks/Items/Block item container")]
	public class BlockInventoryItemContainer : ItemContainer, IBlockItemData
	{
		[SerializeField]
		private short _stackSize = 64;
		[SerializeField]
		private Sprite _icon;
		[SerializeField] 
		private BlockArchetype _blockArchetype;

		[Inject]
		private EcsWorld _world;

		private int _id;

		public IBlockArchetype BlockArchetype => _blockArchetype;
		public override int Id => _id;
		public override short StackSize => _stackSize;
		public override Sprite Icon => _icon;

		public override Item Create()
		{
			return new BlockInventoryItem(this, _world);
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
