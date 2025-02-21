using _Scripts.Core.SerializableDataCore;
using Zenject;

namespace _Scripts.Core.SettingsCore
{
	public class SettingsSystemsManagerInstaller : MonoInstaller
	{
		[Inject]
		private SerializableDataSystemsManager _serializableDataSystemsManager;
		
		public override void InstallBindings()
		{
			var settingsSystemsManager = new SettingsSystemsManager();
			_serializableDataSystemsManager.AddSystem(settingsSystemsManager);
			Container
				.BindInterfacesAndSelfTo<SettingsSystemsManager>()
				.FromInstance(settingsSystemsManager)
				.AsSingle();
		}
	}
}
