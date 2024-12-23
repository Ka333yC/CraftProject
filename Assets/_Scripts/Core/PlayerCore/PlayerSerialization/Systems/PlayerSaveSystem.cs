using Assets.Scripts.Apart.Extensions.Ecs;
using Assets.Scripts.Core.ChunkCore.LifeTimeControl.Components;
using Assets.Scripts.Core.ChunkCore.Saving.Components;
using Assets.Scripts.Undone.PlayerCore.Saving.Components;
using ChunkCore.ChunksContainerScripts.Components;
using Cysharp.Threading.Tasks;
using Leopotam.EcsLite;
using PhysicsCore.ObjectPhysics.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace Assets.Scripts.Undone.PlayerCore.Saving.Systems
{
	public class PlayerSaveSystem : IEcsPreInitSystem, IEcsInitSystem, IEcsRunSystem
	{
		[Inject]
		private DiContainer _container;

		private PlayersSerializer _playersSerializer;

		private EcsWorld _world;
		private EcsPool<ObjectPhysicsComponent> _objectPhysicsPool;
		private EcsPool<PlayerSavingTag> _playerSavingPool;
		private EcsPool<NeedSavePlayerTag> _needSavePlayerTagPool;
		private EcsFilter _playerToSaveFilter;
		private EcsFilter _playerSavingFilter;

		public void PreInit(IEcsSystems systems)
		{
			_world = systems.GetWorld();
			_objectPhysicsPool = _world.GetPool<ObjectPhysicsComponent>();
			_playerSavingPool = _world.GetPool<PlayerSavingTag>();
			_needSavePlayerTagPool = _world.GetPool<NeedSavePlayerTag>();
			_playerToSaveFilter = _world
				.Filter<PlayerComponent>()
				.Inc<ObjectPhysicsComponent>()
				.Inc<NeedSavePlayerTag>()
				.Exc<PlayerSavingTag>()
				.End();
			_playerSavingFilter = _world
				.Filter<PlayerSavingTag>()
				.End();
		}

		public void Init(IEcsSystems systems)
		{
			_playersSerializer = _container.Instantiate<PlayersSerializer>();
		}

		public void Run(IEcsSystems systems)
		{
			if(_playerSavingFilter.Any() || !_playerToSaveFilter.Any())
			{
				return;
			}

			foreach(int playerEntity in _playerToSaveFilter)
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
