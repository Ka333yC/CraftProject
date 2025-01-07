using Leopotam.EcsLite;

namespace _Scripts.Apart.Extensions.Ecs
{
	public static class EcsPoolsExtensions
	{
		public static ref T AddOrGet<T>(this EcsPool<T> pool, int entity) where T : struct
		{
			if(pool.Has(entity))
			{
				return ref pool.Get(entity);
			}

			return ref pool.Add(entity);
		}

		public static void AddIfNotHas<T>(this EcsPool<T> pool, int entity) where T : struct
		{
			if(!pool.Has(entity))
			{
				pool.Add(entity);
			}
		}

		public static void DelIfHas<T>(this EcsPool<T> pool, int entity) where T : struct
		{
			if(pool.Has(entity))
			{
				pool.Del(entity);
			}
		}
	}
}
