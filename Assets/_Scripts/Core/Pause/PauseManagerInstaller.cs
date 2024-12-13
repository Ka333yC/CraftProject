using Zenject;

namespace Assets.Scripts.Core.Pause
{
	public class PauseManagerInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			Container
				.Bind<PauseManager>()
				.FromNew()
				.AsSingle();
		}
	}
}
