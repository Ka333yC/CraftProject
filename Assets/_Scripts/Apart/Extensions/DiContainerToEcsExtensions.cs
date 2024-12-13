using Assets.Scripts.Core;
using Leopotam.EcsLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace Assets.Scripts.Apart.ZenjectToEcs
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
