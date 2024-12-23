using Assets.Scripts.Undone.PlayerCore.Saving.Components;
using Extensions.Ecs;
using Leopotam.EcsLite;
using PhysicsCore.ObjectPhysics.Components;
using PhysicsCore.ObjectPhysics.PositionUpdater.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Undone.PlayerCore.Saving.Systems
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
