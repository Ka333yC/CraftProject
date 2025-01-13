using _Scripts.Core.BlocksCore;
using _Scripts.Core.InventoryCore.ItemLogic;
using _Scripts.Implementation.DataBaseImplementation.GameWorldDB;
using Leopotam.EcsLite;
using Zenject;

namespace _Scripts.Implementation.SceneManagement.GameWorldScene
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
