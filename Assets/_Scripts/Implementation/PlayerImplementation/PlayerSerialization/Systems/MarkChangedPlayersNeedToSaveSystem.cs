using _Scripts.Apart.Extensions.Ecs;
using _Scripts.Core.PlayerCore.Components;
using _Scripts.Implementation.PlayerImplementation.PlayerSerialization.Components;
using _Scripts.Undone.PhysicsCore.ObjectPhysicsCore.Components;
using _Scripts.Undone.PhysicsCore.ObjectPhysicsCore.PositionsUpdate.Components;
using Leopotam.EcsLite;

namespace _Scripts.Implementation.PlayerImplementation.PlayerSerialization.Systems
{
	public class MarkChangedPlayersNeedToSaveSystem : IEcsPreInitSystem, IEcsRunSystem
	{
		private EcsPool<NeedSavePlayerTag> _needSavePlayerPool;
		private EcsFilter _playersChangedPositionFilter;

		public void PreInit(IEcsSystems systems)
		{
			var world = systems.GetWorld();
			_needSavePlayerPool = world.GetPool<NeedSavePlayerTag>();
			_playersChangedPositionFilter = world
				.Filter<PlayerComponent>()
				.Inc<ObjectPhysicsComponent>()
				.Inc<BlockPositionChangedComponent>()
				.End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach(var playerEntity in _playersChangedPositionFilter)
			{
				_needSavePlayerPool.AddIfNotHas(playerEntity);
			}
		}
	}
}
