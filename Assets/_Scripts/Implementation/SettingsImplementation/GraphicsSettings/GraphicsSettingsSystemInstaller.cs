using Zenject;

namespace _Scripts.Implementation.SettingsImplementation.GraphicsSettings
{
	public class GraphicsSettingsSystemInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			Container
				.BindInterfacesAndSelfTo<GraphicsSettingsSystem>()
				.FromNew()
				.AsSingle();
		}
	}
}
