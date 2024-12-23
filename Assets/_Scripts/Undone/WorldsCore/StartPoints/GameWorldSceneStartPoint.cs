using Assets.Scripts.Core.ChunkCore.Saving;
using Assets.Scripts.Core.UICore.Core;
using Assets.Scripts.Core.UICore;
using Assets.Scripts.Core.WorldsCore;
using System;
using UnityEngine;
using Zenject;
using TempScripts;
using Assets.Scripts.Core.PlayerCore.FindCoordinatesToSpawn;
using Assets.Scripts.Undone.PlayerCore.Saving;
using Leopotam.EcsLite;
using Assets._Scripts;
using Assets._Scripts.Core.BlocksCore;

namespace Assets.Scripts.Undone
{
	public class GameWorldSceneStartPoint : MonoBehaviour
	{
		[SerializeField]
		private EcsGameStartup _ecsGameStartup;
		[SerializeField]
		private GameObject _playerPrefab;

		[Inject]
		private DiContainer _container;
		//[Inject]
		//private ViewFactory _viewFactory;
		//[Inject]
		//private PageViewStack _viewStack;
		[Inject]
		private EcsWorld _world;

		private void Start()
		{
			_ecsGameStartup.enabled = true;

			//var mainGameWorldView = _viewFactory.CreatePage<MainGameWorldView>();
			//_viewStack.OpenView(mainGameWorldView);

			SpawnPlayer();
		}

		private async void SpawnPlayer()
		{
			var spawnPosition = await _container.Instantiate<PlayersLoader>().GetSavedPosition(default);
			if(!spawnPosition.HasValue)
			{
				var spawnPositionFinder = _container.Instantiate<PlayerSpawnPositionFinder>();
				spawnPosition = await spawnPositionFinder.FindSpawnPosition(default);
			}

			Instantiate(_playerPrefab, spawnPosition.Value, Quaternion.identity);
		}
	}
}
