
using Leopotam.EcsLite;

namespace Assets.Scripts.Apart.Extensions.Ecs
{
	public static class EcsFiltersExtensions
	{
		public static bool Any(this EcsFilter filter)
		{
			return filter.GetEntitiesCount() > 0;
		}
	}
}
