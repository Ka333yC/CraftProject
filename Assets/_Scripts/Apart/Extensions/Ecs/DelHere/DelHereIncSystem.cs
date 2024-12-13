using Leopotam.EcsLite;

namespace Assets.Scripts.Apart.Extensions.Ecs.DelHere
{
	public class DelHereIncSystem<Del, Inc> : IEcsPreInitSystem, IEcsRunSystem
		where Del : struct
		where Inc : struct
	{
		private EcsPool<Del> _pool;
		private EcsFilter _filter;

		public void PreInit(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();
			_pool = world.GetPool<Del>();
			_filter = world
				.Filter<Del>()
				.Inc<Inc>()
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
