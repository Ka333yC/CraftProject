using Zenject;

namespace _Scripts.Implementation.DataBaseImplementation.GameWorldDB
{
	public class GameWorldDBInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			Container
				.BindInterfacesAndSelfTo<GameWorldDBCommandExecutor>()
				.FromNew()
				.AsSingle();
		}
	}
}
