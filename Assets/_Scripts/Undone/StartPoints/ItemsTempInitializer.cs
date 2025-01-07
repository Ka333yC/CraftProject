using _Scripts.Core.BlocksCore;
using _Scripts.Core.ChunkGraphicsCore;
using _Scripts.Core.InventoryCore.ItemLogic;
using _Scripts.Implementation.InventoryImplementation;
using UnityEngine;
using Zenject;

namespace _Scripts.TempScripts
{
	public class ItemsTempInitializer : MonoBehaviour
	{
		[SerializeField]
		private ItemContainer[] _itemContainers;
		[SerializeField]
		private Material _chunkMaterial;

		[Inject]
		private ItemsContainers _itemsContainer;
		[Inject]
		private BlocksContainers _blocksContainers;

		public void Initialize()
		{
			_itemsContainer.Initialize(_itemContainers);

			_blocksContainers.Initialize();

			var textureCreator = new ChunkGraphicsTextureCreator(_blocksContainers);
			_chunkMaterial.mainTexture = textureCreator.CreateTexture();
		}
	}
}
