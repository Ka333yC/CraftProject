using _Scripts.Core.InventoryCore.ItemLogic;
using _Scripts.Undone.WorldsCore;
using Zenject;

namespace _Scripts.Implementation.DataBaseImplementation.WorldsDataDB
{
	public class GlobalDataInstaller : MonoInstaller 
	{
		public override void InstallBindings()
		{
			InstallScenesLauncher();
			InstallGameWorldsDBCommandExecutor();
		}

		private void InstallScenesLauncher()
		{
			Container
				.BindInterfacesAndSelfTo<ScenesLauncher>()
				.FromNew()
				.AsSingle();
		}

		private void InstallGameWorldsDBCommandExecutor()
		{
			Container
				.BindInterfacesAndSelfTo<GameWorldsDBCommandExecutor>()
				.FromNew()
				.AsSingle();
		}
	}
}
