using Assets._Scripts.Core.BlocksCore;
using Assets._Scripts.Implementation.InventoryImplementation;
using Assets.Scripts.Core.ChunkGraphicsCore;
using Assets.Scripts.Core.InventoryCore.ItemLogic;
using Assets.Scripts.InventoryCore;
using ChunkCore.LifeTimeControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Undone.WorldsCore
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
