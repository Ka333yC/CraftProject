using Zenject;

namespace Assets.Scripts.Undone.SettingsCore
{
	public class SettingsSystemsManagerInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			var manager = new SettingsSystemsManager();
			manager.Initialize().Forget();
			Container
				.BindInterfacesAndSelfTo<SettingsSystemsManager>()
				.FromInstance(manager)
				.AsSingle();
		}
	}
}
