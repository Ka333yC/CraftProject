using Leopotam.EcsLite;

namespace Assets.Scripts.Apart.Extensions.Ecs
{
	public class DelHereSystem<T> : IEcsPreInitSystem, IEcsRunSystem where T : struct
	{
		private EcsPool<T> _pool;
		private EcsFilter _filter;

		public void PreInit(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();
			_pool = world.GetPool<T>();
			_filter = world.Filter<T>().End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach(var entity in _filter)
			{
				_pool.Del(entity);
			}
		}
	}
}
