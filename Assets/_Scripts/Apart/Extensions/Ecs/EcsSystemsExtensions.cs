using System.Collections.Generic;
using _Scripts.Apart.Extensions.Ecs.DelHere;
using Leopotam.EcsLite;

namespace _Scripts.Apart.Extensions.Ecs
{
	public static class EcsSystemsExtensions
	{
		public static IEcsSystems DelHere<T>(this IEcsSystems systems) where T : struct
		{
			return systems.Add(new DelHereSystem<T>());
		}

		public static IEcsSystems AddRange(this IEcsSystems systems, IEnumerable<IEcsSystem> systemsToAdd)
		{
			foreach(var systemToAdd in systemsToAdd)
			{
				systems.Add(systemToAdd);
			}

			return systems;
		}
	}
}
