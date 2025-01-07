using _Scripts.Core.BlocksCore;
using _Scripts.Core.InventoryCore.ItemLogic;
using _Scripts.Implementation.DataBaseImplementation.GameWorldDB;
using Leopotam.EcsLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace Assets._Scripts.Implementation.SceneManagement
{
	public class GameWorldSceneDataInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			InstallEcsWorld();
			InstallGameWorldDB();
			InstallItemsContainers();
			InstallBlocksContainers();
		}

		private void InstallEcsWorld()
		{
			Container
				.Bind<EcsWorld>()
				.FromNew()
				.AsSingle();
		}

		private void InstallGameWorldDB()
		{
			Container
				.BindInterfacesAndSelfTo<GameWorldDBCommandExecutor>()
				.FromNew()
				.AsSingle();
		}

		private void InstallItemsContainers()
		{
			Container
				.BindInterfacesAndSelfTo<ItemsContainers>()
				.FromNew()
				.AsSingle();
		}

		private void InstallBlocksContainers()
		{
			Container
				.BindInterfacesAndSelfTo<BlocksContainers>()
				.FromNew()
				.AsSingle();
		}
	}
}
