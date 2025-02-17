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
			InstallItemsArchetypes();
			InstallBlocksArchetypes();
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

		private void InstallItemsArchetypes()
		{
			Container
				.BindInterfacesAndSelfTo<ItemsArchetypes>()
				.FromNew()
				.AsSingle();
		}

		private void InstallBlocksArchetypes()
		{
			Container
				.BindInterfacesAndSelfTo<BlocksArchetypes>()
				.FromNew()
				.AsSingle();
		}
	}
}
