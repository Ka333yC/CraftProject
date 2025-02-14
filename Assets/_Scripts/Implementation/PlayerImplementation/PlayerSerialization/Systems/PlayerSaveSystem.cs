using System;
using _Scripts.Apart.Extensions.Ecs;
using _Scripts.Core.PlayerCore.Components;
using _Scripts.Implementation.PlayerImplementation.PlayerSerialization.Components;
using _Scripts.Undone.PhysicsCore.ObjectPhysicsCore.Components;
using Cysharp.Threading.Tasks;
using Leopotam.EcsLite;
using Zenject;

namespace _Scripts.Implementation.PlayerImplementation.PlayerSerialization.Systems
{
	public class PlayerSaveSystem : IEcsPreInitSystem, IEcsInitSystem, IEcsRunSystem
	{
		[Inject]
		private DiContainer _container;

		private PlayersSerializer _playersSerializer;

		private EcsWorld _world;
		private EcsPool<PlayerSavingTag> _playerSavingPool;
		private EcsPool<NeedSavePlayerTag> _needSavePlayerTagPool;
		private EcsFilter _playersToSaveFilter;
		private EcsFilter _playersSavingFilter;

		public void PreInit(IEcsSystems systems)
		{
			_world = systems.GetWorld();
			_playerSavingPool = _world.GetPool<PlayerSavingTag>();
			_needSavePlayerTagPool = _world.GetPool<NeedSavePlayerTag>();
			_playersToSaveFilter = _world
				.Filter<PlayerComponent>()
				.Inc<ObjectPhysicsComponent>()
				.Inc<NeedSavePlayerTag>()
				.Exc<PlayerSavingTag>()
				.End();
			_playersSavingFilter = _world
				.Filter<PlayerSavingTag>()
				.End();
		}

		public void Init(IEcsSystems systems)
		{
			_playersSerializer = _container.Instantiate<PlayersSerializer>();
		}

		public void Run(IEcsSystems systems)
		{
			if(_playersSavingFilter.Any() || !_playersToSaveFilter.Any())
			{
				return;
			}

			foreach(int playerEntity in _playersToSaveFilter)
			{
				SavePlayer(playerEntity).Forget();
			}
		}

		private async UniTask SavePlayer(int playerEntity) 
		{
			try
			{
				_playerSavingPool.Add(playerEntity);
				await _playersSerializer.Save(playerEntity);
				_needSavePlayerTagPool.Del(playerEntity);
			}
			catch(OperationCanceledException)
			{

			}
			finally
			{
				_playerSavingPool.Del(playerEntity);
			}
		}
	}
}
