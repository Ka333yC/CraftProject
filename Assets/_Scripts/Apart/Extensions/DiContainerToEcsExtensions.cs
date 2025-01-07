using Leopotam.EcsLite;
using Zenject;

namespace _Scripts.Apart.Extensions
{
	public static class DiContainerToEcsExtensions
	{
		public static void InjectEcsSystems(this DiContainer container, EcsSystems systems)
		{
			var systemsList = systems.GetAllSystems();
			foreach(var system in systemsList)
			{
				container.Inject(system);
			}
		}

		public static void InjectEcsWorld(this DiContainer container, EcsWorld world)
		{
			container
				.Bind<EcsWorld>()
				.FromInstance(world)
				.AsSingle();
		}
	}
}
