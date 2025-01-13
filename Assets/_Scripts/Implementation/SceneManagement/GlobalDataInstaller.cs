using _Scripts.Implementation.DataBaseImplementation.GameWorldsDataDB;
using Zenject;

namespace _Scripts.Implementation.SceneManagement
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
