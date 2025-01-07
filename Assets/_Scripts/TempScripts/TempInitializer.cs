using _Scripts.Core.BlocksCore;
using _Scripts.Core.ChunkGraphicsCore;
using _Scripts.Core.InventoryCore.ItemLogic;
using _Scripts.Implementation.InventoryImplementation;
using UnityEngine;
using Zenject;

namespace _Scripts.TempScripts
{
	public class TempInitializer : MonoInstaller
	{
		[SerializeField]
		private ItemContainer[] _itemContainers;
		[SerializeField]
		private Material _chunkMaterial;

		public override void InstallBindings()
		{
			var itemsContainer = new ItemsContainers();
			Container
				.Bind<ItemsContainers>()
				.FromInstance(itemsContainer)
				.AsSingle();
			itemsContainer.Initialize(_itemContainers);

			var blocksContainers = Container.Instantiate<BlocksContainers>();
			Container
				.Bind<BlocksContainers>()
				.FromInstance(blocksContainers)
				.AsSingle();
			blocksContainers.Initialize();

			var textureCreator = new ChunkGraphicsTextureCreator(blocksContainers);
			_chunkMaterial.mainTexture = textureCreator.CreateTexture();
		}
	}
}
