using Leopotam.EcsLite;
using Zenject;

namespace _Scripts.Core.EcsCore
{
	public class EcsWorldInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			var world = new EcsWorld();
			// Создаю энтити с id = 0, чтобы была возможность иметь default значение
			world.GetPool<DefaultEntityComponent>().Add(world.NewEntity());
			Container
				.Bind<EcsWorld>()
				.FromInstance(world)
				.AsSingle();
		}
	}
}
