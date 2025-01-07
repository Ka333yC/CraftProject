using _Scripts.Undone.WorldsCore;
using Zenject;

namespace _Scripts.Implementation.DataBaseImplementation.WorldsDataDB
{
	public class WorldsDataInstaller : MonoInstaller 
	{
		public override void InstallBindings()
		{
			Container
				.BindInterfacesAndSelfTo<WorldLauncher>()
				.FromNew()
				.AsSingle();
			Container
				.BindInterfacesAndSelfTo<WorldsDataDBCommandExecutor>()
				.FromNew()
				.AsSingle();
		}
	}
}
