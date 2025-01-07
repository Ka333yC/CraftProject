using Leopotam.EcsLite;

namespace _Scripts.Apart.Extensions.Ecs.DelHere
{
	public class DelHereExcSystem<Del, Exc> : IEcsPreInitSystem, IEcsRunSystem
		where Del : struct
		where Exc : struct
	{
		private EcsPool<Del> _pool;
		private EcsFilter _filter;

		public void PreInit(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();
			_pool = world.GetPool<Del>();
			_filter = world
				.Filter<Del>()
				.Exc<Exc>()
				.End();
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
