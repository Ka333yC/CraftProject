using _Scripts.Implementation.PlayerImplementation;
using _Scripts.Implementation.PlayerImplementation.PlayerSerialization;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace _Scripts.Undone.WorldsCore.StartPoints
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
			var playersSerializer = _container.Instantiate<PlayersSerializer>();
			var spawnPosition = await playersSerializer.GetSavedPosition();
			if(!spawnPosition.HasValue)
			{
				var spawnPositionFinder = _container.Instantiate<PlayerSpawnPositionFinder>();
				spawnPosition = await spawnPositionFinder.FindSpawnPosition();
			}

			Instantiate(_playerPrefab, spawnPosition.Value, Quaternion.identity);
		}
	}
}
