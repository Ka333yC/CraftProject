using _Scripts.Apart.Extensions.Ecs;
using _Scripts.Core.PhysicsCore.ObjectPhysicsCore.Components;
using _Scripts.Core.PhysicsCore.ObjectPhysicsCore.PositionsUpdate.Components;
using _Scripts.Core.PlayerCore.Components;
using _Scripts.Implementation.PlayerImplementation.PlayerSerialization.Components;
using Leopotam.EcsLite;

namespace _Scripts.Implementation.PlayerImplementation.PlayerSerialization.Systems
{
	public class MarkChangedPlayersNeedToSaveSystem : IEcsPreInitSystem, IEcsRunSystem
	{
		private EcsPool<NeedSavePlayerTag> _needSavePlayerPool;
		private EcsFilter _playerChangedPositionFilter;

		public void PreInit(IEcsSystems systems)
		{
			var world = systems.GetWorld();
			_needSavePlayerPool = world.GetPool<NeedSavePlayerTag>();
			_playerChangedPositionFilter = world
				.Filter<PlayerComponent>()
				.Inc<ObjectPhysicsComponent>()
				.Inc<BlockPositionChangedComponent>()
				.End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach(var playerEntity in _playerChangedPositionFilter)
			{
				_needSavePlayerPool.AddIfNotHas(playerEntity);
			}
		}
	}
}
