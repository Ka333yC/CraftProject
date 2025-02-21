using _Scripts.Core.SettingsCore;
using Zenject;

namespace _Scripts.Core.SerializableDataCore
{
	public class SerializableDataSystemsManagerInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			var manager = new SerializableDataSystemsManager();
			manager.Initialize().Forget();
			Container
				.BindInterfacesAndSelfTo<SerializableDataSystemsManager>()
				.FromInstance(manager)
				.AsSingle();
		}
	}
}