//using Assets.Scripts.Core.ChunkCore.Saving;
//using Assets.Scripts.Core.UICore.Core;
//using Assets.Scripts.Core.UICore;
//using Assets.Scripts.Core.WorldsCore;
//using System;
//using UnityEngine;
//using Zenject;
//using Assets.Scripts.Implementation.UI.GameWorldSceneUI.MainGameWorld;
//using TempScripts;
//using Assets.Scripts.Core.PlayerCore.FindCoordinatesToSpawn;
//using Assets.Scripts.Undone.PlayerCore.Saving;
//using Leopotam.EcsLite;

//namespace Assets.Scripts.Undone
//{
//	public class GameWorldSceneStartPoint : MonoBehaviour
//	{
//		[SerializeField]
//		private EcsGameStartup _ecsGameStartup;
//		[SerializeField]
//		private Player _playerPrefab;

//		[Inject]
//		private DiContainer _container;
//		[Inject]
//		private ViewFactory _viewFactory;
//		[Inject]
//		private PageViewStack _viewStack;
//		[Inject]
//		private EcsWorld _world;

//		private void Start()
//		{
//			_ecsGameStartup.enabled = true;

//			var mainGameWorldView = _viewFactory.CreatePage<MainGameWorldView>();
//			_viewStack.OpenView(mainGameWorldView);

//			SpawnPlayer();
//		}

//		private async void SpawnPlayer()
//		{
//			var savedPosition = await _container.Instantiate<PlayersLoader>().GetSavedPosition(default);
//			if(!savedPosition.HasValue)
//			{
//				var coordinatesFinder = new PlayerSpawnPositionFinder(_world);
//				savedPosition = await coordinatesFinder.FindSpawnPosition(default);
//			}

//			Instantiate(_playerPrefab, savedPosition.Value, Quaternion.identity);
//		}
//	}
//}
