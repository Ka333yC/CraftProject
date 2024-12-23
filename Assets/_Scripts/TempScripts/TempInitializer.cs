using Assets.Scripts.Core.ChunkGraphicsCore;
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
		//[SerializeField]
		//private BaseInventoryItemContainer[] _items;
		[SerializeField]
		private Material _chunkMaterial;

		[Inject]
		private DiContainer _container;

		public override void InstallBindings()
		{
			//Item.Containers.Initialize(_items);
			//Block.Containers.Initialize(_container);
			InitializeChunkMaterial();
		}

		private void InitializeChunkMaterial()
		{
			//var chunkGraphicsTextureCreator = new ChunkGraphicsTextureCreator();
			//_chunkMaterial.mainTexture = chunkGraphicsTextureCreator.CreateTexture();
		}
	}
}
