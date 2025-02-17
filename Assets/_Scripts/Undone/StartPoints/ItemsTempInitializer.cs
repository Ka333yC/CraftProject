using _Scripts.Core.BlocksCore;
using _Scripts.Core.ChunkGraphicsCore;
using _Scripts.Core.InventoryCore.ItemLogic;
using _Scripts.Implementation.InventoryImplementation;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace _Scripts.Undone.StartPoints
{
	public class ItemsTempInitializer : MonoBehaviour
	{
		[SerializeField]
		private ItemArchetype[] _itemArchetypes;
		[SerializeField]
		private Material _chunkMaterial;

		[Inject]
		private ItemsArchetypes _itemsArchetypes;
		[Inject]
		private BlocksArchetypes _blocksArchetypes;

		public void Initialize()
		{
			_itemsArchetypes.Initialize(_itemArchetypes);

			_blocksArchetypes.Initialize();

			var textureCreator = new ChunkGraphicsTextureCreator(_blocksArchetypes);
			_chunkMaterial.mainTexture = textureCreator.CreateTexture();
		}
	}
}
