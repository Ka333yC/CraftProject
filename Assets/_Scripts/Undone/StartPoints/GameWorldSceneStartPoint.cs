using System;
using System.IO;
using System.Threading.Tasks;
using _Scripts.Core.PlayerCore;
using _Scripts.Core.UICore;
using _Scripts.Core.UICore.Page;
using _Scripts.Implementation.DataBaseImplementation.GameWorldDB;
using _Scripts.Implementation.DataBaseImplementation.GameWorldsDataDB;
using _Scripts.Implementation.DataBaseImplementation.GameWorldsDataDB.Tables.GameWorldParametersTable;
using _Scripts.Implementation.PlayerImplementation;
using _Scripts.Implementation.PlayerImplementation.PlayerSerialization;
using _Scripts.Implementation.SceneManagement;
using _Scripts.Implementation.SceneManagement.GameWorldScene;
using _Scripts.Implementation.UIImplementation.GameWorldSceneUI.MainGameWorldPage;
using _Scripts.Implementation.UIImplementation.GameWorldSceneUI.PlayerToolbarPopUp;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Scripts.Undone.StartPoints
{
	public class GameWorldSceneStartPoint : MonoBehaviour
	{
		[SerializeField]
		private EcsGameStartup _ecsGameStartup;
		[SerializeField]
		private ItemsTempInitializer _itemsInitializer;
		[SerializeField]
		private Player _playerPrefab;
		[SerializeField]
		private CustomGameWorldParameters _debugGameWorldParameters;

		[Inject]
		private DiContainer _container;
		[Inject]
		private ScenesLauncher _scenesLauncher;
		[Inject]
		private ViewFactory _viewFactory;
		[Inject]
		private PageViewStack _viewStack;
		[Inject]
		private GameWorldsDBCommandExecutor _gameWorldsDBCommandExecutor;
		[Inject]
		private GameWorldDBCommandExecutor _gameWorldDBCommandExecutor;

		private async void Start()
		{
			var worldParameters = await GetWorldParameters();
			await OpenGameWorldDataBase(worldParameters);

			_itemsInitializer.Initialize();

			_ecsGameStartup.enabled = true;

			var mainGameWorldView = _viewFactory.CreatePage<MainGameWorldView>();
			_viewStack.OpenView(mainGameWorldView);

			await SpawnPlayer();

			var toolbarPopUp = _viewFactory.CreatePopUp<PlayerToolbarPopUpView>();
			_viewStack.ActiveView.PopUpStack.OpenView(toolbarPopUp);
		}

		private async Task<GameWorldParameters> GetWorldParameters() 
		{
#if UNITY_EDITOR
			if(!_scenesLauncher.GameWorldLaunchParameters.HasValue)
			{
				return _debugGameWorldParameters.GetGameWorldParameters();
			}
#endif
			var worldId = _scenesLauncher.GameWorldLaunchParameters.Value.WorldId;
			var worldParameters = await GetWorldParametersFromDB(worldId);
			return worldParameters;
		}

		private async Task<GameWorldParameters> GetWorldParametersFromDB(int worldId)
		{
			var commandExecutor = _gameWorldsDBCommandExecutor.CommandExecutor;
			var command = GameWorldParameters.SelectWhereIdCommand;
			command.Id = worldId;
			var worldParameters = new GameWorldParameters();
			await commandExecutor.ExecuteReaderAsync(command, (reader) =>
			{
				if(!reader.Read())
				{
					throw new ArgumentException("The request returned nothing.");
				}

				worldParameters.Id = worldId;
				worldParameters.Name = reader.GetString(0);
				worldParameters.Seed = reader.GetInt32(1);
				worldParameters.WorldFolderPath = reader.GetString(2);
			});

			return worldParameters;
		}

		private async UniTask OpenGameWorldDataBase(GameWorldParameters worldParameters) 
		{
			if(!Directory.Exists(worldParameters.WorldFolderPath))
			{
				Directory.CreateDirectory(worldParameters.WorldFolderPath);
			}

			await _gameWorldDBCommandExecutor.Initialize(worldParameters);
		}

		private async UniTask SpawnPlayer()
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
